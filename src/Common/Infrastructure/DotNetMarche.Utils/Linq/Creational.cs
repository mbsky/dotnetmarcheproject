using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Linq
{
	public static class Creational
	{
		public static T[] Fill<T>(this T[] original, Func<T> creator)
		{
			for (Int32 I = 0; I < original.Length; ++I)
				original[I] = creator();
			return original;
		}
	}
}
