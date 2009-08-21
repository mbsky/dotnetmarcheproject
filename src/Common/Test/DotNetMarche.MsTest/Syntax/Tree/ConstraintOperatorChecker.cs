using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.MsTest.Syntax.Operators;
using DotNetMarche.Utils.Expressions;

namespace DotNetMarche.MsTest.Syntax.Tree
{
	public class ConstraintOperatorChecker : IOperatorsChecker<IConstraint>
	{
		#region IOperatorsChecker<Constraint> Members

		/// <summary>
		/// A constraint is an operator if he implements some of the interfaces
		/// of the standard operators.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool IsBinaryOperator(IConstraint token)
		{
			return token is BinaryConstraint; 
		}

		public Boolean IsUnaryOperator(IConstraint token)
		{
			return token is UnaryConstraint;
		}

		private static Dictionary<Type, Int32> precedences;

		static ConstraintOperatorChecker()
		{
			precedences = new Dictionary<Type, Int32> {
            	{typeof(AndConstraint), 50}, {typeof(OrConstraint), 20}
            };
		}

		/// <summary>
		/// Here we have precedence of operator
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool OperatorAHasMorePrecedenceThanB(IConstraint a, IConstraint b)
		{
			return precedences[a.GetType()] > precedences[b.GetType()];
		}

		public bool IsOpenBracket(IConstraint token)
		{
			return token is OpenBracket;
		}

		public bool IsClosedBracket(IConstraint token)
		{
			return token is CloseBracket;
		}

		#endregion

		#region IOperatorsChecker<Constraint> Members

		#endregion


		#region IEnumerable<IConstraint> Members

		public IEnumerator<IConstraint> GetEnumerator()
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
