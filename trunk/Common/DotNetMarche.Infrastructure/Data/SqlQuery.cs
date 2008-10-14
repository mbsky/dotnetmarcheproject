using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Data
{
	public class SqlQuery
	{
		#region Properties and constructors

		internal DbCommand Command { get; set; }
		internal DbProviderFactory Factory { get; set; }
		internal StringBuilder query = new StringBuilder();
		internal String ConnectionStringName { get; set; }

		/// <summary>
		/// Init all the data needed by the data access.
		/// </summary>
		/// <param name="cmdType"></param>
		/// <param name="queryText"></param>
		private void InitQuery(CommandType cmdType, string queryText)
		{
			Factory = DataAccess.GetFactory();
			Command = Factory.CreateCommand();
			Command.CommandType = cmdType;
			query.Append(queryText);
		}

		internal SqlQuery(string query, CommandType cmdType)
		{
			InitQuery(cmdType, query);
			ConnectionStringName = String.Empty;
		}

		internal SqlQuery(String connectionStringName)
		{
			ConnectionStringName = connectionStringName;
		}

		#endregion

		#region Execution

		public T ExecuteScalar<T>()
		{
			T result = default(T);
			DataAccess.Execute(this, () => result = (T)Command.ExecuteScalar());
			return result;
		}

		public Int32 ExecuteNonQuery()
		{
			Int32 result = 0;
			DataAccess.Execute(this, () => result = Command.ExecuteNonQuery());
			return result;
		}

		/// <summary>
		/// Execute reader is more complex, because we need to keep the datareader
		/// open until the caller use it and be sure to dipose at the end of the use.
		/// </summary>
		/// <param name="func"></param>
		public void ExecuteReader(Action<IDataReader> func)
		{
			DataAccess.Execute(this, () =>
												{
													using (IDataReader dr = Command.ExecuteReader())
														func(dr);
												});
		}

		/// <summary>
		/// Execute reader is more complex, because we need to keep the datareader
		/// open until the caller use it and be sure to dipose at the end of the use.
		/// </summary>
		/// <param name="func"></param>
		public void ExecuteReader(Action<IDataRecord> func)
		{
			DataAccess.Execute(this, () =>
												{
													using (IDataReader dr = Command.ExecuteReader())
													{
														while (dr.Read())
															func(dr);
													}
												});
		}

		public void FillDataTable(DataTable dt)
		{
			DataAccess.Execute(this, () =>
												{
													using (DbDataAdapter da = Factory.CreateDataAdapter())
													{
														da.SelectCommand = Command;
														da.Fill(dt);
													}
												});
		}

		#endregion

		#region Parameters management

		public SqlQuery SetStringParam(string commandName, string value)
		{
			SetParam(commandName, value, DbType.String);
			return this;
		}

		public SqlQuery SetInt32Param(string commandName, Int32 value)
		{
			SetParam(commandName, value, DbType.Int32);
			return this;
		}

		public SqlQuery SetDateTimeParam(string commandName, DateTime value)
		{
			SetParam(commandName, value, DbType.DateTime);
			return this;
		}

		public SqlQuery SetSingleParam(string commandName, Single value)
		{
			SetParam(commandName, value, DbType.Single);
			return this;
		}

		public SqlQuery SetDoubleParam(string commandName, Double value)
		{
			SetParam(commandName, value, DbType.Double);
			return this;
		}

		public void SetParam(string commandName, Object value, DbType type)
		{
			String paramName = DataAccess.GetParameterName(Command, commandName, ConnectionStringName);
			if (Command.CommandType == CommandType.Text)
				query.Replace("{" + commandName + "}", paramName);
			DbParameter param = Factory.CreateParameter();
			param.DbType = type;
			param.ParameterName = paramName;
			param.Value = value;
			Command.Parameters.Add(param);
		}

		#endregion

		#region Fluent Itnerface for creation

		public SqlQuery CreateQuery(string s)
		{
			InitQuery(CommandType.Text, s);
			return this;
		}

		#endregion
	}
}
