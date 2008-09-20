using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Base
{
	static class CurrentContext
	{
		private static IContext context;

		static CurrentContext()
		{
			context = IoC.Resolve<IContext>();
		}

		public static Object GetData(String key)
		{
			return context.GetData(key);
		}

		public static Object SetData(String key, Object value)
		{
			return context.SetData(key, value);
		}

		public static IEnumerator<KeyValuePair<String, Object>> Enumerate()
		{
			return context.GetEnumerator();
		}
	}
}
