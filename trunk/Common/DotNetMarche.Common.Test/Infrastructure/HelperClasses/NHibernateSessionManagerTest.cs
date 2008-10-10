using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete.Repository;
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

		#region Initialization and 4 phase test management

		private IDisposable OverrideContextCleanUp;
		private TestContext overrideContext;
		private MockRepository mockRepository;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			overrideContext = new TestContext();
			OverrideContextCleanUp = CurrentContext.Override(overrideContext);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OverrideContextCleanUp.Dispose();
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
			overrideContext.storage.Add(sessionkey, session);
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
			overrideContext.storage.Add(sessionkey1, session);
			String sessionkey2 = Invoker.InvokePrivate<String>(
					typeof(NHibernateSessionManager), "GetContextSessionKeyForConfigFileName", "files\\NhConfig2.cfg.xml");
			overrideContext.storage.Add(sessionkey2, session2);
			NHibernateSessionManager.CloseSessions();
		}
	}
}
