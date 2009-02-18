using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	internal class MyEqualToConstraint : EqualConstraint
	{
		public MyEqualToConstraint(object expected) : base(expected)
		{
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer)
		{
			writer.WritePredicate("=");
			base.WriteDescriptionTo(writer);
		}
	}
}