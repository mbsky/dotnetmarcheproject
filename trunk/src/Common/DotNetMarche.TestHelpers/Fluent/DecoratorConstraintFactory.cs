using DotNetMarche.TestHelpers.Fluent.Operators;
using NUnit.Framework.Constraints;

namespace Nablasoft.Test.UnitTest {
	public abstract class DecoratorConstraintFactory {
		/// <summary>
		/// Useful when we need to create duplicate of this decorator constraint
		/// </summary>
		/// <returns></returns>
		public abstract UnaryOperator Create(Constraint constraint);
	}
}
