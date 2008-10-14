using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Concrete.Repository;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.TestHelpers.Data;
using DotNetMarche.TestHelpers.Reflection;
using NHibernate;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace DotNetMarche.Common.Test.Infrastructure.HelperClasses
{
	[TestFixture]
	public class NHibernateSessionManagerTest
	{
		private const string SessionDataTypeName = "DotNetMarche.Infrastructure.Concrete.Repository.NHibernateSessionManager+SessionData, DotNetMarche.Infrastructure.Concrete";

		#region Initialization and 4 phase test management

		private InMemoryConfigurationRegistry repo;
		private IDisposable OverrideContextCleanUp;
		private TestContext overrideContext;
		private MockRepository mockRepository;
		private IDisposable OverrideSettings;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			overrideContext = new TestContext();
			OverrideContextCleanUp = CurrentContext.Override(overrideContext);
			repo = new InMemoryConfigurationRegistry();
			repo.ConnStrings.Add(
				"main", new ConnectionStringSettings(
					"main", "Data Source=DbFile1.db;Version=3", "System.Data.SQLite"));
			repo.ConnStrings.Add(
				"NhConfig1", new ConnectionStringSettings(
					"NhConfig1", "Data Source=:memory:;Version=3;New=True;", "System.Data.SQLite"));
			repo.ConnStrings.Add(
				"NhConfig2", new ConnectionStringSettings(
					"NhConfig2", "Data Source=:memory:;Version=3;New=True;", "System.Data.SQLite"));

			OverrideSettings = ConfigurationRegistry.Override(repo);

			NHibernateSessionManager.GenerateDbFor("files//NhConfigFile1.cfg.xml");
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OverrideContextCleanUp.Dispose();
			OverrideSettings.Dispose();
		}

		[SetUp]
		public void SetUp()
		{
			overrideContext.storage.Clear();
			mockRepository = new MockRepository();
		}

		[TearDown]
		public void TearDown()
		{
			mockRepository.VerifyAll();
		}

		#endregion

		#region Helpers

		private Int32 curId = 999;
		private Int32 GetNewId()
		{
			return curId++;
		}

		#endregion

		#region Basic Tests

		[Test]
		public void BasicGetSession()
		{
			ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			Assert.That(session, Is.Not.Null);
		}

		[Test]
		public void BasicGetSessionForDefaultConfiguration()
		{
			ISession session = NHibernateSessionManager.GetSession();
			Assert.That(session, Is.Not.Null);
		}

		[Test, ExpectedException]
		public void BasicGetSessionForWrongConfiguration()
		{
			ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigWrong.cfg.xml");
			Assert.That(session, Is.Not.Null);
		}

		[Test]
		public void SessionIsInTheContext()
		{
			ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			Assert.That(overrideContext.storage.Count, Is.EqualTo(1));
		}

		[Test]
		public void TwoSessionIsInTheContext()
		{
			NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			NHibernateSessionManager.GetSessionFor("files\\NhConfig2.cfg.xml");
			Assert.That(overrideContext.storage.Count, Is.EqualTo(2));
		}

		[Test]
		public void TwoConfigurationReturnDistinctSessions()
		{
			ISession session1 = NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			ISession session2 = NHibernateSessionManager.GetSessionFor("files\\NhConfig2.cfg.xml");
			Assert.That(!ReferenceEquals(session1, session2));
		}

		[Test]
		public void SameSessionReturnedForConsequentCalls()
		{
			ISession session1 = NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			ISession session2 = NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			Assert.That(Object.ReferenceEquals(session1, session2));
		}

		[Test]
		public void CloseSessionIsNoMoreInContext()
		{
			NHibernateSessionManager.GetSessionFor("files\\NhConfig1.cfg.xml");
			NHibernateSessionManager.CloseSessionFor("files\\NhConfig1.cfg.xml");
			Assert.That(overrideContext.storage.Count, Is.EqualTo(0));
		}

		/// <summary>
		/// Verify that the closeSessionFor correctly invoke flush and dispose of the session.
		/// </summary>
		[Test]
		public void SessionIsDisposedAndFlushedWhenClosed()
		{
			ISession session = mockRepository.CreateMock<ISession>();
			Expect.Call(session.Flush);
			Expect.Call(session.Dispose);
			mockRepository.ReplayAll();
			String sessionkey = Invoker.InvokePrivate<String>(
				typeof(NHibernateSessionManager), "GetContextSessionKeyForConfigFileName", "files\\NhConfig1.cfg.xml");
			Object obj = Invoker.CreatePrivateInstance(SessionDataTypeName);
			Invoker.SetField(obj, "Session", session);
			overrideContext.storage.Add(sessionkey, obj);
			NHibernateSessionManager.CloseSessionFor("files\\NhConfig1.cfg.xml");
		}

		/// <summary>
		/// Verify that the closeSessionFor correctly invoke flush and dispose of the session.
		/// </summary>
		[Test]
		public void SessionIsDisposedAndFlushedWhenClosedForAllSessions()
		{
			ISession session = mockRepository.CreateMock<ISession>();
			Expect.Call(session.Flush);
			Expect.Call(session.Dispose);
			ISession session2 = mockRepository.CreateMock<ISession>();
			Expect.Call(session2.Flush);
			Expect.Call(session2.Dispose);
			mockRepository.ReplayAll();
			//Grab the key that session use to indicize the session, and preload the override context.
			String sessionkey1 = Invoker.InvokePrivate<String>(
				typeof(NHibernateSessionManager), "GetContextSessionKeyForConfigFileName", "files\\NhConfig1.cfg.xml");
			Object obj = Invoker.CreatePrivateInstance(SessionDataTypeName);
			Invoker.SetField(obj, "Session", session);
			overrideContext.storage.Add(sessionkey1, obj);
			String sessionkey2 = Invoker.InvokePrivate<String>(
					typeof(NHibernateSessionManager), "GetContextSessionKeyForConfigFileName", "files\\NhConfig2.cfg.xml");
			obj = Invoker.CreatePrivateInstance(SessionDataTypeName);
			Invoker.SetField(obj, "Session", session2);
			overrideContext.storage.Add(sessionkey2, obj);
			NHibernateSessionManager.CloseSessions();
		}

		#endregion

		#region Test With Global Transaction Manager

		/// <summary>
		/// Verify that the factory return a session that is enlisted in the global transaction.
		/// </summary>
		[Test]
		public void TestEnlistInGlobalTransaction()
		{
			Int32 insertedId;
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
				{
					AnEntity e = AnEntity.CreateSome();
					insertedId = (Int32)session.Save(e);
				}
				GlobalTransactionManager.DoomCurrentTransaction();
			}
			DbAssert.OnQuery("select count(*) cnt from AnEntity where id = {id}")
				.SetInt32Param("id", insertedId)
				.That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}

		/// <summary>
		/// Verify that the factory return a session that is enlisted in the global transaction.
		/// </summary>
		[Test]
		public void TestEnlistInGlobalTransactionReadDataAccessWrittenData()
		{
			Int32 insertedId;
			using (GlobalTransactionManager.BeginTransaction())
			{
				Int32 newId = GetNewId();
DataAccess.OnDb("main")
	.CreateQuery("insert into AnEntity (id, name, value) values ({pid}, {pname}, {pvalue})")
	.SetInt32Param("pid", newId)
	.SetStringParam("pname", "xxx")
	.SetInt32Param("pvalue", 108)
	.ExecuteNonQuery();
				using (ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
				{
					AnEntity e = session.Load<AnEntity>(newId);
					Assert.That(e.Name, Is.EqualTo("xxx"));
					Assert.That(e.Value, Is.EqualTo(108));
				}
			}
		}

		/// <summary>
		/// Verify that the factory return a session that is enlisted in the global transaction.
		/// </summary>
		[Test]
		public void TestEnlistInGlobalTransactionRead()
		{
			Int32 insertedId;
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
				{
					AnEntity e = AnEntity.CreateSome();
					insertedId = (Int32)session.Save(e);
				}
				//We are still in the transaction, ensure we can read the db in the same transaction
				DbAssert.OnDb("main").WithQuery("select count(*) cnt from AnEntity where id = {id}")
					.SetInt32Param("id", insertedId)
					.That("cnt", Is.EqualTo(1)).ExecuteAssert();
			}
		}


		[Test]
		public void TestEnlistInGlobalTransactionReverseOrder()
		{
			Int32 insertedId;
			using (ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					AnEntity e = AnEntity.CreateSome();
					insertedId = (Int32)session.Save(e);
					session.Flush();
					GlobalTransactionManager.DoomCurrentTransaction();
				}
			}
			DbAssert.OnQuery("select count(*) cnt from AnEntity where id = {id}")
				.SetInt32Param("id", insertedId)
				.That("cnt", Is.EqualTo(0)).ExecuteAssert();
		}

		[Test]
		public void TestSessionCanWorkWhenTransactionIsClosed()
		{
			Int32 insertedId;
			using (ISession session = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					AnEntity e = AnEntity.CreateSome();
					session.Save(e);
					session.Flush();
				}
				//Now the transaction is closed, session still need to work correctly
				AnEntity e1 = AnEntity.CreateSome();
				insertedId = (Int32)session.Save(e1);
				session.Flush();
			}
			//Need to see the second entity
			DbAssert.OnQuery("select count(*) cnt from AnEntity where id = {id}")
				.SetInt32Param("id", insertedId)
				.That("cnt", Is.EqualTo(1)).ExecuteAssert();
		}

		#endregion

		#region Automatic Disposed Session Management.

		/// <summary>
		/// If we ask a session to the sessionmanager, then we dispose the session outside of the 
		/// sessino manager, when we ask again for the session we should have a new session because
		/// the older is disposed
		/// </summary>
		[Test]
		public void TestOuterDisposeOfTheSession()
		{
			ISession session1;
			using (session1 = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml"))
			{
			}
			ISession session2 = NHibernateSessionManager.GetSessionFor("files\\NhConfigFile1.cfg.xml");
			Assert.That(session1, Is.Not.EqualTo(session2), "Same session returned even if we disposed outside the SessionManager");
		}

		#endregion
	}
}
