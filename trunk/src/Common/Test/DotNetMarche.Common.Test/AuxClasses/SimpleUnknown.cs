using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.AuxClasses
{
	public class SimpleUnknown
	{
		private Int32 field = 1;

		public Int32 AMethod()
		{
			return field;
		}

		public Int32 BMethod(Int32 val)
		{
			return val * 2 * field;
		}		
		
		public Int32 BMethod(String val)
		{
			return val.Length * field;
		}

		public String SMethod()
		{
			return "Hello";
		}

		public Int32 Val { get; set; }

		public void VMethod()
		{
			Val = 10;
		}		
		
		public void VMethod(String param)
		{
			Val = param.Length;
		}
	}
}
