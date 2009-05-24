using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	class CloseBracket : Constraint {

		public override bool Matches(object actualObject) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}

		public override void WriteDescriptionTo(MessageWriter writer) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}
	}
}