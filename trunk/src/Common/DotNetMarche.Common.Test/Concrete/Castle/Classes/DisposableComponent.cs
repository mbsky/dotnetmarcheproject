using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Common.Test.Concrete.Castle.Classes;

namespace DotNetMarche.Common.Test.Concrete.Castle.Classes
{
	public class DisposableComponent : IDisposable {
 
		public ITest ITest;
		public Boolean IsDisposed = false;

		public DisposableComponent(ITest iTest) {
			ITest = iTest;
		}

		#region IDisposable Members
		 
		public void Dispose() {
			IsDisposed = true;
		}

		#endregion
	}
}