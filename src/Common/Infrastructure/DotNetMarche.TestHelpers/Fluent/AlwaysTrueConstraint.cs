using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace Nablasoft.Test.UnitTest {

	/// <summary>
	/// A constraint that is always true
	/// </summary>
	class AlwaysTrueConstraint : Constraint  {

		public override bool Matches(object actual) {
			return true;
		}

		public override void WriteDescriptionTo(MessageWriter writer) {
			writer.WritePredicate("Alwais true constraint");
		}
	}
}
