using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Linq
{
	public static class DynamicLinqExtension
	{
		/// <summary>
		/// This is the extension method that permits me to create a where condition
		/// with a simple string
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public static IEnumerable<T> Where<T>(this IEnumerable<T> source, String query)
		{
			Func<T, Boolean> predicate = DynamicLinq.ParseToFunction<T, Boolean>(query);
			return source.Where(predicate);
		}

		/// <summary>
		/// <see cref="Where{T}(IEnumerable{T},string)"/>.
		/// This version support parameters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="query"></param>
		/// <param name="param1"></param>
		/// <returns></returns>
		public static IEnumerable<T> Where<T>(this IEnumerable<T> source, String query, Object param1)
		{
			Func<T, Object, Boolean> predicate = DynamicLinq.ParseToFunction<T, Object, Boolean>(query);
			return source.Where(e => predicate(e, param1));  //Currying 
		}
	}
}
