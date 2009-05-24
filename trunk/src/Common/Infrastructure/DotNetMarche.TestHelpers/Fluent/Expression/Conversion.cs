using System;
using System.Collections.Generic;
using System.Text;
using Nablasoft.Test.UnitTest.Expression;

namespace DotNetMarche.TestHelpers.Fluent.Expression
{
	//public class Conversion {

	//   private IOperatorsChecker<String> opChecker = new StandardOperatorsChecker();

	//   /// <summary>
	//   /// Convert an infix to postfix, all token should be separated by a space
	//   /// to make the parsing simple. The algorithm is simple
	//   /// 
	//   /// when we encounter an open bracket we push it into the stack to mark the beginning of a scope ()
	//   /// when we encounter a closing bracket we begin to pop the element from the stack until we
	//   ///   finish the stack or we encounter a ( that marks the beginning of this scope.
	//   /// When we encounter an operator (+ - * =) we push on the stack but before we have to check the
	//   ///   precedence, so we walk the stack, if we find a ( we stop because we cannot exit from current
	//   ///	scope, but if we find an operator that has more precedence than this we simply pop the 
	//   ///	element and append to the operation
	//   /// every other else is a simple token
	//   /// </summary>
	//   /// <param name="expression"></param>
	//   /// <returns></returns>
	//   public String InfixToPostfix(String expression) {
	//      String[] tokens = expression.Split(' ');
	//      StringBuilder result = new StringBuilder();
	//      Stack<String> stack = new Stack<String>();
	//      foreach (String token in tokens) {
	//         if (opChecker.IsBinaryOperator(token)) {
	//            while (
	//               stack.Count > 0 &&
	//               stack.Peek() != "(" &&
	//               opChecker.OperatorAHasMorePrecedenceThanB(stack.Peek(), token))
	//               result.Append(stack.Pop() + ' ');
	//            stack.Push(token);
	//         }
	//         else if (opChecker.IsUnaryOperator(token)) {
	//            stack.Push(token);
	//         }
	//         else if (token == ")") {
	//            while (stack.Count > 0 && stack.Peek() != "(")
	//               result.Append(stack.Pop() + ' ');
	//            stack.Pop();
	//         }
	//         else if (token == "(") {
	//            stack.Push(token);
	//         }
	//         else {
	//            result.Append(token + ' ');
	//            while (stack.Count > 0 && opChecker.IsUnaryOperator(stack.Peek()))
	//               result.Append(stack.Pop() + ' ');
	//         }
	//      }
	//      //This one is for operation not fully bracketed
	//      while (stack.Count > 0)
	//         result.Append(stack.Pop() + ' ');
	//      return result.ToString().Trim();
	//   }
	//}
}