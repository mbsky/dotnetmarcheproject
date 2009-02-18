using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	class AndOperator : BinaryOperator {
		private Boolean leftOk;
		private Boolean rightOk;
		public override bool Matches(object actual) {
			base.actual = actual;
			return (leftOk = leftConstraint.Matches(actual)) && 
			       (rightOk = rightConstraint.Matches(actual));
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			writer.Write("[");
			leftConstraint.WriteDescriptionTo(writer);
			writer.WriteConnector("AND");
			rightConstraint.WriteDescriptionTo(writer);
			writer.Write("]");
		}

		public override void WriteActualValueTo(NUnit.Framework.MessageWriter writer) {
			if (!leftOk) {
				writer.WriteMessageLine("Left part of the And is false because ");
				leftConstraint.WriteActualValueTo(writer);
			} else if (!rightOk) {
				writer.WriteMessageLine("Right part of the And is false because ", leftOk);
				rightConstraint.WriteActualValueTo(writer);
			} else {
				Debug.Fail("This code should be not reached");
			}
		}

		public override void WriteMessageTo(NUnit.Framework.MessageWriter writer) {
			//writer.WriteMessageLine("{0} AND {1}", leftOk, leftOk ? rightOk.ToString() : "NotEvaluated");
			base.WriteMessageTo(writer);
		}
	}
}