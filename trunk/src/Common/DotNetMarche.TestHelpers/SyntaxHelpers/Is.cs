using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.TestHelpers.Constraints;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.SyntaxHelpers
{
	public static partial class Is
	{
		public static Constraint ObjectEqual(Object otherObject)
		{
			return new ObjectEqualConstraint(otherObject);
		}
	}
}
