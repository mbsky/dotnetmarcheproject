using System;
using System.Collections.Generic;
using DotNetMarche.TestHelpers.Fluent;
using DotNetMarche.TestHelpers.Fluent.Comparers;
using DotNetMarche.TestHelpers.Fluent.Expression;
using DotNetMarche.TestHelpers.Fluent.Operators;
using DotNetMarche.Utils.Expressions;
using Nablasoft.Test.UnitTest.Operators;
using NUnit.Framework.Constraints;
using NotOperator = DotNetMarche.TestHelpers.Fluent.Operators.NotOperator;
using AndOperator = DotNetMarche.TestHelpers.Fluent.Operators.AndOperator;
using OrOperator = DotNetMarche.TestHelpers.Fluent.Operators.OrOperator;
namespace Nablasoft.Test.UnitTest
{
	public class MyConstraintBuilder : IResolveConstraint
	{

		public MyConstraintBuilder()
		{
			constraints = new Stack<Constraint>();
		}

		private MyConstraintBuilder(MyConstraintBuilder original)
		{
			constraints = new Stack<Constraint>(original.constraints);
		}

		private Stack<Constraint> constraints;

		public MyConstraintBuilder Property(String propertyName, Object propertyValue)
		{
			PropertyConstraint pc = new PropertyConstraint(propertyName, new MyEqualToConstraint(propertyValue));
			constraints.Push(pc);
			return this;
		}

		public PropertyAdaptor Property(String propertyName)
		{
			return new PropertyAdaptor(propertyName, this);
		}

		#region Direct Access

		public MyConstraintBuilder Push(Constraint constraint)
		{
			constraints.Push(constraint);
			return this;
		}

		#endregion

		#region property for chaining

		#region logical operators

		public MyConstraintBuilder And
		{
			get
			{
				constraints.Push(new AndOperator());
				return this;
			}
		}

		public MyConstraintBuilder Or
		{
			get
			{
				constraints.Push(new OrOperator());
				return this;
			}
		}

		public MyConstraintBuilder Not
		{
			get
			{
				constraints.Push(new NotOperator());
				return this;
			}
		}

		#endregion

		#region Collection operators

		public MyConstraintBuilder None
		{
			get
			{
				constraints.Push(new CountItemConstraint(0));
				return this;
			}
		}


		public MyConstraintBuilder One
		{
			get
			{
				constraints.Push(new CountItemConstraint(1));
				return this;
			}
		}

		public MyConstraintBuilder Twice
		{
			get
			{
				constraints.Push(new CountItemConstraint(2));
				return this;
			}
		}

		public MyConstraintBuilder All
		{
			get
			{
				constraints.Push(new CountItemConstraint(-1));
				return this;
			}
		}

		public MyConstraintBuilder Count(Int32 countOccurrence)
		{
			constraints.Push(new CountItemConstraint(countOccurrence));
			return this;
		}

		#endregion

		#region Standard operators

		public MyConstraintBuilder EqualTo(Object value)
		{
			constraints.Push(new MyEqualToConstraint(value));
			return this;
		}

		public MyConstraintBuilder AllPropertiesEqualTo(object value)
		{
			constraints.Push(new ObjectComparerConstraint(value));
			return this;
		}

		public MyConstraintBuilder SomePropertiesEqualTo(object value, params String[] propertyNames)
		{
			ObjectComparerConstraint cnstr = new ObjectComparerConstraint(value);
			cnstr.PropertiesToCompare.AddRange(propertyNames);
			constraints.Push(cnstr);
			return this;
		}

		public MyConstraintBuilder LessThan(IComparable value)
		{
			constraints.Push(new LessThanConstraint(value));
			return this;
		}

		public MyConstraintBuilder GreaterThan(IComparable value)
		{
			constraints.Push(new GreaterThanConstraint(value));
			return this;
		}

		#endregion

		#region Bracket handling

