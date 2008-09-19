using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Base
{
	static class ConfigurationRegistry
	{

		private static IConfigurationRegistry registry;

		static ConfigurationRegistry()
		{
			registry = IoC.Resolve<IConfigurationRegistry>();
		}

		public static ConnectionStringSettings MainConnectionString {
			get { return registry.MainConnectionString; }
		}

		/// <summary>
		/// Multiple connection string use a named connection string.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static ConnectionStringSettings ConnectionString(String name)
		{
			return registry.ConnectionString(name);
		}
	}
}
