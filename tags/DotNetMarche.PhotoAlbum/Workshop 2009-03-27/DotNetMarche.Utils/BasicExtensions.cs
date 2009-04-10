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

		/// <summary>
		/// This function permits to enumerate an object with foreach while modifiying the 
		/// original enumeration, this is simply a matter of creating a copy of the original
		/// enumeration.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<T> SafeEnumerate<T>(this IEnumerable<T> source)
		{
			List<T> copy = source.ToList();
			return copy;
		}
	}
}
