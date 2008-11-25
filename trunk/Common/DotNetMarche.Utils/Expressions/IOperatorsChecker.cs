using System;
using System.Collections.Generic;

namespace DotNetMarche.Utils.Expressions
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">represents the token type.</typeparam>
	public interface IOperatorsChecker<T> : IEnumerable<T> {

		/// <summary>
		/// Tells if the current token is an operator or a bracket or a value
		/// ex, in 3 + 5 - 40 only the token + and - are operators.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Boolean IsBinaryOperator(T token);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Boolean IsUnaryOperator(T token);

        /// <summary>
        /// Return true if the token is equal to the Member access operator
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Boolean IsMemberAccessOperator(T token);

		/// <summary>
		/// return true if Operator A has more precedence respect operator B
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		Boolean OperatorAHasMorePrecedenceThanB(T a, T b);

		/// <summary>
		/// Open bracket and close bracket alter parenthesis, they are treated as 
		/// special operators.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Boolean IsOpenBracket(T token);

		/// <summary>
		/// <see cref="IsOpenBracket"/>
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Boolean IsClosedBracket(T token);

		/// <summary>
		/// return a value that tells if the token is a known operator. Remember that parenthesis
		/// should be considered as operators.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Boolean IsOperator(T token);


	}
}