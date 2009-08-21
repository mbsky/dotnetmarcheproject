using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Castle
{
	public class CastleWindsorIoCEngine : IInversionOfControlContainer
	{

		private WindsorContainer container;

		/// <summary>
		/// Construct the real engine, all engine have a string for configuration, for Castle is 
		/// the xmlFileName of the file where we implement the settings.
		/// </summary>
		/// <param name="xmlFileName"></param>
		public CastleWindsorIoCEngine(String xmlFileName)
		{
			container = new WindsorContainer(xmlFileName);
		}

		#region IInversionOfControlContainer Members

		public T Resolve<T>()
		{
			return container.Resolve<T>();
		}

		public T Resolve<T>(String objectName)
		{
			return container.Resolve<T>(objectName);
		}

		/// <summary>
		/// This is the version used when I want to specify from external 
		/// caller some of the dependencies. This is expecially useful for testing
		/// purpose.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
public T Resolve<T>(params object[] values)
{
	System.Collections.Hashtable arguments = new System.Collections.Hashtable();
	for (Int32 I = 0; I < values.Length; I += 2)
	{
		arguments.Add(values[I], values[I + 1]);
	}
	return container.Resolve<T>(arguments);
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
		public T ResolveWithName<T>(String name, params object[] values)
		{
			System.Collections.Hashtable arguments = new System.Collections.Hashtable();
			for (Int32 I = 0; I < values.Length; I += 2)
			{
				arguments.Add(values[I], values[I + 1]);
			}
			return container.Resolve<T>(name, arguments);
		}

		#endregion
	}
}