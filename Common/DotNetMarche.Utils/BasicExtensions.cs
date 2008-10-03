using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils
{
	public static class BasicExtensions
	{
		public static T SafeGet<K, T>(this IDictionary<K, T> dic, K key)
		{
			return dic.ContainsKey(key) ? dic[key] : default(T);
		}
	}
}
