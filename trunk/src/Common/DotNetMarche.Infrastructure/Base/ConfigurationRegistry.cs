using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Base
{
	/// <summary>
	/// This class represent the current configuration with a registry pattern
	/// 
	/// </summary>
	public static class ConfigurationRegistry
	{

		private static IConfigurationRegistry registry;

		internal static IDisposable Override(IConfigurationRegistry overrideRegistry)
		{
			IConfigurationRegistry current = registry;
			registry = overrideRegistry;
			return new DisposableAction(() => registry = current);
		}

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
			if (String.IsNullOrEmpty(name)) return MainConnectionString;
			return registry.ConnectionString(name);
		}
	}
}
