using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Caching;

namespace DotNetMarche.Infrastructure.Data
{
	public class SqlQuery
	{

		#region Cache

		/// <summary>
		/// The string is manipulated to create the parameters, the caller uses
		/// the {param} syntax to indicate a parameter, but after substituition the string
		/// become @param in sql server, :param in oracle etc etc.
		/// After the substitution the real string can be cached.
		/// </summary>
		private String cachedString;

		#endregion

		#region Properties and constructors

		internal String ConnectionStringName { get; set; }
		internal DbCommand Command { get; set; }
		internal DbProviderFactory Factory { get; set; }


		/// <summary>
		/// provide access to the real query, if we have a cached query then use 
		/// the cached one, if not return the dynamically build query.
		/// </summary>
		internal String Query
		{
			get {
				return cachedString ??
				       (String) GlobalCache.Insert(originalQuery, "DataAccess", query.ToString(), DateTime.Now.AddHours(1), null); }
		}
		private StringBuilder query;
		private String originalQuery;

		private Dictionary<String, OutputParameter> outputParameters;
		internal Dictionary<String, OutputParameter> OutputParameters
		{
			get { return outputParameters ?? (outputParameters = new Dictionary<String, OutputParameter>()); }
		}

		internal Int32 OutputParamCount
		{
			get { return outputParameters == null ? 0 : outputParameters.Count; }

		}

		/// <summary>
		/// Init all the data needed by the data access.
		/// </summary>
		/// <param name="cmdType"></param>
		/// <param name="queryText"></param>
		private void InitQuery(CommandType cmdType, string queryText)
		{
			cachedString = GlobalCache.Get<String>(queryText);
			if (cachedString == null)
			{
				query = new StringBuilder();
				query.Append(originalQuery = queryText);
			}

			Factory = DataAccess.GetFactory();
			Command = Factory.CreateCommand();
			Command.CommandType = cmdType;

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

		#region Parameter Management

		public SqlQuery SetStringParam(string parameterName, string value)
		{
			SetParam(parameterName, value, DbType.String);
			return this;
		}

		public SqlQuery SetInt64Param(string parameterName, Int64 value)
		{
			SetParam(parameterName, value, DbType.Int64);
			return this;
		}

		public SqlQuery SetInt32Param(string parameterName, Int32? value)
		{
			SetParam(parameterName, value, DbType.Int32);
			return this;
		}

		public SqlQuery SetInt16Param(string parameterName, Int16 value)
		{
			SetParam(parameterName, value, DbType.Int16);
			return this;
		}

		public SqlQuery SetInt8Param(string parameterName, Byte value)
		{
			SetParam(parameterName, value, DbType.Byte);
			return this;
		}

		public SqlQuery SetSingleParam(string parameterName, Single value)
		{
			SetParam(parameterName, value, DbType.Single);
			return this;
		}

		public SqlQuery SetBooleanParam(string parameterName, Boolean? value)
		{
			SetParam(parameterName, value, DbType.Boolean);
			return this;
		}

		public SqlQuery SetGuidParam(string parameterName, Guid value)
		{
			SetParam(parameterName, value, DbType.Guid);
			return this;
		}

		public SqlQuery SetBooleanParam(string parameterName, Boolean value)
		{
			SetParam(parameterName, value, DbType.Boolean);
			return this;
		}

		public SqlQuery SetDateTimeParam(string parameterName, DateTime value)
		{
			SetParam(parameterName, value, DbType.DateTime);
			return this;
		}

		public SqlQuery SetDateTimeParam(string parameterName, DateTime? value)
		{
			if (value != null) SetParam(parameterName, value, DbType.DateTime);
			return this;
		}

		public SqlQuery SetFloatParam(string parameterName, Single value)
		{
			SetParam(parameterName, value, DbType.Single);
			return this;
		}

		public void SetParam(string commandName, Object value, DbType type)
		{
			String paramName = DataAccess.GetParameterName(Command, commandName, ConnectionStringName);
			//if the cached string is null we do not have cache, so we need to update command text.
			if (cachedString == null)
			{
				if (Command.CommandType == CommandType.Text)
					query.Replace("{" + commandName + "}", paramName);
			}
			DbParameter param = Factory.CreateParameter();
			param.DbType = type;
			param.ParameterName = paramName;
			param.Value = value ?? DBNull.Value;
			Command.Parameters.Add(param);
		}

		public String SetOutParam(string commandName, DbType type)
		{
			String paramName = DataAccess.GetParameterName(Command, commandName);
			if (Command.CommandType == CommandType.Text)
				query.Replace("{" + commandName + "}", paramName);
			DbParameter param = Factory.CreateParameter();
			param.DbType = type;
			param.ParameterName = paramName;
			param.Direction = ParameterDirection.Output;
			Command.Parameters.Add(param);
			return paramName;
		}

		#endregion

		public SqlQuery SetTimeout(Int32 millisecondsTimeout)
		{
			this.Command.CommandTimeout = millisecondsTimeout;
			return this;
		}


		#region OutputParameter

		public SqlQuery SetInt32OutParam(string paramName)
		{
			String pname = SetOutParam(paramName, DbType.Int32);
			OutputParameters.Add(paramName, new OutputParameter(pname, typeof(Int32)));
			return this;
		}

		public SqlQuery SetInt64OutParam(string paramName)
		{
			String pname = SetOutParam(paramName, DbType.Int64);
			OutputParameters.Add(paramName, new OutputParameter(pname, typeof(Int64)));
			return this;
		}

		public T GetOutParam<T>(String paramName)
		{
			return (T)outputParameters[paramName].Value;
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
