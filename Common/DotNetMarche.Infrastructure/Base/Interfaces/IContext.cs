using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Base.Interfaces
{

	interface IContext : IEnumerable<KeyValuePair<String, Object>>
	{
		Object GetData(String key);
		Object SetData(String key, Object value);
		Object this[String key] { get; set; }
	}
}
