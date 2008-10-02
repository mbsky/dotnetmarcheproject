using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Logging
{
	public static class Logger
	{
		private static readonly ILogger concrete;
		static Logger()
		{
			concrete = IoC.Resolve<ILogger>();
		}

		#region Verbose logging

		public static void Verbose(String category, String message, Exception ex)
		{
			concrete.Log(category, LogLevel.Verbose, message, ex);
		}

		public static void Verbose(String message, Exception ex)
		{
			concrete.Log(String.Empty, LogLevel.Verbose, message, ex);
		}

		public static void Verbose(String message)
		{
			concrete.Log(String.Empty, LogLevel.Verbose, message, null);
		}

		public static void VerboseFormat(String message, params Object[] parameters)
		{
			concrete.Log(String.Empty, LogLevel.Verbose, String.Format(message, parameters), null);
		}

		public static void VerboseFormat(String message, Exception ex, params Object[] parameters)
		{
			concrete.Log(String.Empty, LogLevel.Verbose, String.Format(message, parameters), ex);
		}

		public static void VerboseFormat(String category, String message, Exception ex, params Object[] parameters)
		{
			concrete.Log(String.Empty, LogLevel.Verbose, String.Format(message, parameters), ex);
		}

		#endregion
	}
}
