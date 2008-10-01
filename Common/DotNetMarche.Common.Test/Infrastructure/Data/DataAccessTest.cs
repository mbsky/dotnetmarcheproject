﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.TestHelpers.BaseTests;
using DotNetMarche.TestHelpers.Data;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure.Data
{
	[TestFixture]
	public class DataAccessTest //: DbTest
	{
		private IDisposable OverrideSettings;
		private InMemoryConfigurationRegistry repo;

		/// <summary>
		/// I know that default testing re
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			repo = new InMemoryConfigurationRegistry();
			OverrideSettings = ConfigurationRegistry.Override(repo);
			InitDatabase();
		}

		private void InitDatabase()
		{
			FileInfo dbFile = new FileInfo("maindbForDataAccess.db");
			if (dbFile.Exists) dbFile.Delete();
			repo.ConnStrings.Add("main",
			                     new ConnectionStringSettings("main", "data source=" + dbFile.FullName, "System.Data.SQLite"));
			DataAccess.CreateQuery("CREATE TABLE TESTTABLE(field1 int, field2 varchar(50))").ExecuteNonQuery();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OverrideSettings.Dispose();
		}

      
		[Test]
		public void TestBasicInsertionQuery()
		{
			Int32 count =	DataAccess.CreateQuery("Insert into TESTTABLE (field1, field2) values (1, 'test')").ExecuteNonQuery();
			Assert.That(count, Is.EqualTo(1));
			DbAssert.OnQuery("Select count(*) cnt from testtable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}
	}
}
