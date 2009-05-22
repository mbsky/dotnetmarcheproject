using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Common.Test.Concrete.Castle.Classes;

namespace DotNetMarche.Common.Test.Concrete.Castle.Classes
{
	class NotDisposableComponent {
		public ITest ITest; 

		public NotDisposableComponent(ITest iTest) {
			ITest = iTest;
		}

	}
}