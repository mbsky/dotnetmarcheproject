using System;
using System.Collections.Generic;
using DotNetMarche.TestHelpers.Fluent.Operators;
using DotNetMarche.Utils.Expressions;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Expression
{
	public class ConstraintOperatorChecker : IOperatorsChecker<Constraint>
	{
		#region IOperatorsChecker<Constraint> Members

		/// <summary>
		/// A constraint is an operator if he implements some of the interfaces
		/// of the standard operators.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool IsBinaryOperator(Constraint token)
		{
			return token is BinaryOperator; // || token is UnaryOperator;
		}

		public Boolean IsUnaryOperator(Constraint token)
		{
			return token is UnaryOperator;
		}

		private static Dictionary<Type, Int32> precedences;
		static ConstraintOperatorChecker()
		{
			precedences = new Dictionary<Type, Int32> {
            	{typeof(CountItemConstraint), 10}, {typeof(AndOperator), 50}, {typeof(OrOperator), 20}
            };
		}

		/// <summary>
		/// Here we have precedence of operator
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool OperatorAHasMorePrecedenceThanB(Constraint a, Constraint b)
		{
			return precedences[a.GetType()] > precedences[b.GetType()];
		}

		public bool IsOpenBracket(Constraint token)
		{
			return token is OpenBracket;
		}

		public bool IsClosedBracket(Constraint token)
		{
			return token is CloseBracket;
		}

		#endregion

		#region IOperatorsChecker<Constraint> Members

		#endregion

		#region IEnumerable<Constraint> Members

		public IEnumerator<Constraint> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}