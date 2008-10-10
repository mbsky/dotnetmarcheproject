using System;
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
	public class DataAccessTest : DbTest
	{
		#region Base test functions

		private IDisposable OverrideSettings;
		private InMemoryConfigurationRegistry repo;

		private void InitDatabase()
		{
			FileInfo dbFile = new FileInfo("maindbForDataAccess.db");
			if (dbFile.Exists) dbFile.Delete();
			repo.ConnStrings.Add("main",
			                     new ConnectionStringSettings("main", "data source=" + dbFile.FullName, "System.Data.SQLite"));
			DataAccess.CreateQuery("CREATE TABLE TESTTABLE(field1 int, field2 varchar(50))").ExecuteNonQuery();
			repo.ConnStrings.Add("preload1", new ConnectionStringSettings("preload1", "data source=" + 
			                                                                          Path.GetFullPath(@"Infrastructure\Data\Preload\preload1.db"), "System.Data.SQLite"));
		}

		#endregion

		#region Basic Test

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
		 
		[Test]
		public void ChangeAnotherConnectionString()
		{
			Int64 count = DataAccess.OnDb("preload1").CreateQuery("select count(*) from Table1").ExecuteScalar<Int64>();
			Assert.That(count, Is.EqualTo(2));
		}		

		[Test]
		public void TestFillDataSet()
		{

		}

		#endregion
	}
}
