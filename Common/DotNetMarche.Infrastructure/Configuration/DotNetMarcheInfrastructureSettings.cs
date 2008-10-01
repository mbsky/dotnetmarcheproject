using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Configuration
{
	public class DotNetMarcheInfrastructureSettings : ConfigurationSection
	{

		[ConfigurationProperty("IoC", IsRequired = true)]
		public IoCConfigurationSetting IoC
		{
			get
			{
				return (IoCConfigurationSetting) this["IoC"];
			}
		}


	}
}
