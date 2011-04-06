using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetMarche.Validator.Utils
{
	public static class LambdaHelper
	{
		public static String GetMemberName<T, TProperty>(this Expression<Func<T, TProperty>> source)
		{
			var expression = source.Body as MemberExpression;

			if (expression == null)
			{
				UnaryExpression unex = source.Body as UnaryExpression;
				if (unex != null)
				{
					expression = unex.Operand as MemberExpression;
				}
			}
			if (expression != null)
			{
				var member = expression.Member;
				return member.Name;
			}

			return null;
		}
	}
}
