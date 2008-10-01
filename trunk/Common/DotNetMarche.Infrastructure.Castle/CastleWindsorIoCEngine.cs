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

		#endregion
	}
}