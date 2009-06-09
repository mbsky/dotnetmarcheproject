using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetMarche.MsTest
{
	public class FluentAssert
	{
		public static void That(Object obj, IConstraint c)
		{
			Assert.IsTrue( c.Validate(obj));
		}

		public static void That(Object obj, IConstraint c, String message)
		{
			Assert.IsTrue(c.Validate(obj), message);
		}
	}
}
