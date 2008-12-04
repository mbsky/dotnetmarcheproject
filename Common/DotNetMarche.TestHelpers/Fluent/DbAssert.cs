using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Nablasoft.Test.UnitTest
{
	public class DbAssert
	{

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

		private String query;
		private Dictionary<String, Constraint> constraints = new Dictionary<String, Constraint>();

		public void ExecuteAssert(Func<String, DbDataReader> funcReader)
		{

			using (DbDataReader dr = funcReader(query))
			{
				NUnit.Framework.Assert.That(dr.Read(), "Query " + query + " does not return data");
				foreach (KeyValuePair<String, Constraint> kvp in constraints)
				{
					NUnit.Framework.Assert.That(dr[kvp.Key], kvp.Value, String.Format("field {0} reason:", kvp.Key));
				}
			}
		}

	}
}
