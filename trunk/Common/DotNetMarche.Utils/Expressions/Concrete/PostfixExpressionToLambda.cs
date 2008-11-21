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
		private static String[] unaryoperators;

		private static Dictionary<String, Func<Expression, Expression, Expression>> 
			binaryOpFactory = new Dictionary<String, Func<Expression, Expression, Expression>> ();

		private static Dictionary<String, Func<Expression, Expression>>
			unaryOpFactory = new Dictionary<String, Func<Expression, Expression>>();

		static PostfixExpressionToLambda()
		{
			Type t = typeof(T);
			propertyNames = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			unaryoperators = new String[] { };
			binaryOpFactory.Add("==", Expression.Equal);
			binaryOpFactory.Add(">", Expression.GreaterThan);
			binaryOpFactory.Add("<", Expression.LessThan);
			binaryOpFactory.Add(">=", Expression.GreaterThanOrEqual);
			binaryOpFactory.Add("<=", Expression.LessThanOrEqual);
			binaryOpFactory.Add("!=", Expression.NotEqual);
			binaryOpFactory.Add("&&", Expression.And);
			binaryOpFactory.Add("||", Expression.Or);

			unaryOpFactory.Add("!", Expression.Not);
		}

		private readonly ParameterExpression inputObj;

		public PostfixExpressionToLambda()
		{
			inputObj = Expression.Parameter(typeof(T), "object");
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
			Expression tempexpression;
			foreach (String token in postfixExpression)
			{
				//First of all check if is a name of a property of the object
				if (propertyNames.Any(p => p.Name == token))
				{
					stack.Push(Expression.Property(inputObj, token));
				}
				else if (IsBinaryOperator(token))
				{
					ExecuteBinaryOperator(token, stack);
				}
				else if (IsUnaryOperator(token))
				{
					ExecuteUnaryOperator(token, stack);
				}
				else
				{
					stack.Push(Expression.Constant(token));
				}
			}
			Expression final = stack.Pop();
			if (stack.Count > 0) throw new ArgumentException("The postfix expression is malformed");
			LambdaExpression lambda = Expression.Lambda(final, inputObj);
			return (Expression<Func<T, RetType>>)lambda;
		}

		private void ExecuteUnaryOperator(string token, Stack<Expression> stack)
		{
			stack.Push(unaryOpFactory[token](stack.Pop()));
		}

		private void ExecuteBinaryOperator(string token, Stack<Expression> stack)
		{
			if (!binaryOpFactory.ContainsKey(token))					
				throw new ArgumentException("The operator " + token + " is not supported");
			Expression op2 = stack.Pop();
			Expression op1 = stack.Pop();
			if (op2.Type != op1.Type && (op2 is ConstantExpression || op1 is ConstantExpression))
			{
				if (op2 is ConstantExpression)
				{
					ConstantExpression cex2 = op2 as ConstantExpression;
					op2 = Expression.Constant(Convert.ChangeType(cex2.Value, op1.Type));
				} else
				{
						ConstantExpression cex1 = op1 as ConstantExpression;
					op1 = Expression.Constant(Convert.ChangeType(cex1.Value, op2.Type));
				}
			}
			stack.Push(binaryOpFactory[token](op1, op2));
		}

		private static Boolean IsBinaryOperator(string token)
		{
			return binaryOpFactory.ContainsKey(token);
		}

		private static Boolean IsUnaryOperator(string token)
		{
			return unaryOpFactory.ContainsKey(token);
		}
	}
}
