using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Base;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class GlobalTransactionTest
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
		public void TestTransactionInContext()
		{
			GlobalTransactionManager.BeginTransaction();
			Assert.That(overrideContext.storage.Count, Is.EqualTo(1)); 
		}

		[Test]
		public void TestTransactionIsInTransaction()
		{
			GlobalTransactionManager.BeginTransaction();
			Assert.That(GlobalTransactionManager.IsInTransaction);
		}

		[Test]
		public void TestGoodTransactionIsCommitted()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			mock(true); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.Enlist(mock);
			}
		}		
		
		[Test]
		public void TestDoomedTransactionIsRollbacked()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			mock(false); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.Enlist(mock);
				GlobalTransactionManager.DoomCurrentTransaction();
			}
		}
	}
}
