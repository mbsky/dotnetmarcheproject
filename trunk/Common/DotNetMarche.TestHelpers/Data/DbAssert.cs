﻿using System;
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

		public void ExecuteAssert()
		{
			DataAccess.CreateQuery(query).ExecuteReader((dr) =>
				{
					Assert.That(dr.Read(), "Query " + query + " did not return suitable data");
					foreach (KeyValuePair<String, Constraint> kvp in constraints)
					{
						Assert.That(dr[kvp.Key], kvp.Value, String.Format("field {0} reason:", kvp.Key));
					}
				});
		}

	}
}