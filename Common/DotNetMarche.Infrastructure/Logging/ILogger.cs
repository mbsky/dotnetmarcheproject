using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Logging
{

	public enum LogLevel
	{
		Verbose, 
		Info,
		Warning,
		Error,
		Critical
	}

	public interface ILogger
	{
		void Log(LogLevel level, String message, Exception ex);
		void Log(String category, LogLevel level, String message, Exception ex);
	}
}
