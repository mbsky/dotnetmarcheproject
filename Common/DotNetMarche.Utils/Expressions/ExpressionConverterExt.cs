using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions
{
	public class ExpressionConverterExt<E, S, D>
	{
		private IOperatorsChecker<S> opChecker;
		private ITokenizer<E, S> tokenizer;
		private ITokenConverter<S, D> tokenConverter;

		public ExpressionConverterExt(IOperatorsChecker<S> opChecker, ITokenizer<E, S> tokenizer, ITokenConverter<S, D> tokenConverter)
		{
			this.opChecker = opChecker;
			this.tokenizer = tokenizer;
			this.tokenConverter = tokenConverter;
		}

		/// <summary>
		/// Convert an infix to postfix, all token should be separated by a space
		/// to make the parsing simple. The algorithm is simple
		/// 
		/// when we encounter an open bracket we push it into the stack to mark the beginning of a scope ()
		/// when we encounter a closing bracket we begin to pop the element from the stack until we
		///   finish the stack or we encounter a ( that marks the beginning of this scope.
		/// When we encounter an operator (+ - * =) we push on the stack but before we have to check the
		///   precedence, so we walk the stack, if we find a ( we stop because we cannot exit from current
		///	scope, but if we find an operator that has more precedence than this we simply pop the 
		///	element and append to the operation
		/// every other else is a simple token
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IList<D> InfixToPostfix(E expression) {

			Stack<S> stack = new Stack<S>();
			List<D> result = new List<D>();

			foreach (S token in tokenizer.Tokenize(expression)) {
				if (opChecker.IsBinaryOperator(token)) {
					//I found a binary operator, now I should see if there are some other operator 
					//with higer precedence
					while (
						stack.Count > 0 &&
						!opChecker.IsOpenBracket(stack.Peek()) &&
						opChecker.OperatorAHasMorePrecedenceThanB(stack.Peek(), token)) {
							result.Add(tokenConverter.Convert(stack.Pop()));
						}
					stack.Push(token);
				} else if (opChecker.IsUnaryOperator(token)) {
					stack.Push(token);
				}
				else if (opChecker.IsClosedBracket(token)) {
					while (stack.Count > 0 && 
					       !opChecker.IsOpenBracket(stack.Peek())) {
								 result.Add(tokenConverter.Convert(stack.Pop()));	
					       }
					stack.Pop();
				}
				else if (opChecker.IsOpenBracket( token)) {
					stack.Push(token);
				}
				else {
					result.Add(tokenConverter.Convert(token));
					////We found an element, apply all the unary operator find until now.
					while (stack.Count > 0 && opChecker.IsUnaryOperator(stack.Peek()))
						result.Add(tokenConverter.Convert(stack.Pop()));
				}
			}
			//This one is for operation not fully bracketed
			while (stack.Count > 0)
				result.Add(tokenConverter.Convert(stack.Pop()));
			return result;
		}
	}
}
