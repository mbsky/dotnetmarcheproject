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

		private String query;

		private Dictionary<String, Constraint> constraints = new Dictionary<String, Constraint>();

		private String connectionStringName;

		#endregion

		public static DbAssert OnQuery(String query)
		{
			return new DbAssert(query);
		}

		public DbAssert That(String field, Constraint constraint)
		{
			constraints.Add(field, constraint);
			return this;
		}

		private DbAssert(string query)
		{
			this.query = query;
		}

		public void ExecuteAssert()
		{
			DataAccess.CreateQuery(query).OnDb(connectionStringName).ExecuteReader((dr) =>
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
		public DbAssert OnDb(String connectionString)
		{
			connectionStringName = connectionString;
			return this;
		}
	}
}