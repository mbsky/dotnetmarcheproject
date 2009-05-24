using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetMarche.TestHelpers.Reflection
{
	/// <summary>
	/// See Ayende: http://ayende.com/Blog/2005/10/29/StaticReflection.aspx for an
	/// explanation of this class.
	/// </summary>
	public static class Static
	{
		public static MethodInfo For(Action action)
		{
			return action.Method;
		}

		public static MethodInfo For<T1>(Action<T1> action)
		{
			return action.Method;
		}

		public static MethodInfo For<T1, T2>(Action<T1, T2> action)
		{
			return action.Method;
		}

		public static MethodInfo For<T1, T2, T3>(Action<T1, T2, T3> action)
		{
			return action.Method;
		}

		public static MethodInfo For<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
		{
			return action.Method;
		}

		public static MethodInfo For<Tret>(Func<Tret> action)
		{
			return action.Method;
		}

		public static MethodInfo For<Tret, T1>(Func<T1, Tret> action)
		{
			return action.Method;
		}

		public static MethodInfo For<Tret, T1, T2>(Func<T1, T2, Tret> action)
		{
			return action.Method;
		}

		public static MethodInfo For<Tret, T1, T2, T3>(Func<T1, T2, T3, Tret> action)
		{
			return action.Method;
		}
	}
}
