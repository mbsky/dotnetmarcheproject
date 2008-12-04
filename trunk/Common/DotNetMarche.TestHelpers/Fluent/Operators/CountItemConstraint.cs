using System;
using System.Collections;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;
using NUnit.Framework.Constraints;
using Nablasoft.Test.UnitTest.Operators;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	public class CountItemConstraint : UnaryOperator {
		
		private readonly Int32 countOccurrence;

		public CountItemConstraint(Int32 countOccurrence, Constraint constraint) : this(countOccurrence) {
			baseConstraint = constraint;
		}

		public CountItemConstraint(int countOccurrence){
			this.countOccurrence = countOccurrence;
		}

		private Boolean MatchAll {
			get { return countOccurrence == -1; }
		}
		private Int32 MatchCount;
		public override bool Matches(object actualObject) {
			base.actual = actualObject;
			if (!(actualObject is ICollection)) {
				throw new ArgumentException("The actualObject value must be a collection", "actualObject");
			}
			MatchCount = 0;
			foreach (object obj2 in (ICollection)actualObject) {
				if (baseConstraint.Matches(obj2)) {
					MatchCount++;
					if (MatchCount > countOccurrence && !MatchAll) return false;
				}
			}
			return MatchCount == countOccurrence ||
			       (MatchCount == ((ICollection)actualObject).Count && MatchAll);

		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			writer.WritePredicate("count(" + countOccurrence + ")");
			baseConstraint.WriteDescriptionTo(writer);
		}

		public override void WriteActualValueTo(NUnit.Framework.MessageWriter writer) {
			writer.WriteActualValue(" found count (" + MatchCount.ToString() + ")");
			writer.Write("[");
			baseConstraint.WriteDescriptionTo(writer);
			writer.WriteMessageLine("] expected count of {0}", countOccurrence );
		}
	}
}