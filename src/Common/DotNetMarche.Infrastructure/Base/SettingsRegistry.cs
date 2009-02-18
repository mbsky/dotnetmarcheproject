using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Configuration;

namespace DotNetMarche.Infrastructure.Base
{
	/// <summary>
	/// This static class is used internally to handle various 
	/// settings of the infrastructure.
	/// </summary>
	internal static class SettingsRegistry
	{
		private static DotNetMarcheInfrastructureSettings DefaultConfiguration
		{
			get { return (DotNetMarcheInfrastructureSettings) ConfigurationManager.GetSection("DotNetMarcheInfrastructure"); }
		}
		
		internal static IoCConfigurationSetting IoC {
			get{ return DefaultConfiguration.IoC;}
		}
	}
}
