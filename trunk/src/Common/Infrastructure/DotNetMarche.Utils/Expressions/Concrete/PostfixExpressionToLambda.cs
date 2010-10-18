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
			binaryOpFactory = new Dictionary<String, Func<Expression, Expression, Expression>>();

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
			LambdaExpression lambda = GenerateExpressionFromPostfixList(postfixExpression);
			return (Expression<Func<T, RetType>>)lambda;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="P1">Type of the first parameter.</typeparam>
		/// <typeparam name="RetType"></typeparam>
		/// <param name="postfixExpression"></param>
		/// <returns></returns>
		public Expression<Func<T, P1, RetType>> Execute<P1, RetType>(IList<String> postfixExpression)
		{
			LambdaExpression lambda = GenerateExpressionFromPostfixList(postfixExpression);
			return (Expression<Func<T, P1, RetType>>)lambda;
		}


		/// <summary>
		/// This is the core function, it generates the Lambda.
		/// </summary>
		/// <param name="postfixExpression"></param>
		/// <returns></returns>
		private LambdaExpression GenerateExpressionFromPostfixList(IList<string> postfixExpression)
		{
			Stack<Expression> stack = new Stack<Expression>();
			Dictionary<String, ParameterExpression> parameters = new Dictionary<String, ParameterExpression>();
			List<ParameterExpression> parametersList = new List<ParameterExpression>();

			parametersList.Add(inputObj);
			Int32 i = 0;
			while (i < postfixExpression.Count)
			{
				String token = postfixExpression[i];
				//First of all check if is a name of a property of the object
				if (propertyNames.Any(p => p.Name == token))
				{
					stack.Push(Expression.Property(inputObj, token));
				}
				else if (IsMemberAccessOperator(token))
				{
					//Member access operator could advance the index. This because the syntax used to invoke a method
					//is not so good with postfix :) that because Name.Contains('xxx') becomes Name Contains . xxx because
					//parenthesis are used to precedence.
					ExecuteMemberAccessOperator(token, stack, postfixExpression, ref i);
				}
				else if (IsBinaryOperator(token))
				{
					ExecuteBinaryOperator(token, stack);
				}
				else if (IsUnaryOperator(token))
				{
					ExecuteUnaryOperator(token, stack);
				}
				else if (IsParameter(token))
				{
					ExecuteParameter(token, stack, parameters, parametersList);
				}
				else
				{
					stack.Push(Expression.Constant(token));
				}
				i++;
			}
			
				
			
			Expression final = stack.Pop();
			if (stack.Count > 0) throw new ArgumentException("The postfix expression is malformed");
			return Expression.Lambda(final, parametersList.ToArray());
		}

		private void ExecuteParameter(string token, Stack<Expression> stack, Dictionary<string, ParameterExpression> parameters, List<ParameterExpression> parametersList)
		{
			if (!parameters.ContainsKey(token))
			{
				ParameterExpression parameter = Expression.Parameter(typeof(Object), token);
				parameters.Add(token, parameter);
				parametersList.Add(parameter);
			}
			stack.Push(parameters[token]);
		}

		private void ExecuteUnaryOperator(string token, Stack<Expression> stack)
		{
			stack.Push(unaryOpFactory[token](stack.Pop()));
		}

		/// <summary>
		/// Executes the member access operator.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="stack">The stack.</param>
		/// <param name="tokens">List of current tokens, used to look in advance to the list</param>
		/// <param name="index">The current index in the <paramref name="tokens"/>, passed by ref
		/// because it could be changed.</param>
		/// <returns>The number of token that are read in advance</returns>
		private void ExecuteMemberAccessOperator(string token, Stack<Expression> stack, IList<string> tokens, ref Int32 index)
		{
			if (!IsMemberAccessOperator(token))
				throw new ArgumentException("The operator " + token + " is not supported");

			Expression op2 = stack.Pop();
			Expression op1 = stack.Pop();
			//we need to check if the dotted property is a property or operator
			string constValue = ((ConstantExpression)op2).Value.ToString();
			if (op1.Type.GetProperty(constValue) != null)
			{
				//is a property, create a property expression
				if ((op1 is MemberExpression) && (op2 is ConstantExpression))
				{
					stack.Push(Expression.Property(op1, constValue));
				}
			}
			else if (op1.Type.GetMethod(constValue) != null)
			{
				MethodInfo minfo = op1.Type.GetMethod(constValue);
				//now I need to add a memberoperator, but I need parameters.
				List<Expression> parameters = new List<Expression>();
				ParameterInfo[] parameterInfos = minfo.GetParameters();	
				for (int i = 0; i < parameterInfos.Length; i++)
				{
					String value = tokens[++index];
					parameters.Add(Expression.Constant(value));
				}
				stack.Push(Expression.Call(op1, minfo, parameters));
			}
			else
			{
				throw new NotSupportedException(constValue + " is not a property of type " + op1.Type.FullName);
			}

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
				}
				else
				{
					ConstantExpression cex1 = op1 as ConstantExpression;
					op1 = Expression.Constant(Convert.ChangeType(cex1.Value, op2.Type));
				}
			}
			stack.Push(binaryOpFactory[token](op1, op2));
		}

		private static Boolean IsMemberAccessOperator(string token)
		{
			return token == ".";
		}

		private static Boolean IsBinaryOperator(string token)
		{
			return binaryOpFactory.ContainsKey(token);
		}

		private static Boolean IsUnaryOperator(string token)
		{
			return unaryOpFactory.ContainsKey(token);
		}

		private static Boolean IsParameter(string token)
		{
			return token.StartsWith(":");
		}
	}
}
