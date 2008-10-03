using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Logging;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using RhinoIs = Rhino.Mocks.Constraints.Is;

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
		public void TransactionInContext()
		{
			GlobalTransactionManager.BeginTransaction();
			Assert.That(overrideContext.storage.Count, Is.EqualTo(1));
		}

		[Test]
		public void TransactionIsInTransaction()
		{
			GlobalTransactionManager.BeginTransaction();
			Assert.That(GlobalTransactionManager.IsInTransaction);
		}

		[Test]
		public void GoodTransactionIsCommitted()
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
		public void TwoGoodTransactionIsCommitted()
		{
			Action<Boolean> mock1 = mockRepository.CreateMock<Action<Boolean>>();
			mock1(true); //Sets the expectation
			Action<Boolean> mock2 = mockRepository.CreateMock<Action<Boolean>>();
			mock2(true); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.Enlist(mock1);
				GlobalTransactionManager.Enlist(mock2);
			}
		}

		/// <summary>
		/// We have the first committ action that throws an exception we must be
		/// sure that the second action is called.
		/// </summary>
		[Test]
		public void IgnoreExceptionWhenCommitt()
		{
			Action<Boolean> mock1 = mockRepository.CreateMock<Action<Boolean>>();
			mock1(true); //Sets the expectation
			LastCall.Throw(new ApplicationException());
			Action<Boolean> mock2 = mockRepository.CreateMock<Action<Boolean>>();
			mock2(true); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.Enlist(mock1);
				GlobalTransactionManager.Enlist(mock2);
			}
		}

		/// <summary>
		/// </summary>
		[Test]
		public void LogExceptionWhenCommitt()
		{
			Action<Boolean> mock1 = mockRepository.CreateMock<Action<Boolean>>();
			Exception ex = new ArgumentException();
			Expect.Call(() => mock1(true)).Throw(ex); //Sets the expectation
			ILogger mockLogger = mockRepository.CreateMock<ILogger>();
			Expect.Call(mockLogger.ActualLevel).Return(LogLevel.Info);
			Expect.Call(() => mockLogger.Log(LogLevel.Error, "", ex))
				.Constraints(RhinoIs.Equal(LogLevel.Error), RhinoIs.Anything(), RhinoIs.Equal(ex));
			mockRepository.ReplayAll();
			using (Logger.Override(mockLogger))
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					GlobalTransactionManager.Enlist(mock1);
				}
			}

		}

		[Test]
		public void DoomedTransactionIsRollbacked()
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

		[Test]
		public void ImplicitTransactionIsDoomedIfExceptionIsThrown()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			mock(false); //Sets the expectation
			mockRepository.ReplayAll();
			try
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					GlobalTransactionManager.Enlist(mock);
					throw new ApplicationException();
				}
			}
			catch (ApplicationException)
			{
				//Ok we catch the exception but the inner using is exited inside an application handler.
			}

		}


	}
}
