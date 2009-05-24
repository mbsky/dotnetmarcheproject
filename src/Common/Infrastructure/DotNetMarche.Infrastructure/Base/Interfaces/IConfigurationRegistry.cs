using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Base.Interfaces
{
	/// <summary>
	/// All application shares some common configuration basic. The configuration
	/// registry is an abstraction of a single common place where everyone can store
	/// and retrieve global configurations.
	/// </summary>
	public interface IConfigurationRegistry
	{
		/// <summary>
		/// Single connection string program use a single configuration
		/// </summary>
		ConnectionStringSettings MainConnectionString { get; }

		/// <summary>
		/// Multiple connection string use a named connection string.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		ConnectionStringSettings ConnectionString(String name);

	}
}
