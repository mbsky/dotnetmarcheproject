using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetMarche.Utils.Expressions;
using DotNetMarche.Utils.Expressions.Concrete;

namespace DotNetMarche.Utils.Linq
{
	public static class DynamicLinq
	{
		private static QueryToObjectOperatorsChecker opchecker = new QueryToObjectOperatorsChecker();
		private static StringTokenConverter tokenConverter = new StringTokenConverter();

		public static Expression<Func<T, U>> ParseToExpression<T, U>(String expression)
		{
			StringAdvancedTokenizer tokenizer = new StringAdvancedTokenizer(opchecker);
			ExpressionConverterExt<String, String, String> converter = 
				new ExpressionConverterExt<String, String, String>(
					opchecker, tokenizer, tokenConverter);
			IList<String> postfix = converter.InfixToPostfix(expression);
			PostfixExpressionToLambda<T> executor = new PostfixExpressionToLambda<T>();
			return executor.Execute<U>(postfix);
		}

		public static Func<T, U> ParseToFunction<T, U>(String expression)
		{
			return ParseToExpression<T, U>(expression).Compile();
		}
	}
}