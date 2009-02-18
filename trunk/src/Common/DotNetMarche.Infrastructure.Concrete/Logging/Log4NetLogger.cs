using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Core;

namespace DotNetMarche.Infrastructure.Logging.Concrete
{
	public class Log4NetLogger : ILogger
	{

		private readonly ILog mainLog;
		private LogLevel level;

		public Log4NetLogger(String configFileName)
		{
			String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
			FileInfo log4netConfiguration = new FileInfo(fileName);
			if (!log4netConfiguration.Exists)
			   throw new ApplicationException(String.Format("Configuration file {0} missing.", log4netConfiguration.FullName));
			XmlConfigurator.ConfigureAndWatch(log4netConfiguration);
			mainLog = LogManager.GetLogger("root");
			if (mainLog.IsDebugEnabled)
				level = LogLevel.Verbose;
			else if (mainLog.IsInfoEnabled)
				level = LogLevel.Info;
			else if (mainLog.IsWarnEnabled)
				level = LogLevel.Warning;
			else if (mainLog.IsErrorEnabled)
				level = LogLevel.Error;
			else 
				level = LogLevel.Critical;


		}

		#region ILogger Members

		public LogLevel ActualLevel
		{
			get { return level; }
		}

		#endregion


		#region ILogger Members


		public void LogVerbose(string message, Exception ex)
		{
			mainLog.Debug(message, ex);
		}

		public void LogInfo(string message, Exception ex)
		{
			mainLog.Info(message, ex);
		}

		public void LogWarning(string message, Exception ex)
		{
			mainLog.Warn(message, ex);
		}

		public void LogError(string message, Exception ex)
		{
			throw new NotImplementedException();
		}

		public void LogCritical(string message, Exception ex)
		{
			throw new NotImplementedException();
		}

		public void LogVerbose(string message)
		{
			throw new NotImplementedException();
		}

		public void LogInfo(string message)
		{
			throw new NotImplementedException();
		}

		public void LogWarning(string message)
		{
			throw new NotImplementedException();
		}

		public void LogError(string message)
		{
			throw new NotImplementedException();
		}

		public void LogCritical(string message)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
