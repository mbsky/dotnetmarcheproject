using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DotNetMarche.Infrastructure.Data;
using NUnit.Framework;
using Constraint = NUnit.Framework.Constraints.Constraint;

namespace DotNetMarche.TestHelpers.Data
{
	public class DbAssert
	{
		#region Fields

		private SqlQuery query;

		private readonly Dictionary<String, Constraint> constraints = new Dictionary<String, Constraint>();

		internal DbAssert()
		{
		}

		internal DbAssert(String connectionStringName, String queryText)
		{
			query = DataAccess.OnDb(connectionStringName).CreateQuery(queryText);
		}

		#endregion

		public static DbAssert OnQuery(String query)
		{
			return new DbAssert() {query = DataAccess.CreateQuery(query)};
		}

		public DbAssert WithQuery(String queryString)
		{
			if (null != query)
				query.CreateQuery(queryString);
			else
				query = DataAccess.CreateQuery(queryString);
			return this;
		}


		public DbAssert SetInt32Param(String paramName, Int32 value)
		{
			query.SetInt32Param(paramName, value);
			return this;
		}

		public DbAssert That(String field, Constraint constraint)
		{
			constraints.Add(field, constraint);
			return this;
		}

		public void ExecuteAssert()
		{
			query.ExecuteReader((dr) =>
				{
					Assert.That(dr.Read(), "Query " + query + " did not return suitable data");
					foreach (KeyValuePair<String, Constraint> kvp in constraints)
					{
						Assert.That(dr[kvp.Key], kvp.Value, String.Format("field {0} reason:", kvp.Key));
					}
				});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static DbAssertOnDbStub OnDb(String connectionString)
		{
			return new DbAssertOnDbStub(connectionString);
		}
	}

	public class DbAssertOnDbStub
	{
		private String connectionString;

		internal DbAssertOnDbStub(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public DbAssert WithQuery(String query)
		{
			return new DbAssert(connectionString, query);
		}
	}
}