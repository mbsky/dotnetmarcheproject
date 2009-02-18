using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Utils;

namespace DotNetMarche.Infrastructure.Concrete
{
	public static partial class With
	{
		public static Double PerformanceCounter(Action action)
		{
			PerfTimer timer = new PerfTimer();
			timer.Start();
			action();
			return timer.Stop();
		}

		public static void Transaction(Action action)
		{
			using (GlobalTransactionManager.BeginTransaction())
			{
				action();
			}
		}
	}
}
