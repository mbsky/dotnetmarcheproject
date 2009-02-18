using System;

namespace NHibernate.Linq
{
	public class DisposableAction : IDisposable
	{
		public delegate void Action();
		private readonly Action action;

		public DisposableAction(Action action)
		{
			this.action = action;
		}

		public void Dispose()
		{
			action();
		}
	}
}