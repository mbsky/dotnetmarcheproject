using System;


namespace DotNetMarche.Common.Test.Concrete.Castle.Classes
{
	public class DisposableTest : IDisposable, ITest {
		public static Int32 NumOfDispose = 0;
		 
		private byte[] big = new byte[1000000];

		#region IDisposable Members

		public Boolean isDisposed = false;

		public void Dispose() {
			isDisposed = true;
			NumOfDispose++;
		} 

		#endregion

		#region ITest Members

		public bool IsDisposed {
			get { return isDisposed; }
		}

		#endregion
	}
}