using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Configuration
{
	public class IoCConfigurationSetting : ConfigurationElement
	{
		private const String IoCConcreteTypeConst = "IoCConcreteType";
		private const String IoCContainerNameConst = "IoCContainerName";

		[ConfigurationProperty(IoCConcreteTypeConst, IsRequired = true)]
		public String IoCConcreteType
		{
			get { return (String)this[IoCConcreteTypeConst]; }
			set { this[IoCConcreteTypeConst] = value; }
		}

		[ConfigurationProperty(IoCContainerNameConst, DefaultValue = "", IsRequired = false)]
		public String IoCContainerName
		{
			get { return (String)this[IoCContainerNameConst]; }
			set { this[IoCContainerNameConst] = value; }
		}

		/// <summary>
		/// Creates the ioc engine requested by the settings.
		/// </summary>
		/// <returns></returns>
		internal IInversionOfControlContainer CreateIoCContainer()
		{
			Type type = Type.GetType(IoCConcreteType);
			return (IInversionOfControlContainer) Activator.CreateInstance(type, new[] {IoCContainerName});
		}
	}
}
