using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	class OpenBracket : Constraint {

		public override bool Matches(object actual) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}

		public override void WriteDescriptionTo(MessageWriter writer) {
			throw new Exception("This is not a real constraint is introduced to define expression");
		}
	}
}