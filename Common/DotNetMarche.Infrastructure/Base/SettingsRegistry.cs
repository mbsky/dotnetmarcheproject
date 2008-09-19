using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Configuration;

namespace DotNetMarche.Infrastructure.Base
{
	internal static class SettingsRegistry
	{
		private static DotNetMarcheIntrastructureSettings DefaultConfiguration
		{
			get { return (DotNetMarcheIntrastructureSettings) ConfigurationManager.GetSection("DotNetMarcheInfrastructure"); }
		}
		
		internal static IoCConfigurationSetting IoC {
			get{ return DefaultConfiguration.IoC;}
		}
	}
}
