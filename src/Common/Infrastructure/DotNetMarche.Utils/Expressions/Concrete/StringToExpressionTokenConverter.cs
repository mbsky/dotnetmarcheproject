using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	/// <summary>
	/// This class is not so useful because .net expression tree should be constructed
	/// directly from the postfix form
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StringToExpressionTokenConverter<T> : ITokenConverter<String, Expression>
	{
		private static PropertyInfo[] propertyNames;

		static StringToExpressionTokenConverter()
		{
			Type t = typeof(T);
			propertyNames = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}

		private readonly ParameterExpression inputObj;
		internal ParameterExpression InputParameter
		{
			get { return inputObj;}
		}

		public StringToExpressionTokenConverter()
		{
			inputObj = Expression.Parameter(typeof (T), "object");
		}

		#region ITokenConverter<string,Expression> Members

		/// <summary>
		/// It is a complex method, we need to convert a string in some expression part
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Expression Convert(string source)
		{
			//First of all check if is a name of a property of the object
			if (propertyNames.Any(p => p.Name == source))
			{
				return Expression.Property(inputObj, source);
			}
			//Is a constant of some type.
			return Expression.Constant(source);
		}

		#endregion
	}
}
