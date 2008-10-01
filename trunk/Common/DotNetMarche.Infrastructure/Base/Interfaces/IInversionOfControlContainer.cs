using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Base.Interfaces
{
	/// <summary>
	/// We abstray even the IoC container, we could use whatever container we want, but it has
	/// at least to satisfy a minimum interface
	/// </summary>
	public interface IInversionOfControlContainer
	{
		/// <summary>
		/// Resolve the default element for the type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Resolve<T>();
	}
}
