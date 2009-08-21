using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Constraints
{
	public class GreaterThanOrEqualConstraint : ComparisonConstraintBase
	{
		public GreaterThanOrEqualConstraint(object expected)
			: base(expected, num => num >= 0)
		{
			Expected = expected;
		}
	}
}
