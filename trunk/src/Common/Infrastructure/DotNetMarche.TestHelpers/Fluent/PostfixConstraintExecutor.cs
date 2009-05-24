
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Operators;
using NUnit.Framework.Constraints;
using BinaryOperator = DotNetMarche.TestHelpers.Fluent.Operators.BinaryOperator;
namespace Nablasoft.Test.UnitTest {
	class PostfixConstraintExecutor {

		/// <summary>
		/// Combine all the constraints in only one constraint.
		/// </summary>
		/// <param name="expression">The expression, in the form of a list
		/// of constraint in a postfix expression form</param>
		/// <returns></returns>
		public Constraint ResolveExpression(IList<Constraint> expression) {
			Stack<Constraint> stack = new Stack<Constraint>();
			StringBuilder errors = new StringBuilder();
			foreach (Constraint constraint in expression) {
				if (constraint is BinaryOperator) {
					BinaryOperator binop = constraint as BinaryOperator;
					Constraint right = stack.Pop();
					Constraint left = stack.Pop();
					stack.Push(binop.SetConstraint(left, right));
				} else if (constraint is UnaryOperator) {
					Debug.Assert(stack.Count > 0, "trying to apply an unary constraint on a null object");
					UnaryOperator unop = constraint as UnaryOperator;
					stack.Push(unop.SetConstraint(stack.Pop()));
				}
				else {
					stack.Push(constraint);
				}
			}
			return stack.Pop();
		}
	}
}
