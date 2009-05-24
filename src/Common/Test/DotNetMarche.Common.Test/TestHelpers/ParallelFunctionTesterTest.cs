using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using DotNetMarche.TestHelpers.Threading;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.TestHelpers
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture, Category("MultiThread")]
	public class ParallelFunctionTesterTest
	{
		private ParallelFunctionTester sut;

		[SetUp]
		public void SetUp()
		{
			sut = new ParallelFunctionTester();
		}

		#region Functions

		private Int32 retStatus;

		private void FunctionDoNothing(Object state, ContextSwitcher switcher)
		{
			//donothing
		}

		private void FunctionWaitAndSet(Object state, ContextSwitcher switcher)
		{
			Thread.Sleep(200);
			retStatus++;
		}

		private void FunctionAssertFalse(Object state, ContextSwitcher switcher)
		{
			Assert.Fail("This is a failure");
		}

		 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		/// <param name="switcher"></param>
		private void FunctionForThread(Object state, ContextSwitcher switcher)
		{
			Int32 value = retStatus;
			++retStatus; // add 1
			switcher.Switch(); // I ask for switch, the next function should be executed so another 1 is added
			Assert.That(retStatus, Is.EqualTo(value + 2));
			++retStatus;
		}

		private StringBuilder trace = new StringBuilder();

		private void FunctionDumpSimple(Object state, ContextSwitcher switcher)
		{
			trace.Append((String) state);
		}

		private void FunctionDumpDouble(Object state, ContextSwitcher switcher)
		{
			trace.Append((String)state);
			switcher.Switch();
			trace.Append((String)state);
		}

		private void FunctionDumpWithFailureAssertion(Object state, ContextSwitcher switcher)
		{
			if (((String)state) == "C") Assert.Fail();
			trace.Append((String)state);
		}

		private void FunctionDumpThatThrowsException(Object state, ContextSwitcher switcher)
		{
			if (((String)state) == "C") throw new System.SystemException();
			trace.Append((String)state);
		}

		#endregion

		/// <summary>
		/// verify that nothing hangs.
		/// </summary>
		[Test]
		public void BasicTest()
		{
			sut.CallMultiple(FunctionDoNothing, 1, new Object[] {null});
			sut.AssertAll();
		}			
		
		[Test]
		public void BasicTestWaiting()
		{
			Int32 start = retStatus;
			sut.CallMultiple(FunctionWaitAndSet, 1, new Object[] { null });
			Assert.That(retStatus, Is.EqualTo(start + 1)); //Delta assertion
		}

		[Test, Explicit("This fail, check the message to look at good formatting")]
		public void BasicTestWithFailingAssert()
		{
			Int32 start = retStatus;
			sut.CallMultiple(FunctionAssertFalse, 1, new Object[] { null });
			sut.AssertAll();
		}

		[Test]
		public void BasicTwoThread()
		{
			trace.Length = 0;
			sut.CallMultiple(FunctionDumpSimple, 6, new Object[] {"A", "B", "C", "D", "E", "F"});
			Assert.That(trace.ToString(), Is.EqualTo("ABCDEF"));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void BasicTwoThreadEx()
		{
			trace.Length = 0;
			sut.CallMultiple(FunctionDumpWithFailureAssertion, 6, new Object[] { "A", "B", "C", "D", "E", "F" });
			Assert.That(trace.ToString(), Is.EqualTo("ABCDEF"));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void BasicTwoThreadExSystemException()
		{
			trace.Length = 0;
			sut.CallMultiple(FunctionDumpThatThrowsException, 6, new Object[] { "A", "B", "C", "D", "E", "F" });
			Assert.That(trace.ToString(), Is.EqualTo("ABCDEF"));
		}

		[Test]
		public void BasicTwoThreadDouble()
		{
			trace.Length = 0;
			sut.CallMultiple(FunctionDumpDouble, 6, new Object[] { "A", "B", "C", "D", "E", "F" });
			Assert.That(trace.ToString(), Is.EqualTo("ABCDEFABCDEF"));
		}	

	}
}
