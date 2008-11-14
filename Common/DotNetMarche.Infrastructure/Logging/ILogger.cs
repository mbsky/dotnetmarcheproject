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

	/// <summary>
	/// Generic interface of a logger.
	/// </summary>
	public interface ILogger
	{
		LogLevel ActualLevel { get;  }
		void LogVerbose(String message, Exception ex);
		void LogInfo(String message, Exception ex);
		void LogWarning(String message, Exception ex);
		void LogError(String message, Exception ex);
		void LogCritical(String message, Exception ex);
		void LogVerbose(String message);
		void LogInfo(String message);
		void LogWarning(String message);
		void LogError(String message);
		void LogCritical(String message);
	}
}
