﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using Microsoft.Win32;

namespace DotNetMarche.Infrastructure.Data
{
	public static class DataAccess
	{

		#region TransactionManagement

		private const String TransactionKeyBase = "C51B2130-6CCF-4b12-9CD3-778768B5B07E";
		private static String GetKeyFromConnName(String connectionName)
		{
			return TransactionKeyBase + connectionName ?? String.Empty;
		}

		internal static GlobalTransactionManager.TransactionToken CreateConnection()
		{
			return CreateConnection(null);
		}

		/// <summary>
		/// This is the Connetion creation function, this function should enlist in the
		/// current transaction as well reusing the same connection for all the call
		/// inside a global transaction to consume less resources.
		/// </summary>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		internal static GlobalTransactionManager.TransactionToken CreateConnection(string connectionName)
		{
			if (GlobalTransactionManager.IsInTransaction)
			{
				Object connData = GlobalTransactionManager.TransactionContext.Get(GetKeyFromConnName(connectionName));
				if (null != connData)
				{
				//We already created the connection for this database in this transaction.
				return GlobalTransactionManager.Enlist(DoNothing);
				}
			} 
			//If we reach here we does not have a global transaction or we never creted connectino for this transaction
			//This is the first time we connect to this database inside this transaction.
			ConnectionData newConnData = new ConnectionData(connectionName);
			GlobalTransactionManager.TransactionToken token =
				GlobalTransactionManager.Enlist(newConnData.CloseConnection);
			token.SetInTransactionContext(GetKeyFromConnName(connectionName), newConnData);
			return token;
		}

		private static void DoNothing(Boolean b) { }

		/// <summary>
		/// Keep all the objects needed to access the database in a single
		/// class. 
		/// </summary>
		private class ConnectionData
		{
			public readonly DbConnection Connection;
			public readonly DbTransaction Transaction;
			public readonly DbProviderFactory Factory;
			/// <summary>
			/// In the constructor we creates all the object we need to access the database.
			/// </summary>
			/// <param name="connectionName"></param>
			public ConnectionData(String connectionName)
			{
				ConnectionStringSettings cn;
				if (String.IsNullOrEmpty(connectionName))
					cn = ConfigurationRegistry.MainConnectionString;
				else
					cn = ConfigurationRegistry.ConnectionString(connectionName);

				Factory = DbProviderFactories.GetFactory(cn.ProviderName);
				Connection = Factory.CreateConnection();
				Connection.ConnectionString = cn.ConnectionString;
				Connection.Open();
				Transaction = Connection.BeginTransaction();
			}

			public void CloseConnection(Boolean shouldCommit)
			{
				if (shouldCommit)
					Transaction.Commit();
				else
					Transaction.Rollback();
				Transaction.Dispose();
				Connection.Dispose();
			}
		}

		#endregion

		#region Static Initialization

		static DataAccess()
		{
			mParametersFormat = new Dictionary<Type, String>();
			mParametersFormat.Add(typeof(SqlCommand), "@{0}");
		}

		#endregion

		#region Handling of connection

		/// <summary>
		/// To know at runtime the format of the parameter we need to check the 
		/// <see cref="System.Data.Common.DbConnection.GetSchema(String)"/> method. 
		/// To cache the format we use a dictionary with command type as a key and
		/// string format as value.
		/// </summary>
		private readonly static Dictionary<Type, String> mParametersFormat;

		/// <summary>
		/// Gets the format of the parameter, to avoid query the schema the parameter
		/// format is cached with the type of the parameter
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private static String GetParameterFormat(DbCommand command)
		{

			if (!mParametersFormat.ContainsKey(command.GetType()))
			{
				mParametersFormat.Add(
					command.GetType(),
					command.Connection.GetSchema("DataSourceInformation")
						.Rows[0]["ParameterMarkerFormat"].ToString());
			}
			return mParametersFormat[command.GetType()];
		}
		#endregion

		#region Execution core

		/// <summary>
		/// This is the core execution function, it accept a simple functor that will accept a sqlcommand
		/// the command is created in the core of the function so it really care of all the standard
		/// burden of creating connection, creating transaction and enlist command into a transaction.
		/// </summary>
		/// <param name="functionToExecute">The delegates that really executes the command.</param>
		public static void Execute(Action<DbCommand, DbProviderFactory> functionToExecute)
		{
			DbProviderFactory factory = GetFactory();
			using (GlobalTransactionManager.TransactionToken token = CreateConnection())
			{
				ConnectionData connectionData = (ConnectionData)token.GetFromTransactionContext(GetKeyFromConnName(String.Empty));
				try
				{
					using (DbCommand command = factory.CreateCommand())
					{
						command.CommandType = CommandType.Text;
						command.Connection = connectionData.Connection;
						command.Transaction = connectionData.Transaction;
						functionToExecute(command, factory);
					}
				}
				catch
				{
					token.Doom();
					throw;
				}
			}
		}

		internal static DbProviderFactory GetFactory()
		{
			return DbProviderFactories.GetFactory(ConfigurationRegistry.MainConnectionString.ProviderName);
		}

		#endregion

		#region helper function
		/// <summary>
		/// This function Execute a command, it accepts a function with no parameter that
		/// Prepare a command to be executed. It internally use the 
		/// function that really executes the code.
		/// </summary>
		/// <typeparam name="T">return parameter type, it reflect the return type
		/// of the delegates</typeparam>
		/// <param name="functionToExecute">The function that prepares the command that should
		/// be executed with execute scalar.</param>
		/// <returns></returns>
		public static T ExecuteScalar<T>(Action<DbCommand, DbProviderFactory> functionToExecute)
		{
			T result = default(T);
			Execute(delegate(DbCommand command, DbProviderFactory factory)
			{
				functionToExecute(command, factory);
				object o = command.ExecuteScalar();
				//result = (T)o; //execute scalar mi ritorna un decimal...che non posso castare
				result = (T)Convert.ChangeType(o, typeof(T));
			});
			return result;
		}

