using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Comparers;

namespace DotNetMarche.MsTest.Constraints
{
	public class GreaterThanConstraint : ComparisonConstraintBase
	{
		public GreaterThanConstraint(object expected)
			: base(expected, num => num > 0)
		{
			Expected = expected;
		}
	}
}
