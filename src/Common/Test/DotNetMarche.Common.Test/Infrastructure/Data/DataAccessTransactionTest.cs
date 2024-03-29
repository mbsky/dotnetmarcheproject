﻿using System;
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
			DataAccess.OnDb("secondary").CreateQuery("CREATE TABLE SecondaryFirstTable(field1 int, field2 varchar(50))").ExecuteNonQuery();
		}

		/// <summary>
		/// This test use a fresh fixture with simple initial cleanup.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			DataAccess.CreateQuery("DELETE FROM FirstTable").ExecuteNonQuery();
			DataAccess.OnDb("secondary").CreateQuery("DELETE FROM SecondaryFirstTable").ExecuteNonQuery();
		}

		#endregion

		#region Basic Transaction Tests

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
				count = DataAccess.OnDb("secondary")
					.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));
				GlobalTransactionManager.DoomCurrentTransaction();
			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
			DbAssert.OnDb("secondary").WithQuery("Select count(*) cnt from SecondaryFirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
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
					count = DataAccess.OnDb("secondary")
						.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
						.ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
					throw new ApplicationException();
				}
			} catch
			{
			}

			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
			DbAssert.OnDb("secondary").WithQuery("Select count(*) cnt from SecondaryFirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
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
				DataAccess.OnDb("secondary")
					.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.ExecuteNonQuery();
				//VErify that in transaction all data is queryable
				DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
				DbAssert.OnDb("secondary").WithQuery("Select count(*) cnt from SecondaryFirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
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
				count = DataAccess.OnDb("secondary")
					.CreateQuery("Insert into SecondaryFirstTable (field1, field2) values (1, 'test')")
					.ExecuteNonQuery();
				Assert.That(count, Is.EqualTo(1));

			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
			DbAssert.OnDb("secondary").WithQuery("Select count(*) cnt from SecondaryFirstTable").That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}

		#endregion

		#region Multiple Transaction Tests

		[Test]
		public void MultipleTransactionBothRollback()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test1')").ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
				}
				using (GlobalTransactionManager.BeginTransaction())
				{
					Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (2, 'test2')").ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
				}
				GlobalTransactionManager.DoomCurrentTransaction();
			}
			DbAssert.OnQuery("Select count(*) cnt from FirstTable").That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}		
        
        [Test]
		public void MultipleTransactionSeeDataCommittedInNested()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					Int32 count = DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test1')").ExecuteNonQuery();
					Assert.That(count, Is.EqualTo(1));
				}
                //First transaction is committed I should see data into db
                GlobalTransactionManager.DoomCurrentTransaction();
                DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                    .That("cnt", Is.EqualTo(1)).ExecuteAssert();
			}
            //Transaction is doomed, so the row disappeared
            DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                    .That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}       
        
        [Test]
		public void MultipleTransactionRollbackAndCommit()
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test1')").ExecuteNonQuery();
					GlobalTransactionManager.DoomCurrentTransaction();
				}
                DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test2')").ExecuteNonQuery();
			}
            //inner transaction is rollbacked, but the external one no, so I should see second row
            DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                .That("cnt", Is.EqualTo(1)).ExecuteAssert();
            DbAssert.OnQuery("Select field2 from FirstTable")
                .That("field2", Is.EqualTo("test2")).ExecuteAssert();
		}

        [Test]
        public void MultipleTransactionFirstRollbackThenCommitt()
        {
            using (GlobalTransactionManager.BeginTransaction())
            {
                using (GlobalTransactionManager.BeginTransaction())
                {
                    DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (1, 'test1')").ExecuteNonQuery();
                    GlobalTransactionManager.DoomCurrentTransaction();
                }
                using (GlobalTransactionManager.BeginTransaction())
                {
                    DataAccess.CreateQuery("Insert into FirstTable (field1, field2) values (2, 'test2')").ExecuteNonQuery();
                    DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                        .That("cnt", Is.EqualTo(1)).ExecuteAssert();
                }
                DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                        .That("cnt", Is.EqualTo(1)).ExecuteAssert();
            }
            DbAssert.OnQuery("Select count(*) cnt from FirstTable")
                        .That("cnt", Is.EqualTo(1)).ExecuteAssert();
        }	
		#endregion
	}
}
