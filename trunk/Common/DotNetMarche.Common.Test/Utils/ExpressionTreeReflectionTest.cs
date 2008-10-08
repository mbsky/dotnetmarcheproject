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
		private static readonly Type suType = Type.GetType("DotNetMarche.Common.Test.AuxClasses.SimpleUnknown, DotNetMarche.Common.Test");
		private static readonly Object suInstance = Activator.CreateInstance(Type.GetType("DotNetMarche.Common.Test.AuxClasses.SimpleUnknown, DotNetMarche.Common.Test"));



		[Test]
		public void TestFuncNoArgInt32()
		{
			Func<Object, Int32> func = ExpressionTreeReflection.ReflectFunction<Int32>(suType, "AMethod");
			Assert.That(func(suInstance), Is.EqualTo(1));
		}

		[Test]
		public void LCGFuncNoArgInt32()
		{
			Func<Object, Int32> func = LCGReflection.ReflectFunction<Int32>(suType, "AMethod");
			Assert.That(func(suInstance), Is.EqualTo(1));
		}

		[Test]
		public void TestDoubleCallFuncNoArgInt32()
		{
			Func<Object, Int32> func = ExpressionTreeReflection.ReflectFunction<Int32>(suType, "AMethod");
			func = ExpressionTreeReflection.ReflectFunction<Int32>(suType, "AMethod");
			Assert.That(func(suInstance), Is.EqualTo(1));
		}

		[Test]
		public void LCGDoubleCallFuncNoArgInt32()
		{
			Func<Object, Int32> func = ExpressionTreeReflection.ReflectFunction<Int32>(suType, "AMethod");
			func = LCGReflection.ReflectFunction<Int32>(suType, "AMethod");
			Assert.That(func(suInstance), Is.EqualTo(1));
		}

		[Test]
		public void TestFuncNoArgString()
		{
			Func<Object, String> func = ExpressionTreeReflection.ReflectFunction<String>(suType, "SMethod");
			Assert.That(func(suInstance), Is.EqualTo("Hello"));
		}

		/// <summary>
		/// Verify performance gain with expression tree instead of reflection.
		/// </summary>
		[Test, Explicit]
		public void TestPerformanceGain()
		{
			Func<Object, Int32> func = ExpressionTreeReflection.ReflectFunction<Int32>(suType, "AMethod");
			Func<Object, Int32> lcgfunc = LCGReflection.ReflectFunction<Int32>(suType, "AMethod");
			MethodInfo minfo = suType.GetMethod("AMethod", BindingFlags.Public | BindingFlags.Instance);
			Double RefDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) minfo.Invoke(suInstance, new Object[] { }); });
			Double ExpDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) func(suInstance); });
			Double LcgDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) lcgfunc(suInstance); });
			Console.WriteLine("Reflection = {0} Expression Tree {1} LCG {2}", RefDuration, ExpDuration, LcgDuration);
		}

		/// <summary>
		/// Verify performance gain with expression tree instead of reflection.
		/// </summary>
		[Test, Explicit]
		public void TestPerformanceGain2()
		{
			Func<Object, String, Int32> func = ExpressionTreeReflection.ReflectFunction<String, Int32>(suType, "BMethod");
			MethodInfo minfo = suType.GetMethod("BMethod", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(String) }, null);
			Double RefDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) minfo.Invoke(suInstance, new Object[] { "test" }); });
			Double ExpDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) func(suInstance, "test"); });
			Console.WriteLine("Reflection = {0} Expression Tree {1}", RefDuration, ExpDuration);
		}

		[Test]
		public void TestFuncOneArgInt32()
		{
			Func<Object, Int32, Int32> func = ExpressionTreeReflection.ReflectFunction<Int32, Int32>(suType, "BMethod");
			Assert.That(func(suInstance, 4), Is.EqualTo(8));
		}

		[Test]
		public void LcgTestFuncOneArgInt32()
		{
			Func<Object, Int32, Int32> func = LCGReflection.ReflectFunction<Int32, Int32>(suType, "BMethod");
			Assert.That(func(suInstance, 4), Is.EqualTo(8));
		}

		[Test]
		public void TestFuncOneArgInt32Overload()
		{
			Func<Object, String, Int32> func = ExpressionTreeReflection.ReflectFunction<String, Int32>(suType, "BMethod");
			Assert.That(func(suInstance, "test"), Is.EqualTo(4));
		}		
		
		[Test]
		public void LcgTestFuncOneArgInt32Overload()
		{
			Func<Object, String, Int32> func = LCGReflection.ReflectFunction<String, Int32>(suType, "BMethod");
			Assert.That(func(suInstance, "test0"), Is.EqualTo(5));
		}

		[Test]
		public void TestActionNoParams()
		{
			Action<Object> func = ExpressionTreeReflection.ReflectAction(suType, "VMethod");
			SimpleUnknown su = new SimpleUnknown();
			func(su);
			Assert.That(su.Val, Is.EqualTo(10));
		}

		[Test]
		public void TestActionOneParamInt32()
		{
			Action<Object, String> func = ExpressionTreeReflection.ReflectAction<String>(suType, "VMethod");
			SimpleUnknown su = new SimpleUnknown();
			func(su, "test");
			Assert.That(su.Val, Is.EqualTo(4));
		}

		[Test]
		public void TestReflectConstructors()
		{
			Func<Object> func = ExpressionTreeReflection.ReflectConstructor(suType);
			Object su = func();
			Assert.That(su, Is.InstanceOfType(typeof(SimpleUnknown)));
		}

		/// <summary>
		/// Verify performance gain with expression tree instead of reflection.
		/// </summary>
		[Test, Explicit]
		public void TestPerformanceGainConstructors()
		{
			Func<Object> func = ExpressionTreeReflection.ReflectConstructor(suType);
			ConstructorInfo cinfo = suType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { }, null);
			Double RefDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) cinfo.Invoke(new Object[] { }); });
			Double ExpDuration = With.PerformanceCounter(() => { for (Int32 I = 0; I < 100000; ++I) func(); });
			Console.WriteLine("Reflection = {0} Expression Tree {1}", RefDuration, ExpDuration);
		}

	}
}
