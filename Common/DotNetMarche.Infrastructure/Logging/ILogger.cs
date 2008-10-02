using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Logging
{

	public enum LogLevel
	{
		Verbose = 1, 
		Info = 2,
		Warning = 3,
		Error = 4,
		Critical = 5,
	}

	public interface ILogger
	{
		LogLevel ActualLevel { get;  }
		void Log(LogLevel level, String message, Exception ex);
	}
}
