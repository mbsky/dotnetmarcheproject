using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Logging;
using DotNetMarche.TestHelpers.BaseTests;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using RhinoIs = Rhino.Mocks.Constraints.Is;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class GlobalTransactionTest : BaseUtilityTest
	{
		#region Initialization and 4 phase test management

		private TestContext overrideContext;
		private MockRepository mockRepository;

		protected override void OnTestFixtureSetUp()
		{
			overrideContext = new TestContext();
			DisposeAtTheEndOfFixture(CurrentContext.Override(overrideContext));
			base.OnTestFixtureSetUp();
		}

		
		protected override void OnSetUp()
		{
			overrideContext.storage.Clear();
			mockRepository = new MockRepository();
			ExecuteAtTheEndOfTest(() => mockRepository.VerifyAll()); 
		}

		#endregion

		#region Helpers

		public void Nope(Boolean b) { }

		#endregion

		#region Tests

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
		public void EnlistWithoutATransaction()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			mock(true); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.Enlist(mock))
			{
				//Do something, the important thing is that the delegate is called because we have 
				//not a transaction active.
			}
		}

		[Test]
		public void EnlistNestedWithTwoTransaction()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			Expect.Call(() => mock(true)).Repeat.Twice(); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				using (GlobalTransactionManager.BeginTransaction())
				{
					GlobalTransactionManager.Enlist(mock, 0);
					GlobalTransactionManager.Enlist(mock, 1);
				}
			}
		}		
		
		/// <summary>
		/// Create two nested transaction, then enlist in the first and second, dispose the second and
		/// be sure that the mock is called one time
		/// </summary>
		[Test]
		public void EnlistNestedWithTwoTransactionDisposeOne()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			Expect.Call(() => mock(true)).Repeat.Once(); //Sets the expectation
			mockRepository.ReplayAll();
			IDisposable first = GlobalTransactionManager.BeginTransaction();
			IDisposable second = GlobalTransactionManager.BeginTransaction();
		
			GlobalTransactionManager.Enlist(mock, 0);
			GlobalTransactionManager.Enlist(mock, 1);
			first.Dispose();
		}		
		
		/// <summary>
		/// If we enlist in the transaction with index zero we should access the corresponding transaction
		/// </summary>
		[Test]
		public void ContextNestedWithTwoTransaction()
		{
			GlobalTransactionManager.BeginTransaction();
			GlobalTransactionManager.BeginTransaction();
		
			GlobalTransactionManager.TransactionToken token = GlobalTransactionManager.Enlist(Nope, 0);
			GlobalTransactionManager.TransactionContext.Set("key", "test", 0);
			Assert.That(token.GetFromTransactionContext("key"), Is.EqualTo("test"));
		}			
		
		/// <summary>
		/// If we enlist without specification, we are enlisting on the last transaction scope.
		/// </summary>
		[Test]
		public void ContextNestedWithTwoTransactionLast()
		{
			GlobalTransactionManager.BeginTransaction();
			GlobalTransactionManager.BeginTransaction();
		
			GlobalTransactionManager.TransactionToken token = GlobalTransactionManager.Enlist(Nope);
			GlobalTransactionManager.TransactionContext.Set("key", "test", 1);
			Assert.That(token.GetFromTransactionContext("key"), Is.EqualTo("test"));
		}		
		

		/// <summary>
		/// Create two nested transaction, then enlist in the first and second, dispose the second and
		/// be sure that the mock is called one time
		/// </summary>
		[Test]
		public void EnlistNestedWithTwoTransactionDisposeTwo()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			Expect.Call(() => mock(true)).Repeat.Once(); //Sets the expectation
			mockRepository.ReplayAll();
			IDisposable first = GlobalTransactionManager.BeginTransaction();
			IDisposable second = GlobalTransactionManager.BeginTransaction();
		
			GlobalTransactionManager.Enlist(mock, 0);
			GlobalTransactionManager.Enlist(mock, 1);
			second.Dispose();
		}

		[Test]
		public void EnlistWithoutATransactionRollback()
		{
			Action<Boolean> mock = mockRepository.CreateMock<Action<Boolean>>();
			mock(false); //Sets the expectation
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.TransactionToken token = GlobalTransactionManager.Enlist(mock))
			{
				token.Doom();
				//Do something, the important thing is that the delegate is called because we have 
				//not a transaction active.
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
			Expect.Call(mockLogger.ActualLevel).Repeat.Any().Return(LogLevel.Info);
			Expect.Call(() => mockLogger.LogError("", ex))
				.Constraints(RhinoIs.Anything(), RhinoIs.Equal(ex));
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

		/// <summary>
		/// into the transaction we are raising an exception, the exception is handled outside
		/// the transaction scope, so the transaction should be aborted.
		/// </summary>
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

		[Test]
		public void IfNoTransactionCountShouldBeZero()
		{
			Assert.That(GlobalTransactionManager.TransactionsCount, Is.EqualTo(0));
		}

		#endregion

		#region Event verification

		public interface ITranEvSink
		{
			void Handle(Object sender, EventArgs e);
		}

		/// <summary>
		/// Verify that the TransactionOpening is called, and verify also that
		/// the transactino count is zero, because a transaction should be still 
		/// not created.
		/// </summary>
		[Test]
		public void VerifyTransactionOpening()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, EventArgs.Empty))
				.IgnoreArguments()
				.Callback(new Func<Object, EventArgs, Boolean>( (sender, e) => GlobalTransactionManager.TransactionsCount == 0));
			GlobalTransactionManager.TransactionOpening += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionOpening -= mock.Handle);
			mockRepository.ReplayAll();
			GlobalTransactionManager.BeginTransaction();
		}

		/// <summary>
		/// Verify that the TransactionOpened is called, and verify also that
		/// the transactino count is 1, because a transaction should be created
		/// </summary>
		[Test]
		public void VerifyTransactionOpened()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, EventArgs.Empty))
				.IgnoreArguments()
							.Callback(new Func<Object, EventArgs, Boolean>((sender, e) => GlobalTransactionManager.TransactionsCount == 1));
			GlobalTransactionManager.TransactionOpened += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionOpened -= mock.Handle);
			mockRepository.ReplayAll();
			GlobalTransactionManager.BeginTransaction();
		}		
		
		/// <summary>
		/// Verify that the TransactionClosing is called, and verify also that
		/// the transactino count is 1, because a transaction is still active
		/// </summary>
		[Test]
		public void VerifyTransactionClosing()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, EventArgs.Empty)) 
				.IgnoreArguments()
							.Callback(new Func<Object, EventArgs, Boolean>((sender, e) => GlobalTransactionManager.TransactionsCount == 1));
			GlobalTransactionManager.TransactionClosing += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosing -= mock.Handle);
			mockRepository.ReplayAll();
			GlobalTransactionManager.BeginTransaction().Dispose();
		}

		/// <summary>
		/// Verify that the event is called with the right isdoomed
		/// </summary>
		[Test]
		public void VerifyTransactionClosingDoomedFalse()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, null))
				.Constraints(RhinoIs.Null(), RhinoIs.Matching<TransactionClosingEventArgs>(args => args.IsDoomed == false));
			GlobalTransactionManager.TransactionClosing += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosing -= mock.Handle);
			mockRepository.ReplayAll();
			GlobalTransactionManager.BeginTransaction().Dispose();
		}			
		
		/// <summary>
		/// Verify that the event is called with the right isdoomed
		/// </summary>
		[Test]
		public void VerifyTransactionClosingDoomedTrue()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, null))
				.Constraints(RhinoIs.Null(), RhinoIs.Matching<TransactionClosingEventArgs>(args => args.IsDoomed == true));
			GlobalTransactionManager.TransactionClosing += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosing -= mock.Handle);
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.DoomCurrentTransaction();
			}
		}		
		
		/// <summary>
		/// Verify that the event is called with the right isdoomed
		/// </summary>
		[Test]
		public void VerifyTransactionClosedDoomedFalse()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, null))
				.Constraints(RhinoIs.Null(), RhinoIs.Matching<TransactionClosedEventArgs>(args => args.IsDoomed == false));
			GlobalTransactionManager.TransactionClosed += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosed -= mock.Handle);
			mockRepository.ReplayAll();
			GlobalTransactionManager.BeginTransaction().Dispose();
		}			
		
		/// <summary>
		/// Verify that the event is called with the right isdoomed
		/// </summary>
		[Test]
		public void VerifyTransactionClosedDoomedTrue()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, null))
				.Constraints(RhinoIs.Null(), RhinoIs.Matching<TransactionClosedEventArgs>(args => args.IsDoomed == true));
			GlobalTransactionManager.TransactionClosed += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosed -= mock.Handle);
			mockRepository.ReplayAll();
			using (GlobalTransactionManager.BeginTransaction())
			{
				GlobalTransactionManager.DoomCurrentTransaction();
			}
		}	

		/// <summary>
		/// Verify that the TransactionClosing is called, and verify also that
		/// the transactino count is 1, because a transaction is still active
		/// </summary>
		[Test]
		public void VerifyTransactionClosed()
		{
			ITranEvSink mock = mockRepository.CreateMock<ITranEvSink>();
			Expect.Call(() => mock.Handle(null, EventArgs.Empty))
				.IgnoreArguments()
							.Callback(new Func<Object, EventArgs, Boolean>((sender, e) => GlobalTransactionManager.TransactionsCount == 0));
			GlobalTransactionManager.TransactionClosed  += mock.Handle;
			ExecuteAtTheEndOfTest(() => GlobalTransactionManager.TransactionClosed -= mock.Handle);
			mockRepository.ReplayAll();
				GlobalTransactionManager.BeginTransaction().Dispose();

		}

		#endregion
	}
}
