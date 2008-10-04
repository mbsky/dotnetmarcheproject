using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Utils;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Utils
{
	[TestFixture]
	public class ExpressionTreeReflectionTest
	{
		[Test]
		public void TestFuncNoArgInt32()
		{
			Type type = typeof (SimpleUnknown);
			Func<Object, Int32> func = ExpressionTreeReflection.Reflect<Int32>(type, "AMethod");
			SimpleUnknown su = new SimpleUnknown();
			Assert.That(func(su), Is.EqualTo(1));
		}		
		
		[Test]
		public void TestFuncNoArgString()
		{
			Type type = typeof (SimpleUnknown);
			Func<Object, String> func = ExpressionTreeReflection.Reflect<String>(type, "SMethod");
			SimpleUnknown su = new SimpleUnknown();
			Assert.That(func(su), Is.EqualTo("Hello"));
		}

		/// <summary>
		/// Verify performance gain with expression tree instead of reflection.
		/// </summary>
		[Test, Explicit]
		public void TestPerformanceGain()
		{
			Type type = typeof (SimpleUnknown);
			Func<Object, Int32> func = ExpressionTreeReflection.Reflect<Int32>(type, "AMethod");
			MethodInfo minfo = type.GetMethod("AMethod", BindingFlags.Public | BindingFlags.Instance);
			SimpleUnknown su = new SimpleUnknown();
			Double RefDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) minfo.Invoke(su, new Object[] {}); });
			Double ExpDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) func(su); });
			Console.WriteLine("Reflection = {0} Expression Tree {1}", RefDuration, ExpDuration);
		}		
		
		/// <summary>
		/// Verify performance gain with expression tree instead of reflection.
		/// </summary>
		[Test, Explicit]
		public void TestPerformanceGain2()
		{
			Type type = typeof (SimpleUnknown);
			Func<Object, String, Int32> func = ExpressionTreeReflection.Reflect<String, Int32>(type, "BMethod");
			MethodInfo minfo = type.GetMethod("BMethod", BindingFlags.Public | BindingFlags.Instance, null, new Type[] {typeof(String)}, null);
			SimpleUnknown su = new SimpleUnknown();
			Double RefDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) minfo.Invoke(su, new Object[] {"test"}); });
			Double ExpDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) func(su, "test"); });
			Console.WriteLine("Reflection = {0} Expression Tree {1}", RefDuration, ExpDuration);
		}

		[Test]
		public void TestFuncOneArgInt32()
		{
			Type type = typeof(SimpleUnknown);
			Func<Object, Int32, Int32> func = ExpressionTreeReflection.Reflect<Int32, Int32>(type, "BMethod");
			SimpleUnknown su = new SimpleUnknown();
			Assert.That(func(su, 4), Is.EqualTo(8));
		}		
		
		[Test]
		public void TestFuncOneArgInt32Overload()
		{
			Type type = typeof(SimpleUnknown);
			Func<Object, String, Int32> func = ExpressionTreeReflection.Reflect<String, Int32>(type, "BMethod");
			SimpleUnknown su = new SimpleUnknown();
			Assert.That(func(su, "test"), Is.EqualTo(8));
		}
	}
}
