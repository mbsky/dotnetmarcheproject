using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DotNetMarche.Utils
{
	public static class LCGReflection
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
			DynamicMethod retmethod = new DynamicMethod(
				objType.FullName + methodName,
				typeof (T),
				new [] {typeof(Object)});
			ILGenerator ilgen = retmethod.GetILGenerator();
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Castclass, objType);
			ilgen.Emit(OpCodes.Callvirt, minfo);
			ilgen.Emit(OpCodes.Ret);
			return (Func<Object, T>) retmethod.CreateDelegate(typeof(Func<Object, T>));
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
				new [] { typeof(P1) },
				null);
			DynamicMethod retmethod = new DynamicMethod(
				objType.FullName + methodName,
				typeof(T),
				new[] { typeof(Object), typeof(P1) });
			ILGenerator ilgen = retmethod.GetILGenerator();
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Castclass, objType);
			ilgen.Emit(OpCodes.Ldarg_1);
			ilgen.Emit(OpCodes.Call, minfo);
			ilgen.Emit(OpCodes.Ret);
			return (Func<Object, P1, T>)retmethod.CreateDelegate(typeof(Func<Object, P1, T>));
		}
	}
}
