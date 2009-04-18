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

		public static T Resolve<T>(String objectName)
		{
			return baseContainer.Resolve<T>(objectName);
		}

		/// <summary>
		/// This is the version used when I want to specify from external 
		/// caller some of the dependencies. This is expecially useful for testing
		/// purpose.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
		public static T Resolve<T>(params object[] values)
		{
         return baseContainer.Resolve<T>(values);
		}

		/// <summary>
		/// This is the version used when I want to specify from external 
		/// caller some of the dependencies. This is expecially useful for testing
		/// purpose.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <param name="name">Name of the object to retrieve</param>
		/// <returns></returns>
		public static T ResolveWithName<T>(String name, params object[] values)
		{
         return baseContainer.Resolve<T>(name, values);
		}
	}
}
