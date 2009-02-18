using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Nablasoft.Test.UnitTest.Operators {
	public class CountWithLambdaConstraint<T> : Constraint {
			
		private readonly Int32 countOccurrence;
		private readonly Func<T, Boolean> discriminator;

		public CountWithLambdaConstraint(Int32 countOccurrence, Func<T, Boolean> discriminator) {
			this.countOccurrence = countOccurrence;
			this.discriminator = discriminator;
		}

		private Boolean MatchAll {
			get { return countOccurrence == -1; }
		}

		private Int32 MatchCount;

		public override bool Matches(object actual) {
			IEnumerable<T> sut = actual as IEnumerable<T>;
			if (sut == null) {
				throw new ArgumentException(
					String.Format("The actualObject value must be INnumerable<{0}>", typeof(T).Name) , "actualObject");
			}
			MatchCount = sut.Count(discriminator);
			return MatchCount == countOccurrence || (MatchCount == sut.Count() && MatchAll);
		}

		public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
			writer.WritePredicate("count(" + countOccurrence + ")");
		}

		public override void WriteActualValueTo(NUnit.Framework.MessageWriter writer) {
			writer.WriteActualValue(" found count (" + MatchCount.ToString() + ")");
			writer.Write("[");
			writer.WriteMessageLine("] expected count of {0}", countOccurrence);
		}

	}
}
