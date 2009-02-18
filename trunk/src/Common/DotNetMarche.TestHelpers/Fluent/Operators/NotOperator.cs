using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	public class NotOperator : UnaryOperator {

		public override bool Matches(object actual) {
			base.actual = actual;
			return !baseConstraint.Matches(actual);
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			writer.WritePredicate("Not ");
			baseConstraint.WriteDescriptionTo(writer);
		}
	}
}