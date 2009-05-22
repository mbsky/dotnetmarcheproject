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

		internal static Boolean VerboseEnabled;
		internal static Boolean InfoEnabled;
		internal static Boolean WarningEnabled;
		internal static Boolean ErrorEnabled;
		internal static Boolean CriticalEnabled;

		internal static DisposableAction Override(ILogger overrideLogger)
		{
			ILogger current = logger;
			logger = overrideLogger;
			if (logger.ActualLevel <= LogLevel.Verbose) VerboseEnabled = true;
			if (logger.ActualLevel <= LogLevel.Info) InfoEnabled = true;
			if (logger.ActualLevel <= LogLevel.Warning) WarningEnabled = true;
			if (logger.ActualLevel <= LogLevel.Error) ErrorEnabled = true;
			if (logger.ActualLevel <= LogLevel.Critical) CriticalEnabled = true;
			return new DisposableAction(() => Logger.logger = current);
		}

		#endregion

		#region Verbose logging

		public static void Verbose(String message, Exception ex)
		{
			if (VerboseEnabled)
				logger.LogVerbose(message, ex);
		}

		public static void Verbose(String message)
		{
			if (VerboseEnabled)
				logger.LogVerbose(message, null);
		}

		public static void VerboseFormat(String message, params Object[] parameters)
		{
			if (VerboseEnabled)
				logger.LogVerbose(String.Format(message, parameters), null);
		}

		public static void VerboseFormat(String message, Exception ex, params Object[] parameters)
		{
			if (VerboseEnabled)
				logger.LogVerbose(String.Format(message, parameters), ex);
		}

		#endregion		
		
		
		#region Warning logging

		public static void Warning(String message, Exception ex)
		{
			if (WarningEnabled)
				logger.LogWarning(message, ex);
		}

		public static void Warning(String message)
		{
			if (WarningEnabled)
				logger.LogWarning(message, null);
		}

		public static void WarningFormat(String message, params Object[] parameters)
		{
			if (WarningEnabled)
				logger.LogWarning( String.Format(message, parameters), null);
		}

		public static void WarningFormat(String message, Exception ex, params Object[] parameters)
		{
			if (WarningEnabled)
				logger.LogWarning(String.Format(message, parameters), ex);
		}

		#endregion
		
		#region Info logging

		public static void Info(String message, Exception ex)
		{
			if (InfoEnabled)
				logger.LogInfo(message, ex);
		}

		public static void Info(String message)
		{
			if (InfoEnabled)
				logger.LogInfo(message, null);
		}

		public static void InfoFormat(String message, params Object[] parameters)
		{
			if (InfoEnabled)
				logger.LogInfo(String.Format(message, parameters), null);
		}

		public static void InfoFormat(String message, Exception ex, params Object[] parameters)
		{
			if (InfoEnabled)
				logger.LogInfo(String.Format(message, parameters), ex);
		}

		#endregion	
		
		#region Error logging

		public static void Error(String message, Exception ex)
		{
			if (ErrorEnabled)
				logger.LogError(message, ex);
		}

		public static void Error(String message)
		{
			if (ErrorEnabled)
				logger.LogError(message, null);
		}

		public static void ErrorFormat(String message, params Object[] parameters)
		{
			if (ErrorEnabled)
				logger.LogError(String.Format(message, parameters), null);
		}

		public static void ErrorFormat(String message, Exception ex, params Object[] parameters)
		{
			if (ErrorEnabled)
				logger.LogError(String.Format(message, parameters), ex);
		}

		#endregion
		
		#region Critical logging

		public static void Critical(String message, Exception ex)
		{
			if (CriticalEnabled)
				logger.LogCritical(message, ex);
		}

		public static void Critical(String message)
		{
			if (CriticalEnabled)
				logger.LogCritical( message, null);
		}

		public static void CriticalFormat(String message, params Object[] parameters)
		{
			if (CriticalEnabled)
				logger.LogCritical( String.Format(message, parameters), null);
		}

		public static void CriticalFormat(String message, Exception ex, params Object[] parameters)
		{
			if (CriticalEnabled)
				logger.LogCritical(String.Format(message, parameters), ex);
		}

		#endregion
	}
}
