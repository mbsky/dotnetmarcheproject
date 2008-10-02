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

		public Log4NetLogger(String configFileName)
		{
			String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
			FileInfo log4netConfiguration = new FileInfo(fileName);
			if (!log4netConfiguration.Exists)
			   throw new ApplicationException(String.Format("Configuration file {0} missing.", log4netConfiguration.FullName));
			XmlConfigurator.ConfigureAndWatch(log4netConfiguration);
			mainLog = LogManager.GetLogger("root");
		}

		#region ILogger Members

		public void Log(LogLevel level, string message, Exception ex)
		{
			//LoggingEvent evt = 
			//mainLog.Logger.Log()
		}

		public void Log(string category, LogLevel level, string message, Exception ex)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
