using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	public class OrOperator : BinaryOperator  {

		public override bool Matches(object actual) {
			base.actual = actual;
			return leftConstraint.Matches(actual) || rightConstraint.Matches(actual);
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			leftConstraint.WriteDescriptionTo(writer);
			writer.WriteConnector("OR");
			rightConstraint.WriteDescriptionTo(writer);
		}

		public override void WriteActualValueTo(NUnit.Framework.MessageWriter writer) {
			writer.WriteMessageLine("Left part failed reason:\n\t");
			leftConstraint.WriteActualValueTo(writer);
			writer.WriteMessageLine("Right part failed reason:\n\t");
			rightConstraint.WriteActualValueTo(writer);
		}

		public override void WriteMessageTo(NUnit.Framework.MessageWriter writer) {
			if (leftConstraint is BinaryOperator)
				leftConstraint.WriteMessageTo(writer);
			else 
				writer.Write("false ");
			writer.Write("OR");
			if (rightConstraint is BinaryOperator)
				rightConstraint.WriteMessageTo(writer);
			else
				writer.Write("false ");
			base.WriteMessageTo(writer);
		}
	}
}