using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Constraints;

namespace DotNetMarche.MsTest.Syntax
{
	public partial class Is
	{
		public static ConstraintBuilder EqualsTo(Object obj)
		{
			return ConstraintBuilder.ForEquals(obj);
		}

		public static ConstraintBuilder GreaterThan(Object obj)
		{
			return ConstraintBuilder.ForGreaterThan(obj);
		}

		public static ConstraintBuilder LesserThan(Object obj)
		{
			return ConstraintBuilder.ForLesserThan(obj);
		}
	}
}
