using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DotNetMarche.Validator.Tests.Extras.NHibernate
{
	class SessionHelper
	{
		internal static ISessionFactory CreateConfigurationForConfigFileName(string configurationFile)
		{
			try
			{
				global::NHibernate.Cfg.Configuration cfg = new global::NHibernate.Cfg.Configuration();
				cfg.Configure(configurationFile);
				return cfg.BuildSessionFactory();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("NHibernate exception stack");
				String formatter = "--";
				do
				{
					System.Diagnostics.Debug.WriteLine(formatter + ex.Message);
					ex = ex.InnerException;
					formatter += "--";
				} while (ex != null);
				throw;
			}
		}
	}
}
