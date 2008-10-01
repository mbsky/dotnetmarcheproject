using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Concrete
{
	/// <summary>
	/// Use configuration from the app.config standard .net file.
	/// </summary>
	class AppConfigConfigurationRegistry : IConfigurationRegistry 
	{
		#region IConfigurationRegistry Members

		/// <summary>
		/// Default connection is the first on the section
		/// </summary>
		public ConnectionStringSettings MainConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings[0]; }
		}

		public ConnectionStringSettings ConnectionString(string name)
		{
			 return ConfigurationManager.ConnectionStrings[name]; 
		}

		#endregion
	}
}
