using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

		internal SqlQuery(string query, CommandType cmdType)
		{
			Factory = DataAccess.GetFactory();
			Command = Factory.CreateCommand();
			Command.CommandType = cmdType;
			this.query.Append(query);
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
			String paramName = DataAccess.GetParameterName(Command, commandName);
			if (Command.CommandType == CommandType.Text)
				query.Replace("{" + commandName + "}", paramName);
			DbParameter param = Factory.CreateParameter();
			param.DbType = type;
			param.ParameterName = paramName;
			param.Value = value;
			Command.Parameters.Add(param);
		}

		#endregion
	}
}
