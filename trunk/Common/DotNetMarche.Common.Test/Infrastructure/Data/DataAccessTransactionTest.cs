using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.TestHelpers.Data;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure.Data
{
	/// <summary>
	/// We need a separate class for the test.
	/// </summary>
	[TestFixture]
	public class DataAccessTransactionTest
	{

		#region Base test functions

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

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OverrideSettings.Dispose();
		}

		private void InitDatabase()
		{
			FileInfo dbFile = new FileInfo("DataAccessTransactionTest1.db");
			if (dbFile.Exists) dbFile.Delete();
			repo.ConnStrings.Add("main", new ConnectionStringSettings("main", "data source=" + dbFile.FullName, "System.Data.SQLite"));
			DataAccess.CreateQuery("CREATE TABLE FirstTable(field1 int, field2 varchar(50))").ExecuteNonQuery();

			dbFile = new FileInfo("DataAccessTransactionTest2.db");
			if (dbFile.Exists) dbFile.Delete();
			repo.ConnStrings.Add("secondary", new ConnectionStringSettings("secondary", "data source=" + dbFile.FullName, "System.Data.SQLite"));
			DataAccess.CreateQuery("CREATE TABLE SecondaryFirstTable(field1 int, field2 varchar(50))").OnDb("secondary").ExecuteNonQuery();
		}

		/// <summary>
		/// This test use a fresh fixture with simple initial cleanup.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			DataAccess.CreateQuery("DELETE FROM FirstTable").ExecuteNonQuery();
			DataAccess.CreateQuery("DELETE FROM SecondaryFirstTable").OnDb("secondary").ExecuteNonQuery();
		}

		#endregion

		/// <summary>
		/// Verify that if we have no transaction we committ everything
		/// </summary>
		[Test]
		public void NoTransactionCommitt()
		{
			Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
			Assert.That(count, Is.EqualTo(1));
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}

		[Test]
		public void BasicTransactionCommitt()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}

		[Test]
		public void BasicTransactionRollback()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
				GlobalTransactionManager.DoomCurrentTransaction();
			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}

		/// <summary>
		/// Test that both database are in the same transaction
		/// </summary>
		[Test]
		public void DistinctDbTransaction()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
				count = DataAccess.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.OnDb("secondary")
					.ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
				GlobalTransactionManager.DoomCurrentTransaction();
			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
			DbAssert.OnQuery("Select count(*) cnt from SecondaryFirstTable").OnDb("secondary").That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}

		/// <summary>
		/// Test that both database are in the same transaction and both are rollbacked if an exception occurs.
		/// </summary>
		[Test]
		public void DistinctDbTransactionExceptionRollback()
		{
			try
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
					count = DataAccess.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
						.OnDb("secondary")
						.ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
					throw new ApplicationException();
				}
			} catch
			{
			}

			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
			DbAssert.OnQuery("Select count(*) cnt from SecondaryFirstTable").OnDb("secondary").That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}

		/// <summary>
		/// Test that both database are in the same transaction
		/// </summary>
		[Test]
		public void DistinctDbTransactionAccessInTransaction()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')")
					.ExecuteNonQuery();
				DataAccess.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.OnDb("secondary")
					.ExecuteNonQuery();
				//VErify that in transaction all data is queryable
				DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
				DbAssert.OnQuery("Select count(*) cnt from SecondaryFirstTable").OnDb("secondary").That("cnt", Is.EqualTo(1)).ExecuteAssert();
				GlobalTransactionManager.DoomCurrentTransaction();
			}
		}

		/// <summary>
		/// Test that both database are in the same transaction
		/// </summary>
		[Test]
		public void DistinctDbTransactionCommitt()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test')").ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
				count = DataAccess.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.OnDb("secondary")
					.ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));

			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
			DbAssert.OnQuery("Select count(*) cnt from SecondaryFirstTable").OnDb("secondary").That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}
	}
}
