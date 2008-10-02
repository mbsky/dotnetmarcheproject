using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Base
{
	static class CurrentContext
	{
		private static readonly IContext context;

		static CurrentContext()
		{
			context = IoC.Resolve<IContext>();
		}

		public static Object GetData(String key)
		{
			return context.GetData(key);
		}

		public static void SetData(String key, Object value)
		{
			context.SetData(key, value);
		}
		public static void ReleaseData(String key)
		{
			context.ReleaseData(key);
		}
		public static IEnumerator<KeyValuePair<String, Object>> Enumerate()
		{
			return context.GetEnumerator();
		}
	}
}