		public static List<T> ExecuteGetEntity<T>(Action<DbCommand, DbProviderFactory> functionToExecute, Func<IDataReader, T> select)
		{
			List<T> retvalue = new List<T>();
			Execute((c, f) =>
			{
				functionToExecute(c, f);
				using (IDataReader dr = c.ExecuteReader())
				{
					while (dr.Read())
					{
						retvalue.Add(select(dr));
					}
				}
			});
			return retvalue;
		}

		/// <summary>
		/// Execute a command with no result.
		/// </summary>
		/// <param name="functionToExecute"></param>
		public static Int32 ExecuteNonQuery(Action<DbCommand, DbProviderFactory> functionToExecute)
		{
			Int32 result = -1;
			Execute(delegate(DbCommand command, DbProviderFactory factory)
			{
				functionToExecute(command, factory);
				result = command.ExecuteNonQuery();
			});
			return result;
		}


		/// <summary>
		/// This is the function that permits to use a datareader without any risk
		/// to forget datareader open.
		/// </summary>
		/// <param name="commandPrepareFunction">The delegate should accepts 3 parameter, 
		/// the command to configure, a factory to create parameters, and finally another
		/// delegate of a function that returns the datareader.</param>
		public static void ExecuteReader(
			Action<DbCommand, DbProviderFactory, Func<IDataReader>> commandPrepareFunction)
		{

			Execute(delegate(DbCommand command, DbProviderFactory factory)
			{
				//The code to execute only assures that the eventually created datareader would be
				//closed in a finally block.
				IDataReader dr = null;
				try
				{
					commandPrepareFunction(command, factory,
											  delegate()
											  {
												  dr = command.ExecuteReader();
												  return dr;
											  });
				}
				finally
				{
					if (dr != null) dr.Dispose();
				}
			});
		}

		public static void FillDataset(
			DataTable table,
			Action<DbCommand, DbProviderFactory> commandPrepareFunction)
		{

			Execute(
				delegate(DbCommand command, DbProviderFactory factory)
				{
					commandPrepareFunction(command, factory);
					using (DbDataAdapter da = factory.CreateDataAdapter())
					{
						da.SelectCommand = command;
						da.Fill(table);
					}
				});
		}

		public static void ExecuteDataset<T>(
			String tableName,
			Action<DbCommand, DbProviderFactory, Func<T>> commandPrepareFunction)
			where T : DataSet, new()
		{

			Execute(delegate(DbCommand command, DbProviderFactory factory)
			{
				//The code to execute only assures that the eventually created datareader would be
				//closed in a finally block.
				using (T ds = new T())
				{
					commandPrepareFunction(command, factory,
											  delegate()
											  {
												  using (DbDataAdapter da = factory.CreateDataAdapter())
												  {
													  da.SelectCommand = command;
													  da.Fill(ds, tableName);
												  }
												  return ds;
											  });
				}

			});
		}

		/// <summary>
		/// This is the function that permits to use a datareader without any risk
		/// to forget datareader open.
		/// </summary>
		/// <param name="commandPrepareFunction"></param>
		public static void ExecuteDataset(
			Action<DbCommand, DbProviderFactory, Func<DataSet>> commandPrepareFunction)
		{

			Execute(delegate(DbCommand command, DbProviderFactory factory)
			{
				//The code to execute only assures that the eventually created datareader would be
				//closed in a finally block.
				using (DataSet ds = new DataSet())
				{
					commandPrepareFunction(command, factory,
											  delegate()
											  {
												  using (DbDataAdapter da = factory.CreateDataAdapter())
												  {
													  da.SelectCommand = command;
													  da.Fill(ds);
												  }
												  return ds;
											  });
				}

			});
		}

		#endregion

		#region Command filler and helpers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="factory"></param>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void AddParameterToCommand(
			DbCommand command,
			DbProviderFactory factory,
			System.Data.DbType type,
			String name,
			object value)
		{

			DbParameter param = factory.CreateParameter();
			param.DbType = type;
			param.ParameterName = GetParameterName(command, name);
			param.Value = value;
			command.Parameters.Add(param);
		}

		public static String GetParameterName(
			DbCommand command,
			String parameterName)
		{

			return String.Format(GetParameterFormat(command), parameterName);
		}

		#endregion

		#region FluentInterface

		public static SqlQuery CreateQuery(string s)
		{
			return new SqlQuery(s, CommandType.Text);
		}

		public static SqlQuery CreateStored(string s)
		{
			return new SqlQuery(s, CommandType.StoredProcedure);
		}

		/// <summary>
		/// Execute a sqlquery.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="executionCore"></param>
		public static void Execute(SqlQuery q, Action executionCore)
		{
			using (GlobalTransactionManager.TransactionToken token = CreateConnection(q.ConnectionStringName))
			{
				ConnectionData connectionData = 
					(ConnectionData) token.GetFromTransactionContext(
						GetKeyFromConnName(q.ConnectionStringName));
				try
				{
					using (q.Command)
					{
						q.Command.Connection = connectionData.Connection;
						q.Command.Transaction = connectionData.Transaction;
						q.Command.CommandText = q.query.ToString();
						executionCore();
					}
				}
				catch
				{
					token.Doom();
					throw;
				}
			}
		}

		#endregion
	}
}