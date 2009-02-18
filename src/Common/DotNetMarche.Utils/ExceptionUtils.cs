using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetMarche.Utils
{
	public static class ExceptionUtils
	{
		/// <summary>
		/// Tells you if you are inside an exception handler or not. 
		/// It is useful inside the dispose method launched by the using clause.
		/// </summary>
		/// <returns></returns>
		public static Boolean IsInExceptionHandler()
		{
			return Marshal.GetExceptionPointers() != IntPtr.Zero ||
				 Marshal.GetExceptionCode() != 0;
		}
	}
}
