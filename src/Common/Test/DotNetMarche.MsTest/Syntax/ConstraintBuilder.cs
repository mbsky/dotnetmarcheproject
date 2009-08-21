using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Constraints;
using DotNetMarche.MsTest.Syntax.Operators;
using DotNetMarche.MsTest.Syntax.Tree;
using DotNetMarche.Utils.Expressions;

namespace DotNetMarche.MsTest.Syntax
{
	public class ConstraintBuilder : IConstraint
	{

		public ConstraintBuilder()
		{
			constraints = new Stack<IConstraint>();
		}

		private ConstraintBuilder(ConstraintBuilder original)
		{
			constraints = new Stack<IConstraint>(original.constraints);
		}

		private Stack<IConstraint> constraints;

		#region Direct Access

		public ConstraintBuilder Push(IConstraint constraint)
		{
			constraints.Push(constraint);
			return this;
		}

		#endregion

		#region Factories

		public static ConstraintBuilder ForEquals(Object obj)
		{
			ConstraintBuilder c = new ConstraintBuilder();
			c.Push(new EqualsConstraint(obj));
			return c;
		}

		public static ConstraintBuilder ForGreaterThan(Object obj)
		{
			ConstraintBuilder c = new ConstraintBuilder();
			c.Push(new GreaterThanConstraint(obj));
			return c;
		}

		public static ConstraintBuilder ForLesserThan(Object obj)
		{
			ConstraintBuilder c = new ConstraintBuilder();
			c.Push(new LessThanConstraint(obj));
			return c;
		}

		#endregion

		#region property for chaining

		#region logical operators

		public ConstraintBuilder And
		{
			get
			{
				constraints.Push(new AndConstraint());
				return this;
			}
		}

		public ConstraintBuilder Or
		{
			get
			{
				constraints.Push(new OrConstraint());
				return this;
			}
		}


		#endregion

		#region Standard operators

		public ConstraintBuilder EqualTo(Object value)
		{
			constraints.Push(new EqualsConstraint(value));
			return this;
		}


		public ConstraintBuilder LessThan(IComparable value)
		{
			constraints.Push(new LessThanConstraint(value));
			return this;
		}

		public ConstraintBuilder GreaterThan(IComparable value)
		{
			constraints.Push(new GreaterThanConstraint(value));
			return this;
		}

		#endregion

		#endregion

		#region conversion operators

		#endregion

		#region operator overloads

		/// <summary>
		/// operator and compose the builder with an and clause
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ConstraintBuilder operator &(ConstraintBuilder left, ConstraintBuilder right)
		{
			ConstraintBuilder newbuilder = new ConstraintBuilder();
			newbuilder.constraints.Push(new OpenBracket());
			AppendToStack(newbuilder.constraints, left.constraints);
			newbuilder.constraints.Push(new AndConstraint());
			AppendToStack(newbuilder.constraints, right.constraints);
			newbuilder.constraints.Push(new CloseBracket());
			return newbuilder;
		}

		/// <summary>
		/// operator and compose the builder with an and clause
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ConstraintBuilder operator |(ConstraintBuilder left, ConstraintBuilder right)
		{
			ConstraintBuilder newbuilder = new ConstraintBuilder();
			newbuilder.constraints.Push(new OpenBracket());
			AppendToStack(newbuilder.constraints, left.constraints);
			newbuilder.constraints.Push(new OrConstraint());
			AppendToStack(newbuilder.constraints, right.constraints);
			newbuilder.constraints.Push(new CloseBracket());
			return newbuilder;
		}

		#endregion

		#region Helpers

		private static void AppendToStack(Stack<IConstraint> original, Stack<IConstraint> append)
		{
			Stack<IConstraint> temp = new Stack<IConstraint>();
			foreach (IConstraint c in append)
			{
				//This are in reverse order.
				temp.Push(c);
			}
			foreach (IConstraint c in temp)
			{
				//This are in reverse order.
				original.Push(c);
			}
		}

		#endregion


		#region IConstraint Members

		public bool Validate(object subject)
		{
			ExpressionConverterExt<List<IConstraint>, IConstraint, IConstraint> converter =
			new ExpressionConverterExt<List<IConstraint>, IConstraint, IConstraint>(
				new ConstraintOperatorChecker(), new DummyTokenizer(), new DummyTokenConverter());
			PostfixIConstraintExecutor executor = new PostfixIConstraintExecutor();
			List<IConstraint> list = new List<IConstraint>();
			list.AddRange(constraints);
			list.Reverse();
			IList<IConstraint> postfix = converter.InfixToPostfix(list);
			IConstraint lastConstraint = executor.ResolveExpression(postfix);
			return lastConstraint.Validate(subject);
		}

		#endregion
	}
}
