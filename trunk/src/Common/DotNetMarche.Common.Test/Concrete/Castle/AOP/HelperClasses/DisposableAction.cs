using System;
using System.Collections.Generic;
using System.Text;

namespace Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses {
	public class DisposableAction :IDisposable {
		public  delegate void DisposeAction();

		public DisposableAction(DisposeAction action) {
			this.action = action;
		}

		private readonly DisposeAction action;

		#region IDisposable Members

		public void Dispose() {
			action();
		}

		#endregion
	}
}
