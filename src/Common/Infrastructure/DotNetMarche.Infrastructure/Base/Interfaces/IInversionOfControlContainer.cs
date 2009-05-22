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
		/// <summary>
		/// Resolve the element for the type T with specific name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objectName">The name of the object we want to resolve.</param>
		/// <returns></returns>
		T Resolve<T>(String objectName);

		/// <summary>
		/// This is the version used when I want to specify from external 
		/// caller some of the dependencies. This is expecially useful for testing
		/// purpose.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
		T Resolve<T>(params object[] values);

		/// <summary>
		/// This is the version used when I want to specify from external 
		/// caller some of the dependencies. This is expecially useful for testing
		/// purpose.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <param name="name">Name of the object to retrieve</param>
		/// <returns></returns>
		T ResolveWithName<T>(String name, params object[] values);
	} 
}
