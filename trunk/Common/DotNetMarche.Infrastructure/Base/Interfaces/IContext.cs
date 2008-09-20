using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Base.Interfaces
{

	/// <summary>
	/// This interface represent a context, it is used to store data
	/// related to the concept of "current context" that can be thread in 
	/// windows application or httprequest in web contetx or whathever 
	/// you want for your architecture.
	/// </summary>
	interface IContext : IEnumerable<KeyValuePair<String, Object>>
	{
		Object GetData(String key);
		Object SetData(String key, Object value);
		Object this[String key] { get; set; }
	}
}
