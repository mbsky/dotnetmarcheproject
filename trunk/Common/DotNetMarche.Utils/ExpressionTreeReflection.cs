using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Utils
{
	/// <summary>
	/// If you want to avoid performance penalities in invoking function through
	/// reflection this can be a solution.
	/// </summary>
	public static class ExpressionTreeReflection
	{
		/// <summary>
		/// Reflect to call a method on a object that accept np parameters and return type T
		/// </summary>
		/// <typeparam name="T">The return type of the method to invoke</typeparam>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Func<Object, T> ReflectFunction<T>(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				null,
				CallingConventions.Any,
				new Type[] { },
				null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo);
			LambdaExpression lambda = Expression.Lambda(invoke, param);
			Expression<Func<Object, T>> dynamicSetterExpression = (Expression<Func<Object, T>>)lambda;
			return dynamicSetterExpression.Compile();
		}


		/// <summary>
		/// Reflect to call a method on a object that accept one parameter and return type T
		/// </summary>
		/// <typeparam name="T">The return type of the method to invoke</typeparam>
		/// <typeparam name="P1">The type of the first parameter accepted by the method</typeparam>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Func<Object, P1, T> ReflectFunction<P1, T>(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				null,
				CallingConventions.Any,
				new Type[] { typeof(P1) },
				null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			ParameterExpression paramP1 = Expression.Parameter(typeof(P1), "paramP1");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo, paramP1);
			LambdaExpression lambda = Expression.Lambda(invoke, param, paramP1);
			Expression<Func<Object, P1, T>> dynamicSetterExpression = (Expression<Func<Object, P1, T>>)lambda;
			return dynamicSetterExpression.Compile();
		}

		/// <summary>
		/// Reflect to call a method on a object that accept two parameters and return type T
		/// </summary>
		/// <typeparam name="T">The return type of the method to invoke</typeparam>
		/// <typeparam name="P1">The type of the first parameter accepted by the method</typeparam>
		/// <typeparam name="P2">The type of the second parameter accepted by the method</typeparam>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Func<Object, P1, P2, T> ReflectFunction<P1, P2, T>(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				null,
				CallingConventions.Any,
				new Type[] { typeof(P1), typeof(P2) },
				null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			ParameterExpression paramP1 = Expression.Parameter(typeof(P1), "paramP1");
			ParameterExpression paramP2 = Expression.Parameter(typeof(P2), "paramP2");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo, paramP1, paramP2);
			LambdaExpression lambda = Expression.Lambda(invoke, param, paramP1, paramP2);
			Expression<Func<Object, P1, P2, T>> dynamicSetterExpression = (Expression<Func<Object, P1, P2, T>>)lambda;
			return dynamicSetterExpression.Compile();
		}

		/// <summary>
		/// Reflect to call a method on a object that accept three parameters and return type T
		/// </summary>
		/// <typeparam name="T">The return type of the method to invoke</typeparam>
		/// <typeparam name="P1">The type of the first parameter accepted by the method</typeparam>
		/// <typeparam name="P2">The type of the second parameter accepted by the method</typeparam>
		/// <typeparam name="P3">The type of the third parameter accepted by the method</typeparam>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Func<Object, P1, P2, P3, T> ReflectFunction<P1, P2, P3, T>(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				null,
				CallingConventions.Any,
				new Type[] { typeof(P1), typeof(P2) },
				null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			ParameterExpression paramP1 = Expression.Parameter(typeof(P1), "paramP1");
			ParameterExpression paramP2 = Expression.Parameter(typeof(P2), "paramP2");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo, paramP1, paramP2);
			LambdaExpression lambda = Expression.Lambda(invoke, param, paramP1, paramP2);
			Expression<Func<Object, P1, P2, P3, T>> dynamicSetterExpression = (Expression<Func<Object, P1, P2, P3, T>>)lambda;
			return dynamicSetterExpression.Compile();
		}

		/// <summary>
		/// Reflect to call a method on a object that accept no parameters and has no return Type
		/// </summary>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Action<Object> ReflectAction(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
	methodName,
	BindingFlags.Instance | BindingFlags.Public,
	null,
	CallingConventions.Any,
	new Type[] { },
	null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo);
			LambdaExpression lambda = Expression.Lambda(invoke, param);
			Expression<Action<Object>> dynamicSetterExpression = (Expression<Action<Object>>)lambda;
			return dynamicSetterExpression.Compile();
		}

		/// <summary>
		/// Reflect to call a method on a object that accepts one and has no return Type
		/// </summary>
		/// <param name="objType">The type of the object we want to reflect</param>
		/// <param name="methodName">The name of the method we want to call</param>
		/// <typeparam name="P1">Type of the first parameter of the method to invoke.</typeparam>
		/// <returns>A function object that you can use to invoke the method.</returns>
		public static Action<Object, P1> ReflectAction<P1>(Type objType, String methodName)
		{
			MethodInfo minfo = objType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				null,
				CallingConventions.Any,
				new Type[] { typeof(P1) },
				null);
			ParameterExpression param = Expression.Parameter(typeof(Object), "object");
			ParameterExpression paramP1 = Expression.Parameter(typeof(P1), "paramP1");
			Expression convertedParamo = Expression.Convert(param, objType);
			Expression invoke = Expression.Call(convertedParamo, minfo, paramP1);
			LambdaExpression lambda = Expression.Lambda(invoke, param, paramP1);
			Expression<Action<Object, P1>> dynamicSetterExpression = (Expression<Action<Object, P1>>)lambda;
			return dynamicSetterExpression.Compile();
		}
	}
}