		public MyConstraintBuilder this[char token]
		{
			get
			{
				switch (token)
				{
					case '(':
						constraints.Push(new OpenBracket());
						break;
					case ')':
						constraints.Push(new CloseBracket());
						break;
					default:
						throw new NotSupportedException();
				}
				return this;
			}
		}

		#endregion

		#endregion

		#region conversion operators

		/// <summary>
		/// Converts a MyConstraintBuilderOld to a simple constraints, it performs a composition
		/// of all the constraint defined in the builder
		/// </summary>
		/// <param name="cnstr"></param>
		/// <returns></returns>
		public static implicit operator Constraint(MyConstraintBuilder cnstr)
		{

			ExpressionConverterExt<List<Constraint>, Constraint, Constraint> converter =
				new ExpressionConverterExt<List<Constraint>, Constraint, Constraint>(
					new ConstraintOperatorChecker(), new DummyTokenizer(), new DummyTokenConverter());
			PostfixConstraintExecutor executor = new PostfixConstraintExecutor();
			List<Constraint> list = new List<Constraint>();
			list.AddRange(cnstr.constraints);
			list.Reverse();
			IList<Constraint> postfix = converter.InfixToPostfix(list);
			return executor.ResolveExpression(postfix);

			//Constraint currentConstraint = null;
			//Stack<Constraint> collapsed = new Stack<Constraint>();
			//while (cnstr.constraints.Count > 0) {
			//   Constraint thisConstraint = cnstr.constraints.Pop();
			//   //Now we have a constraint we should understand if it is a constraint or an
			//   //unary operator or a binary operator. 
			//   UnaryOperator unop = thisConstraint as UnaryOperator;
			//   if (thisConstraint is UnaryOperator) {
			//      //This is an unary operator so there is the need to compose
			//      currentConstraint = ((UnaryOperator)thisConstraint).SetConstraint(currentConstraint);
			//   }
			//   else {
			//      //This is a real constraint, begin to accumulate and push the last
			//      if (currentConstraint != null)
			//         collapsed.Push(currentConstraint);
			//      currentConstraint = thisConstraint;
			//   }
			//}
			//collapsed.Push(currentConstraint);
			////now we have the list of the real Constraint, let accumulate with and
			//currentConstraint = collapsed.Pop();
			//while (collapsed.Count > 1) {
			//   Constraint constraintToAccumulate = collapsed.Pop();
			//   BinaryOperator binop = constraintToAccumulate as BinaryOperator ?? new AndOperator();
			//   currentConstraint = binop.SetConstraint(collapsed.Pop(), currentConstraint);
			//}
			//return currentConstraint;
		}

		#endregion

		#region operator overloads

		/// <summary>
		/// operator and compose the builder with an and clause
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MyConstraintBuilder operator &(MyConstraintBuilder left, MyConstraintBuilder right)
		{
			MyConstraintBuilder newbuilder = new MyConstraintBuilder();
			newbuilder.constraints.Push(new OpenBracket());
			AppendToStack(newbuilder.constraints, left.constraints);
			newbuilder.constraints.Push(new AndOperator());
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
		public static MyConstraintBuilder operator |(MyConstraintBuilder left, MyConstraintBuilder right)
		{
			MyConstraintBuilder newbuilder = new MyConstraintBuilder();
			newbuilder.constraints.Push(new OpenBracket());
			AppendToStack(newbuilder.constraints, left.constraints);
			newbuilder.constraints.Push(new OrOperator());
			AppendToStack(newbuilder.constraints, right.constraints);
			newbuilder.constraints.Push(new CloseBracket());
			return newbuilder;
		}

		#endregion

		#region Helpers

		private static void AppendToStack(Stack<Constraint> original, Stack<Constraint> append)
		{
			Stack<Constraint> temp = new Stack<Constraint>();
			foreach (Constraint c in append)
			{
				//This are in reverse order.
				temp.Push(c);
			}
			foreach (Constraint c in temp)
			{
				//This are in reverse order.
				original.Push(c);
			}
		}

		#endregion

        #region IResolveConstraint Members

        public Constraint Resolve()
        {
            return this;
        }

        #endregion
    }
}
