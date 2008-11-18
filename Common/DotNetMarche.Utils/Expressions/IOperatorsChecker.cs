using System;

namespace DotNetMarche.Utils.Expressions
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">represents the token type.</typeparam>
	public interface IOperatorsChecker<T> {

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
		/// return true if Operator A has more precedence respect operator B
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		Boolean OperatorAHasMorePrecedenceThanB(T a, T b);
		Boolean IsOpenBracket(T token);
		Boolean IsClosedBracket(T token);

		Boolean IsOperator(T token);
	}
}