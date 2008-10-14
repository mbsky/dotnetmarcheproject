using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Helpers;
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
		/// This function can be invoked only inside a transaction, it ensure that the connection data
		/// was correctly created and transaction initialized.
		/// </summary>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		public static ConnectionData GetActualConnectionData(String connectionName)
		{
			Verify.That(GlobalTransactionManager.IsInTransaction,
							"Cannot call GetActualConnectionData if we are not into a transaction");
			return (ConnectionData)CreateConnection(connectionName).GetFromTransactionContext(GetKeyFromConnName(connectionName));
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
			ConnectionData newConnData = null;
			if (GlobalTransactionManager.IsInTransaction)
			{
				//We are in a transaction, check if at current connection stack level there is  a connection data.
				Object connData = GlobalTransactionManager.TransactionContext.Get(GetKeyFromConnName(connectionName));
				if (null != connData)
				{
					//We already created the connection for this database in this transaction.
					return GlobalTransactionManager.Enlist(DoNothing);
				}
				//There is not a transaction in the current transaction stack level, are we in nested transaction?
				if (GlobalTransactionManager.TransactionsCount > 1)
				{
					//The only connection data valid is the one at the first level, lets check if it is present.
					connData = GlobalTransactionManager.TransactionContext.Get(GetKeyFromConnName(connectionName), 0);
					if (null == connData)
					{
						//We never created the connection data
						newConnData = new ConnectionData(connectionName);
						GlobalTransactionManager.TransactionContext.Set(GetKeyFromConnName(connectionName), newConnData, 0);
						GlobalTransactionManager.Enlist(newConnData.CloseConnection, 0);
					}
					else
					{
						newConnData = (ConnectionData)connData;
					}

					GlobalTransactionManager.TransactionToken lasttoken = null;
					//Now we have the connection data, we need to store this connection data in each connection that is active and
					//that still not have start a transaction
					for (Int32 Ix = 1; Ix < GlobalTransactionManager.TransactionsCount; ++Ix)
					{
						if (GlobalTransactionManager.TransactionContext.Get(GetKeyFromConnName(connectionName), Ix) == null)
						{
							//In this step of the stack there is no ConnectionData, store and increase the transaction
							newConnData.BeginNestedTransaction();
							lasttoken = GlobalTransactionManager.Enlist(newConnData.CloseConnection, Ix);
							lasttoken.SetInTransactionContext(GetKeyFromConnName(connectionName), newConnData);
						}
					}
					//Return the last token, the one corresponding to the current transaction level.
					return lasttoken;
				}
			}

			//We are not in nested transaction and there is not connection data, create for the first time
			newConnData = new ConnectionData(connectionName);
			GlobalTransactionManager.TransactionToken token =
				GlobalTransactionManager.Enlist(newConnData.CloseConnection);
			token.SetInTransactionContext(GetKeyFromConnName(connectionName), newConnData);
			return token;
		}

		private static void DoNothing(Boolean b) { }

		#endregion

		#region Inner classes

		/// <summary>
		/// Keep all the objects needed to access the database in a single
		/// class. 
		/// </summary>
		public class ConnectionData
		{
			public readonly DbConnection Connection;
			public readonly Stack<DbTransaction> TransactionStack = new Stack<DbTransaction>();
			public readonly DbProviderFactory Factory;

			public DbTransaction CurrentTransaction
			{
				get { return TransactionStack.Peek(); }
			}

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
				TransactionStack.Push(Connection.BeginTransaction());
			}

			/// <summary>
			/// Used for nested transaction, when we need to estabilish a nested transaction
			/// we need to start another transaction on the same connection so we does
			/// not really need to recreate every object
			/// </summary>
			public void BeginNestedTransaction()
			{
				TransactionStack.Push(Connection.BeginTransaction());
			}

			/// <summary>
			/// This connection data should be closed, the transactino is to be committed
			/// or rollbacked and then disposed, but the connectino should be disposed
			/// only if we are not in a nested transaction.
			/// </summary>
			/// <param name="shouldCommit"></param>
			public void CloseConnection(Boolean shouldCommit)
			{
				DbTransaction innerTransaction = TransactionStack.Pop();
				if (shouldCommit)
					innerTransaction.Commit();
				innerTransaction.Dispose();
				if (TransactionStack.Count == 0)
				{
					Connection.Dispose();
				}
			}
		}

		public struct ConnectionObjects
		{
			private DbConnection connection;

		}

		#endregion

		#region Static Initialization

		/// <summary>
		/// Some providers does not implements correctly the ParameterMarkerFormat
		/// technique to find the syntax of the parameters
		/// </summary>
		private static Dictionary<String, String> wrongProviders;

		static DataAccess()
		{
			mParametersFormat = new Dictionary<String, String>();
			wrongProviders = new Dictionary<String, String>();
			wrongProviders.Add("System.Data.SqlClient.SqlCommand", "@{0}");
			wrongProviders.Add("System.Data.SQLite.SQLiteCommand", ":{0}");
		}

		#endregion

		#region Handling of connection

		/// <summary>
		/// To know at runtime the format of the parameter we need to check the 
		/// <see cref="System.Data.Common.DbConnection.GetSchema(String)"/> method. 
		/// To cache the format we use a dictionary with command type as a key and
		/// string format as value.
		/// </summary>
		private readonly static Dictionary<String, String> mParametersFormat;

		/// <summary>
		/// Gets the format of the parameter, to avoid query the schema the parameter
		/// format is cached with the type of the parameter
		/// </summary>
		/// <param name="command"></param>
		/// <param name="connectionStringName">Connection String name is needed because if 
		/// we did not already get the format of the parameter we need the connection to 
		/// retrieve the format from the schema.</param>
		/// <returns></returns>
		private static String GetParameterFormat(DbCommand command, String connectionStringName)
		{
			if (!mParametersFormat.ContainsKey(connectionStringName))
			{
				String typeName = command.GetType().FullName;
				if (wrongProviders.ContainsKey(typeName))
				{
					mParametersFormat.Add(connectionStringName, wrongProviders[typeName]);
				}
				else
				{
					ConnectionStringSettings cn;
					if (String.IsNullOrEmpty(connectionStringName))
						cn = ConfigurationRegistry.MainConnectionString;
					else
						cn = ConfigurationRegistry.ConnectionString(connectionStringName);
					DbProviderFactory Factory = DbProviderFactories.GetFactory(cn.ProviderName);
					using (DbConnection conn = Factory.CreateConnection())
					{
						conn.ConnectionString = cn.ConnectionString;
						conn.Open();
						mParametersFormat.Add(
											typeName,
											conn.GetSchema("DataSourceInformation")
												.Rows[0]["ParameterMarkerFormat"].ToString());
					}
				}
			}
			return mParametersFormat[connectionStringName];
		}

		/// <summary>
		/// Gets the format of the parameter, to avoid query the schema the parameter
		/// format is cached with the type of the parameter. This version is used 
		/// for the old interface of DataAccess based on anonimous function.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private static String GetParameterFormat(DbCommand command)
		{
			String typeName = command.GetType().FullName;
			if (!mParametersFormat.ContainsKey(typeName))
			{
				if (wrongProviders.ContainsKey(typeName))
				{
					mParametersFormat.Add(typeName, wrongProviders[typeName]);
				} else
				{
					mParametersFormat.Add(typeName,
					command.Connection.GetSchema("DataSourceInformation")
						.Rows[0]["ParameterMarkerFormat"].ToString());
				}
			}
			return mParametersFormat[typeName];
		}

		#endregion

		#region Execution core

		/// <summary>
		/// This is the core execution function, it accept a simple functor that will accept a sqlcommand
		/// the command is created in the core of the function so it really care of all the standard
		/// burden of creating connection, creating transaction and enlist command into a transaction.
		/// </summary>
		/// <param name="functionToExecute">The delegates that really executes the command.</param>
		internal static void Execute(Action<DbCommand, DbProviderFactory> functionToExecute)
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
						command.Transaction = connectionData.CurrentTransaction;
						functionToExecute(command, factory);
					}
				}
				catch
				{
					//There is an exception, I doom the transaction.
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

		///This is the older interface not used anymore.
		#region helper function
		///// <summary>
		///// This function Execute a command, it accepts a function with no parameter that
		///// Prepare a command to be executed. It internally use the 
		///// function that really executes the code.
		///// </summary>
		///// <typeparam name="T">return parameter type, it reflect the return type
		///// of the delegates</typeparam>
		///// <param name="functionToExecute">The function that prepares the command that should
		///// be executed with execute scalar.</param>
		///// <returns></returns>
		//public static T ExecuteScalar<T>(Action<DbCommand, DbProviderFactory> functionToExecute)
		//{
		//   T result = default(T);
		//   Execute(delegate(DbCommand command, DbProviderFactory factory)
		//   {
		//      functionToExecute(command, factory);
		//      object o = command.ExecuteScalar();
		//      //result = (T)o; //execute scalar mi ritorna un decimal...che non posso castare
		//      result = (T)Convert.ChangeType(o, typeof(T));
		//   });
		//   return result;
		//}

		//public static List<T> ExecuteGetEntity<T>(Action<DbCommand, DbProviderFactory> functionToExecute, Func<IDataReader, T> select)
		//{
		//   List<T> retvalue = new List<T>();
		//   Execute((c, f) =>
		//   {
		//      functionToExecute(c, f);
		//      using (IDataReader dr = c.ExecuteReader())
		//      {
		//         while (dr.Read())
		//         {
		//            retvalue.Add(select(dr));
		//         }
		//      }
		//   });
		//   return retvalue;
		//}

		///// <summary>
		///// Execute a command with no result.
		///// </summary>
		///// <param name="functionToExecute"></param>
		//public static Int32 ExecuteNonQuery(Action<DbCommand, DbProviderFactory> functionToExecute)
		//{
		//   Int32 result = -1;
		//   Execute(delegate(DbCommand command, DbProviderFactory factory)
		//   {
		//      functionToExecute(command, factory);
		//      result = command.ExecuteNonQuery();
		//   });
		//   return result;
		//}


		///// <summary>
		///// This is the function that permits to use a datareader without any risk
		///// to forget datareader open.
		///// </summary>
		///// <param name="commandPrepareFunction">The delegate should accepts 3 parameter, 
		///// the command to configure, a factory to create parameters, and finally another
		///// delegate of a function that returns the datareader.</param>
		//public static void ExecuteReader(
		//   Action<DbCommand, DbProviderFactory, Func<IDataReader>> commandPrepareFunction)
		//{

		//   Execute(delegate(DbCommand command, DbProviderFactory factory)
		//   {
		//      //The code to execute only assures that the eventually created datareader would be
		//      //closed in a finally block.
		//      IDataReader dr = null;
		//      try
		//      {
		//         commandPrepareFunction(command, factory,
		//                             delegate()
		//                             {
		//                                dr = command.ExecuteReader();
		//                                return dr;
		//                             });
		//      }
		//      finally
		//      {
		//         if (dr != null) dr.Dispose();
		//      }
		//   });
		//}

		//public static void FillDataset(
		//   DataTable table,
		//   Action<DbCommand, DbProviderFactory> commandPrepareFunction)
		//{

		//   Execute(
		//      delegate(DbCommand command, DbProviderFactory factory)
		//      {
		//         commandPrepareFunction(command, factory);
		//         using (DbDataAdapter da = factory.CreateDataAdapter())
		//         {
		//            da.SelectCommand = command;
		//            da.Fill(table);
		//         }
		//      });
		//}

		//public static void ExecuteDataset<T>(
		//   String tableName,
		//   Action<DbCommand, DbProviderFactory, Func<T>> commandPrepareFunction)
		//   where T : DataSet, new()
		//{

		//   Execute(delegate(DbCommand command, DbProviderFactory factory)
		//   {
		//      //The code to execute only assures that the eventually created datareader would be
		//      //closed in a finally block.
		//      using (T ds = new T())
		//      {
		//         commandPrepareFunction(command, factory,
		//                             delegate()
		//                             {
		//                                using (DbDataAdapter da = factory.CreateDataAdapter())
		//                                {
		//                                   da.SelectCommand = command;
		//                                   da.Fill(ds, tableName);
		//                                }
		//                                return ds;
		//                             });
		//      }

		//   });
		//}

		///// <summary>
		///// This is the function that permits to use a datareader without any risk
		///// to forget datareader open.
		///// </summary>
		///// <param name="commandPrepareFunction"></param>
		//public static void ExecuteDataset(
		//   Action<DbCommand, DbProviderFactory, Func<DataSet>> commandPrepareFunction)
		//{

		//   Execute(delegate(DbCommand command, DbProviderFactory factory)
		//   {
		//      //The code to execute only assures that the eventually created datareader would be
		//      //closed in a finally block.
		//      using (DataSet ds = new DataSet())
		//      {
		//         commandPrepareFunction(command, factory,
		//                             delegate()
		//                             {
		//                                using (DbDataAdapter da = factory.CreateDataAdapter())
		//                                {
		//                                   da.SelectCommand = command;
		//                                   da.Fill(ds);
		//                                }
		//                                return ds;
		//                             });
		//      }

		//   });
		//}

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
			String parameterName,
			String connectionStringName)
		{

			return String.Format(GetParameterFormat(command, connectionStringName), parameterName);
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

		public static SqlQuery OnDb(String connectionStringName)
		{
			return new SqlQuery(connectionStringName);
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
					(ConnectionData)token.GetFromTransactionContext(
						GetKeyFromConnName(q.ConnectionStringName));
				try
				{
					using (q.Command)
					{
						q.Command.Connection = connectionData.Connection;
						q.Command.Transaction = connectionData.CurrentTransaction;
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
