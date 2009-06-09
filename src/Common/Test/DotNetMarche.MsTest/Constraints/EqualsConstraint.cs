using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Constraints
{
	public class EqualsConstraint : AbstractConstraint
	{
		public Object Expected { get; set; }

		public EqualsConstraint(object expected)
		{
			Expected = expected;
		}

		internal override bool InnerValidate(object subject)
		{
			return subject.Equals(Expected);
		}
	}
}
