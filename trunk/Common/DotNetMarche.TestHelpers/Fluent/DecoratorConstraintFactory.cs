using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;
using Nablasoft.Test.UnitTest.Operators;
using NUnit.Framework.Constraints;
using Nablasoft.Test.UnitTest.Operators;

namespace Nablasoft.Test.UnitTest {
	public abstract class DecoratorConstraintFactory {
		/// <summary>
		/// Useful when we need to create duplicate of this decorator constraint
		/// </summary>
		/// <returns></returns>
		public abstract UnaryOperator Create(Constraint constraint);
	}
}
