using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Threading
{
	public class ContextSwitcher
	{

	/// <summary>
		/// The assertion failed in the thread
		/// </summary>
		internal AssertionException FailedAssertion { get; set; }

		//internal ContextSwitcherHost host;


		private Int32 index;

		public ContextSwitcher(Int32 index)
		{
			this.index = index;
			ThisEvent = new ManualResetEvent(false);
		}

		internal Boolean AssertionFail
		{
			get { return FailedAssertion != null; }
		}

		internal ContextSwitcher NextSwitcher { get; set; }

		private ManualResetEvent ThisEvent { get; set; }

		//internal ManualResetEvent NextEvent { get; set; }

		/// <summary>
		/// Serialize the thread, if a context do a Switch it means that 
		/// the execution should be serialized.
		/// </summary>
		public void Switch()
		{
			SignalNext();
			//now wait for the other block to have finisched
			Wait();
		}

		internal void Wait()
		{
			Debug.WriteLine("[+" + index + "]");
			ThisEvent.WaitOne(Timeout.Infinite);
			ThisEvent.Reset();
			Debug.WriteLine("[-" + index + "]");
		}

		internal void Signal()
		{
			if (isBroken)
				SignalNext();
			else
				ThisEvent.Set();
		}

		internal void SignalNext()
		{
			NextSwitcher.Signal();
		}

		private Boolean isBroken = false;

		/// <summary>
		/// The context is gone into exception.
		/// </summary>
		public void SignalException()
		{
			isBroken = true;
			Signal();
		}
	}
}
