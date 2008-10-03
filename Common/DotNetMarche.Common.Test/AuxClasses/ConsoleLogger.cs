using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Logging;

namespace DotNetMarche.Common.Test.AuxClasses
{
	public class ConsoleLogger : ILogger
	{
		#region ILogger Members

		public LogLevel ActualLevel
		{
			get { return LogLevel.Verbose; }
		}

		public void Log(LogLevel level, string message, Exception ex)
		{
			if (level < LogLevel.Error)
				Console.WriteLine("{0}:\t{1} {2}", level, message, ex);
			else
				Console.Error.WriteLine("{0}:\t{1} {2}", level, message, ex);
		}

		#endregion
	}
}
