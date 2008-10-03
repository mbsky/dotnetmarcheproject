using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Logging
{
	public static class Logger
	{
		#region Constructors and overriding

		private static ILogger logger;
		static Logger()
		{
			logger = IoC.Resolve<ILogger>();
		}

		internal static DisposableAction Override(ILogger overrideLogger)
		{
			ILogger current = logger;
			logger = overrideLogger;
			return new DisposableAction(() => Logger.logger = current);
		}

		#endregion

		#region Verbose logging

		public static void Verbose(String message, Exception ex)
		{
			if (logger.ActualLevel <= LogLevel.Verbose)
				logger.Log(LogLevel.Verbose, message, ex);
		}

		public static void Verbose(String message)
		{
			if (logger.ActualLevel <= LogLevel.Verbose)
				logger.Log(LogLevel.Verbose, message, null);
		}

		public static void VerboseFormat(String message, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Verbose)
				logger.Log(LogLevel.Verbose, String.Format(message, parameters), null);
		}

		public static void VerboseFormat(String message, Exception ex, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Verbose)
				logger.Log(LogLevel.Verbose, String.Format(message, parameters), ex);
		}

		#endregion		
		
		
		#region Warning logging

		public static void Warning(String message, Exception ex)
		{
			if (logger.ActualLevel <= LogLevel.Warning)
				logger.Log(LogLevel.Warning, message, ex);
		}

		public static void Warning(String message)
		{
			if (logger.ActualLevel <= LogLevel.Warning)
				logger.Log(LogLevel.Warning, message, null);
		}

		public static void WarningFormat(String message, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Warning)
				logger.Log(LogLevel.Warning, String.Format(message, parameters), null);
		}

		public static void WarningFormat(String message, Exception ex, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Warning)
				logger.Log(LogLevel.Warning, String.Format(message, parameters), ex);
		}

		#endregion
		
		#region Info logging

		public static void Info(String message, Exception ex)
		{
			if (logger.ActualLevel <= LogLevel.Info)
				logger.Log(LogLevel.Info, message, ex);
		}

		public static void Info(String message)
		{
			if (logger.ActualLevel <= LogLevel.Info)
				logger.Log(LogLevel.Info, message, null);
		}

		public static void InfoFormat(String message, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Info)
				logger.Log(LogLevel.Info, String.Format(message, parameters), null);
		}

		public static void InfoFormat(String message, Exception ex, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Info)
				logger.Log(LogLevel.Info, String.Format(message, parameters), ex);
		}

		#endregion	
		
		#region Error logging

		public static void Error(String message, Exception ex)
		{
			if (logger.ActualLevel <= LogLevel.Error)
				logger.Log(LogLevel.Error, message, ex);
		}

		public static void Error(String message)
		{
			if (logger.ActualLevel <= LogLevel.Error)
				logger.Log(LogLevel.Error, message, null);
		}

		public static void ErrorFormat(String message, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Error)
				logger.Log(LogLevel.Error, String.Format(message, parameters), null);
		}

		public static void ErrorFormat(String message, Exception ex, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Error)
				logger.Log(LogLevel.Error, String.Format(message, parameters), ex);
		}

		#endregion
		
		#region Critical logging

		public static void Critical(String message, Exception ex)
		{
			if (logger.ActualLevel <= LogLevel.Critical)
				logger.Log(LogLevel.Critical, message, ex);
		}

		public static void Critical(String message)
		{
			if (logger.ActualLevel <= LogLevel.Critical)
				logger.Log(LogLevel.Critical, message, null);
		}

		public static void CriticalFormat(String message, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Critical)
				logger.Log(LogLevel.Critical, String.Format(message, parameters), null);
		}

		public static void CriticalFormat(String message, Exception ex, params Object[] parameters)
		{
			if (logger.ActualLevel <= LogLevel.Critical)
				logger.Log(LogLevel.Critical, String.Format(message, parameters), ex);
		}

		#endregion
	}
}
