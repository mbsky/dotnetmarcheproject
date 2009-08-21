using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Syntax.Operators;

namespace DotNetMarche.MsTest.Syntax.Tree
{
	class PostfixIConstraintExecutor
	{

		/// <summary>
		/// Combine all the IConstraints in only one IConstraint.
		/// </summary>
		/// <param name="expression">The expression, in the form of a list
		/// of IConstraint in a postfix expression form</param>
		/// <returns></returns>
		public IConstraint ResolveExpression(IList<IConstraint> expression)
		{
			Stack<IConstraint> stack = new Stack<IConstraint>();
			StringBuilder errors = new StringBuilder();
			foreach (IConstraint IConstraint in expression)
			{
				if (IConstraint is BinaryConstraint)
				{
					BinaryConstraint binop = IConstraint as BinaryConstraint;
					IConstraint right = stack.Pop();
					IConstraint left = stack.Pop();
					stack.Push(binop.SetConstraint(left, right));
				}
				else if (IConstraint is UnaryConstraint)
				{
					Debug.Assert(stack.Count > 0, "trying to apply an unary IConstraint on a null object");
					UnaryConstraint unop = IConstraint as UnaryConstraint;
					stack.Push(unop.SetConstraint(stack.Pop()));
				}
				else
				{
					stack.Push(IConstraint);
				}
			}
			return stack.Pop();
		}
	}
}
