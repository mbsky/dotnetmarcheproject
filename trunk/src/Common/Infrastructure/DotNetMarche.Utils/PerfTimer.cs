using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetMarche.Utils
{
	/// <summary>
	/// Simple class to verify timing 
	/// </summary>
	public class PerfTimer
	{
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		private static readonly double frequency = GetFrequency();

		private static double GetFrequency()
		{
			Int64 tempFreq;
			QueryPerformanceFrequency(out tempFreq);
			return tempFreq;
		}

		private Int64 initialTicks;

		public void Start()
		{
			QueryPerformanceCounter(out initialTicks);
		}

		public Double Stop()
		{
			Int64 stopTicks;
			QueryPerformanceCounter(out stopTicks);
			return (stopTicks - initialTicks) / frequency;
		}
	}
}
