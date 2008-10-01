using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Concrete
{
	public class InMemoryConfigurationRegistry : IConfigurationRegistry
	{
		public InMemoryConfigurationRegistry()
		{
			ConnStrings = new Dictionary<String, ConnectionStringSettings>();
		}

		#region IConfigurationRegistry Members

		internal Dictionary<String, ConnectionStringSettings> ConnStrings { get; set; }

		public ConnectionStringSettings MainConnectionString
		{
			get { return ConnStrings.First().Value; }
		}

		public ConnectionStringSettings ConnectionString(string name)
		{
			return ConnStrings[name];
		}

		#endregion
	}
}
