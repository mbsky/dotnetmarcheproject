using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.TestHelpers.Fluent;

namespace Nablasoft.Test.UnitTest {
	public static class CollectionAssert {

		public static CollectionConstraintBuilder<T> Has<T>(
			this IEnumerable<T> sut) {

			return new CollectionConstraintBuilder<T>(sut);
		}

	}
}
