using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetMarche.TestHelpers.Reflection
{
	public static class Invoker
	{
		/// <summary>
		/// Invoke a function through reflection, used for testing.
		/// </summary>
		/// <typeparam name="Tret"></typeparam>
		/// <param name="func"></param>
		/// <param name="obj"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Tret InvokeFunc<Tret>(Func<Tret> func, Object obj, params Object[] parameters)
		{
			MethodInfo mi = Static.For(func);
			return (Tret) mi.Invoke(obj, parameters);
		}

		/// <summary>
		/// Invoke a function through reflection, used for testing.
		/// </summary>
		/// <typeparam name="Tret"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <param name="func"></param>
		/// <param name="obj"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Tret InvokeFunc<Tret, T1>(Func<Tret, T1> func, Object obj, params Object[] parameters)
		{
			MethodInfo mi = Static.For(func);
			return (Tret)mi.Invoke(obj, parameters);
		}

		/// <summary>
		/// Invoke a function through reflection, used for testing.
		/// </summary>
		/// <typeparam name="Tret"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="func"></param>
		/// <param name="obj"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Tret InvokeFunc<Tret, T1, T2>(Func<Tret, T1, T2> func, Object obj, params Object[] parameters)
		{
			MethodInfo mi = Static.For(func);
			return (Tret)mi.Invoke(obj, parameters);
		}

		/// <summary>
		/// Invoke a function through reflection, used for testing.
		/// </summary>
		/// <typeparam name="Tret"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="T3"></typeparam>
		/// <param name="func"></param>
		/// <param name="obj"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Tret InvokeFunc<Tret, T1, T2, T3>(Func<Tret, T1, T2, T3> func, Object obj, params Object[] parameters)
		{
			MethodInfo mi = Static.For(func);
			return (Tret)mi.Invoke(obj, parameters);
		}

		/// <summary>
		/// Used to invoke private instance methods.
		/// </summary>
		/// <typeparam name="Tret">Return type of the method.</typeparam>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="obj">The object we want to invoke the method for.</param>
		/// <param name="parameters">Parameters of the method</param>
		/// <returns>Return value of the method.</returns>
		public static Tret InvokePrivate<Tret>(String methodName, Object obj, params Object[] parameters)
		{
			MethodInfo mi = obj.GetType().GetMethod(methodName,
			                                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			return (Tret)mi.Invoke(obj, parameters);
		}

		/// <summary>
		/// Used to invoke private methods for static function, here you should specify the type of the object.
		/// </summary>
		/// <typeparam name="Tret">Return type of the method</typeparam>
		/// <param name="objectType">The type of the object that has the static method we want to call</param>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameters">Parameters needed by the method.</param>
		/// <returns></returns>
		public static Tret InvokePrivate<Tret>(Type objectType, String methodName, params Object[] parameters)
		{
			MethodInfo mi = objectType.GetMethod(methodName,
																 BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			return (Tret)mi.Invoke(null, parameters);
		}
	}
}
