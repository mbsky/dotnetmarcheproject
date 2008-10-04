using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.AuxClasses
{
	public class SimpleUnknown
	{
		public Int32 AMethod()
		{
			return 1;
		}

		public Int32 BMethod(Int32 val)
		{
			return val * 2;
		}		
		
		public Int32 BMethod(String val)
		{
			return val.Length * 2;
		}

		public String SMethod()
		{
			return "Hello";
		}
	}
}
