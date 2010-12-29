﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Constraints
{
	public class EqualsConstraint : ComparisonConstraintBase
	{
		public EqualsConstraint(object expected) : base(expected, num => num == 0)
		{
			Expected = expected;
		}
	}
}