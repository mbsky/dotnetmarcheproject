using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	class CloseBracket : Constraint {

		public override bool Matches(object actualObject) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}
	}
}