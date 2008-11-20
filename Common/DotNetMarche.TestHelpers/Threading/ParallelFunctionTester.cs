using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetMarche.Utils.Linq;
using NUnit.Framework;

namespace DotNetMarche.TestHelpers.Threading
{
	/// <summary>
	/// used to easy the test of multithreaded objects.
	/// </summary>
	public class ParallelFunctionTester
	{
		private struct ExecState
		{
			public Action<Object, ContextSwitcher> Action;
			public ContextSwitcher Switcher;
			public Object State;
			public Thread Thread;

			public ExecState(Action<object, ContextSwitcher> action, ContextSwitcher switcher, object state, Thread thread)
			{
				Action = action;
				Switcher = switcher;
				State = state;
				this.Thread = thread;
			}
		}

		private Int32 ExecutingCount;

		public void CallMultiple(Action<Object, ContextSwitcher> act, Int32 count, Object[] state)
		{
			ExecutingCount = count;
			ContextSwitcherHost host = new ContextSwitcherHost(count);
			ContextSwitcher[] switchers = new ContextSwitcher[count];
			for (Int32 I = count - 1; I >= 0; --I)
			{
				switchers[I] = new ContextSwitcher(I);
				if (I < count - 1)
				{
					switchers[I].NextSwitcher = switchers[I + 1];
				}
			}
			switchers[count - 1].NextSwitcher = switchers[0];
			for (Int32 I = 0; I < count; ++I)
			{
				Thread t = new Thread(CallerFunction);
				t.Name = "ParallelFunctionTester [" + I + "]";
				t.Start(new ExecState(act, switchers[I], state[I], t));
			}
			switchers[0].Signal();
			while (ExecutingCount > 0) Thread.Sleep(300);

			//Now raise the exception if needed
			StringBuilder sb = new StringBuilder();
			StringBuilder detail = new StringBuilder();
			for (Int32 I = 0; I < count; ++I)
			{
				if (switchers[I].AssertionFail)
				{
					sb.AppendFormat("Function number {0} has failed:\n", I);
					detail.AppendFormat("Message for function number {0}:{1}\n", I, switchers[I].FailedAssertion);
				}
			}
			if (sb.Length > 0) Assert.Fail(sb + "\n" + detail);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		private void CallerFunction(Object obj)
		{
			ExecState status = (ExecState)obj;
			try
			{
				status.Switcher.Wait();
				status.Action(status.State, status.Switcher);
				status.Switcher.SignalNext();
			}
			catch (NUnit.Framework.AssertionException ex)
			{
				status.Switcher.FailedAssertion = ex;
				//Ops exception, this means that no more signaling from this thread occurs.
				status.Switcher.SignalException();
			} catch (Exception ex)
			{
				status.Switcher.SignalException();
			}
			finally
			{
				Interlocked.Decrement(ref ExecutingCount);
			}
		}

		public void AssertAll()
		{

		}
	}
}
