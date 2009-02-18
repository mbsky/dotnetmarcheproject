using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;

namespace DotNetMarche.Infrastructure.Castle
{
	class SingletonContextLifecycle : ContextLifecycle
	{
		private Object _instance;

		/// <summary>
		/// resolve, if the instance is already created, not the need to create anymore.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override object Resolve(CreationContext context)
		{
			if (_instance != null) return _instance;
			_instance = base.Resolve(context);
			return _instance;
		}
	}
}
