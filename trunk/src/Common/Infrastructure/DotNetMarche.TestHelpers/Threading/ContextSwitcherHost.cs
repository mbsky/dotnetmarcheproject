using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetMarche.Utils.Linq;

namespace DotNetMarche.TestHelpers.Threading
{
	public class ContextSwitcherHost
	{
		private readonly object syncRoot = new object();

		internal object SyncRoot
		{
			get
			{
				return syncRoot;
			}
		}

		internal ManualResetEvent[] Event;

		internal Int32 Count;

		public ContextSwitcher[] Switchers { get; set; }

		public ContextSwitcherHost(int count)
		{
			Count = count;
			Event = (new ManualResetEvent[Count]).Fill(() => new ManualResetEvent(false));
			Int32 I = 0;
			//Switchers = (new ContextSwitcher[Count]).Fill(() => new ContextSwitcher(this, I++));
		}

		/// <summary>
		/// Start the execution of the chain.
		/// </summary>
		internal void Start()
		{
			Event[0].Set();
		}

		private Int32 CurrentWaitIndex;

		/// <summary>
		/// Every time a thread calls switch it want to mark a new section that must be executed 
		/// syncronized
		/// </summary>
		/// <param name="contextIndex"></param>
		public void Switch(Int32 contextIndex)
		{
			Debug.WriteLine("[+" + contextIndex + "](" + CurrentWaitIndex + ")");
			Debug.Flush();
			//I've executed a block, signal the other block
			Signal(contextIndex);
			//now wait for the other block to have finisched
			Wait(contextIndex);
			Debug.WriteLine("[-" + contextIndex + "](" + CurrentWaitIndex + ")");
			Debug.Flush();
		}

		/// <summary>
		/// A thread should wait until the previous thread does not signal 
		/// monitor to proceed. Remember that if the previous thread is dead
		/// for an exception there is no need to stop.
		/// </summary>
		/// <param name="index"></param>
		public void Wait(int index)
		{
			Int32 prevIndex = index == 0 ? Count -1 : index - 1;
			while (!Event[index].WaitOne(200)) ;
			Event[index].Reset();
		}

		public void Signal(int index)
		{
			Int32 signalIndex = index == Count - 1 ? 0 : index + 1;
			Event[signalIndex].Set();
		}

		private List<Int32> deadContexts = new List<Int32>();

		/// <summary>
		/// A specific thread was dead, continue the execution considering that the 
		/// thread will always signal.
		/// </summary>
		/// <param name="index"></param>
		public void SignalException(Int32 index)
		{
			deadContexts.Add(index);
		}
	}
}
