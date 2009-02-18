using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Core;

namespace DotNetMarche.NunitExtension.Addins
{
	class RandomizerTestFixture : TestSuite
	{
		public RandomizerTestFixture(Type fixtureType)
			: base(fixtureType)
		{
			this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod(fixtureType);
			this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod(fixtureType);
		}

		private static List<Int32> rndSequence;
		static RandomizerTestFixture()
		{
			Random rnd = new Random();
			rndSequence = Enumerable.Range(0, 100).OrderBy(n => rnd.Next()).ToList();
		}
		 

		protected override void DoOneTimeSetUp(TestResult suiteResult)
		{
			base.DoOneTimeSetUp(suiteResult);
			suiteResult.AssertCount = NUnitFramework.GetAssertCount();
		}

		protected override void DoOneTimeTearDown(TestResult suiteResult)
		{
			base.DoOneTimeTearDown(suiteResult);
			suiteResult.AssertCount += NUnitFramework.GetAssertCount();
		}

		private List<Test> randomList;
		
private void RunAllTests(
 TestSuiteResult suiteResult, EventListener listener, ITestFilter filter)
{
	Random rnd = new Random();
	foreach (Test test in ArrayList.Synchronized(Tests).Cast<Test>().OrderBy(ts => rnd.Next()))
			{
				if (filter.Pass(test))
				{
					RunState saveRunState = test.RunState;

					if (test.RunState == RunState.Runnable && this.RunState != RunState.Runnable && this.RunState != RunState.Explicit)
					{
						test.RunState = this.RunState;
						test.IgnoreReason = this.IgnoreReason;
					}

					TestResult result = test.Run(listener, filter);

					suiteResult.AddResult(result);

					if (saveRunState != test.RunState)
					{
						test.RunState = saveRunState;
						test.IgnoreReason = null;
					}
				}
			}
		}

		public override TestResult Run(EventListener listener)
		{
			return Run(listener, TestFilter.Empty);
		}

		public override TestResult Run(EventListener listener, ITestFilter filter)
		{
			using (new TestContext())
			{
				TestSuiteResult suiteResult = new TestSuiteResult(new TestInfo(this), TestName.Name);

				listener.SuiteStarted(this.TestName);
				long startTime = DateTime.Now.Ticks;

				switch (this.RunState)
				{
					case RunState.Runnable:
					case RunState.Explicit:
						suiteResult.RunState = RunState.Executed;
						DoOneTimeSetUp(suiteResult);
						if (suiteResult.IsFailure)
							MarkTestsFailed(Tests, suiteResult, listener, filter);
						else
						{
							try
							{
								RunAllTests(suiteResult, listener, filter);
							}
							finally
							{
								DoOneTimeTearDown(suiteResult);
							}
						}
						break;

					case RunState.Skipped:
						suiteResult.Skip(this.IgnoreReason);
						MarkTestsNotRun(Tests, RunState.Skipped, IgnoreReason, suiteResult, listener, filter);
						break;

					default:
					case RunState.Ignored:
					case RunState.NotRunnable:
						suiteResult.Ignore(this.IgnoreReason);
						MarkTestsNotRun(Tests, RunState.Ignored, IgnoreReason, suiteResult, listener, filter);
						break;
				}

				long stopTime = DateTime.Now.Ticks;
				double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
				suiteResult.Time = time;

				listener.SuiteFinished(suiteResult);
				return suiteResult;
			}
		}

		private void MarkTestsNotRun(
			IList tests, RunState runState, string ignoreReason, TestSuiteResult suiteResult, EventListener listener, ITestFilter filter)
		{
			foreach (Test test in ArrayList.Synchronized(tests))
			{
				if (filter.Pass(test))
					MarkTestNotRun(test, runState, ignoreReason, suiteResult, listener, filter);
			}
		}

		private void MarkTestNotRun(
			 Test test, RunState runState, string ignoreReason, TestSuiteResult suiteResult, EventListener listener, ITestFilter filter)
		{
			if (test is TestSuite)
			{
				listener.SuiteStarted(test.TestName);
				TestSuiteResult result = new TestSuiteResult(new TestInfo(test), test.TestName.FullName);
				result.NotRun(runState, ignoreReason, null);
				MarkTestsNotRun(test.Tests, runState, ignoreReason, suiteResult, listener, filter);
				suiteResult.AddResult(result);
				listener.SuiteFinished(result);
			}
			else
			{
				listener.TestStarted(test.TestName);
				TestCaseResult result = new TestCaseResult(new TestInfo(test));
				result.NotRun(runState, ignoreReason, null);
				suiteResult.AddResult(result);
				listener.TestFinished(result);
			}
		}

		private void MarkTestsFailed(
			 IList tests, TestSuiteResult suiteResult, EventListener listener, ITestFilter filter)
		{
			foreach (Test test in ArrayList.Synchronized(tests))
				if (filter.Pass(test))
					MarkTestFailed(test, suiteResult, listener, filter);
		}

		private void MarkTestFailed(
			 Test test, TestSuiteResult suiteResult, EventListener listener, ITestFilter filter)
		{
			if (test is TestSuite)
			{
				listener.SuiteStarted(test.TestName);
				TestSuiteResult result = new TestSuiteResult(new TestInfo(test), test.TestName.FullName);
				string msg = string.Format("Parent SetUp failed in {0}", this.FixtureType.Name);
				result.Failure(msg, null, FailureSite.Parent);
				MarkTestsFailed(test.Tests, suiteResult, listener, filter);
				suiteResult.AddResult(result);
				listener.SuiteFinished(result);
			}
			else
			{
				listener.TestStarted(test.TestName);
				TestCaseResult result = new TestCaseResult(new TestInfo(test));
				string msg = string.Format("TestFixtureSetUp failed in {0}", this.FixtureType.Name);
				result.Failure(msg, null, FailureSite.Parent);
				suiteResult.AddResult(result);
				listener.TestFinished(result);
			}
		}

		//public override System.Collections.IList Tests
		//{
		//   get
		//   {
		//      return randomList ?? (randomList = RandomizeList());
		//   }
		//}

		//private List<Test> RandomizeList()
		//{
		//   //List<Test> randomized = new List<Test>();
		//   //for (Int32 I = 0; I < base.Tests.Count; ++I)
		//   //{

		//   //}
		//   List<Test> baseList = base.Tests.Cast<Test>().ToList();
		//   List<Test> result = base.Tests.Cast<Test>().OrderBy(test => rndSequence[baseList.IndexOf(test) % rndSequence.Count]).ToList();
		//   return result;
		//}

		
	}
}
