using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest
{
	public static partial class AssertBase
	{
		public static void Assert(this object obj, AbstractConstraint c)
		{
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(c.Validate(obj));
		}
	}
}
