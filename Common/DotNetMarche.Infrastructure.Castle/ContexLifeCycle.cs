﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Castle.Core;
using Castle.MicroKernel.Lifestyle;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Castle
{
	public class ContextLifecycle : AbstractLifestyleManager
	{

		#region Execution Context handling

		private const String dataId = "FABD0E79-E421-4f19-9C8A-A5307828E72D";

		private class Context : IDisposable
		{

			public Object Add(Object obj)
			{
				if (obj is IDisposable)
					disposeList.AddLast((IDisposable)obj);
				return obj;
			}
			private readonly LinkedList<IDisposable> disposeList = new LinkedList<IDisposable>();

			public void Dispose()
			{
				foreach (IDisposable d in disposeList)
					d.Dispose();
			}
		}

		/// <summary>
		/// access the context bag for the current call.
		/// </summary>
		private Context CurrentContext
		{
			get { return GetCurrentThreadContext() ?? defaultContext; }
		}

		private static Context GetCurrentThreadContext()
		{
			return CallContext.GetData(dataId) as Context;
		}

		#endregion

		#region Manual context set

		/// <summary>
		/// Begin a thread context, now all call to resolve are cached in the context.
		/// </summary>
		/// <returns></returns>
		public static DisposableAction BeginThreadContext()
		{
			Context c = GetCurrentThreadContext();
			if (c != null)
				throw new InvalidOperationException("Another thread context was already begun");
			CallContext.SetData(dataId, new Context());
			return new DisposableAction(delegate()
			{
				GetCurrentThreadContext().Dispose();
				CallContext.FreeNamedDataSlot(dataId);
			});
		}


		#endregion

		#region Container and lifecycle function

		/// <summary>
		/// This is the context for this lifecycle manager. when a thread context is not active
		/// this is the real active context. This context will be released toghether with the
		/// whole container.
		/// </summary>
		readonly Context defaultContext = new Context();

		/// <summary>
		/// This is the core function, lets the context resolve the object then add it into
		/// the current context and then dispose it.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override object Resolve(global::Castle.MicroKernel.CreationContext context)
		{
			Object instance = base.Resolve(context);
			//Check if the component model that request the component is singleton.
			if (context.Handler.ComponentModel.LifestyleType == LifestyleType.Singleton)
				return defaultContext.Add(instance);
			else
				return CurrentContext.Add(instance);
		}

		public override void Dispose()
		{
			defaultContext.Dispose();
		}
		#endregion

		/// <summary>
		/// new in the trunk version.
		/// </summary>
		public override bool ContainerShouldTrackForDisposal
		{
			get { throw new NotImplementedException(); }
		}
	}
}
