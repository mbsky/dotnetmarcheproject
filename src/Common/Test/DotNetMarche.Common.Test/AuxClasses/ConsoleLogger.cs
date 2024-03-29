﻿using System;
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

		#region ILogger Members


		public void LogVerbose(string message, Exception ex)
		{
			Log(LogLevel.Verbose, message, ex);
		}

		public void LogInfo(string message, Exception ex)
		{
			Log(LogLevel.Info, message, ex);
		}

		public void LogWarning(string message, Exception ex)
		{
			Log(LogLevel.Warning, message, ex);
		}

		public void LogError(string message, Exception ex)
		{
			Log(LogLevel.Error, message, ex);
		}

		public void LogCritical(string message, Exception ex)
		{
			Log(LogLevel.Critical, message, ex);
		}

		public void LogVerbose(string message)
		{
			Log(LogLevel.Verbose, message, null);
		}

		public void LogInfo(string message)
		{
			Log(LogLevel.Info, message, null);
		}

		public void LogWarning(string message)
		{
			Log(LogLevel.Warning, message, null);
		}

		public void LogError(string message)
		{
			Log(LogLevel.Error, message, null);
		}

		public void LogCritical(string message)
		{
			Log(LogLevel.Critical, message, null);
		}

		#endregion
	}
}
