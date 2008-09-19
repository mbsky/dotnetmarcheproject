using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Base
{
	public static class IoC
	{
		private static IInversionOfControlContainer baseContainer;

		static IoC()
		{
			baseContainer = SettingsRegistry.IoC.CreateIoCContainer();
		}

		public static T Resolve<T>()
		{
			return baseContainer.Resolve<T>();
		}
	}
}
