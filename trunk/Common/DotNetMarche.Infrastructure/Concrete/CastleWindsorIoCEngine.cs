using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Concrete
{
	public class CastleWindsorIoCEngine : IInversionOfControlContainer
	{

		/// <summary>
		/// Construct the real engine, all engine have a string for configuration, for Castle is 
		/// the name of the file where we implement the settings.
		/// </summary>
		/// <param name="name"></param>
		public CastleWindsorIoCEngine(String name)
		{
		}

		#region IInversionOfControlContainer Members

		public T Resolve<T>()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
