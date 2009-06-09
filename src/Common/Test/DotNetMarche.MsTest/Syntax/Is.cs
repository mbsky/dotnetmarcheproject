using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Constraints;

namespace DotNetMarche.MsTest.Syntax
{
	public partial class Is
	{
		public static IConstraint EqualsTo(Object obj)
		{
			return new EqualsConstraint(obj);
		}
	}
}
