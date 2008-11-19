using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	/// <summary>
	/// Convert an expression in postfix form combining all expression in 
	/// an expression tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PostfixExpressionToLambda<T>
	{
		private static PropertyInfo[] propertyNames;

		static PostfixExpressionToLambda()
		{
			Type t = typeof(T);
			propertyNames = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}

		private readonly ParameterExpression inputObj;

		public PostfixExpressionToLambda()
		{
			inputObj = Expression.Parameter(typeof (T), "object");
		}

		/// <summary>
		/// This is the main function, it is based on the fact that the postfix expression
		/// passed as argument is a representatino of a function that accepts a parameter
		/// T as input and return some type based on the expression.
		/// The inputObj is a parameter expression created in constructor to represent the
		/// parameter of the whole lambda
		/// </summary>
		/// <typeparam name="RetType"></typeparam>
		/// <param name="postfixExpression"></param>
		/// <returns></returns>
		public Expression<Func<T, RetType>> Execute<RetType>(IList<String> postfixExpression)
		{
			Stack<Expression> stack = new Stack<Expression>();
			foreach (String token in postfixExpression)
			{
				//First of all check if is a name of a property of the object
				if (propertyNames.Any(p => p.Name == token))
				{
					stack.Push(Expression.Property(inputObj, token));
				} else
				{
					stack.Push(Expression.Constant(token));
				}

				//if (opChecker.IsBinaryOperator(token))
				//{
				//   Double op2 = stack.Pop();
				//   Double op1 = stack.Pop();
				//   switch (token)
				//   {
				//      case "*":
				//         stack.Push(op1 * op2);
				//         break;
				//      case "-":
				//         stack.Push(op1 - op2);
				//         break;
				//      case "+":
				//         stack.Push(op1 + op2);
				//         break;
				//      case "/":
				//         stack.Push(op1 / op2);
				//         break;
				//   }
				//}
				//else
				//{
				//   stack.Push(Double.Parse(token));
				//}
			}
			Expression final = stack.Pop();
			if (stack.Count > 0) throw new ArgumentException("The postfix expression is malformed");
			LambdaExpression lambda = Expression.Lambda(final, inputObj);
			return (Expression<Func<T, RetType>>)lambda;
		}
	}
}
