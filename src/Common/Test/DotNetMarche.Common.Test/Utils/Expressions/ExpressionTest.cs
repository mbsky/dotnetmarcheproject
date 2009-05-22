using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Utils.Expressions
{
	[TestFixture]
	public class ExpressionTest
	{

		//[Test]
		//public void TestStandardOr()
		//{
		//   Assert.That(4, MyIs.EqualTo(5).Or.EqualTo(4));
		//}

		[Test]
		public void TestStackEnumeration()
		{
			Stack<Int32> test = new Stack<int>();
			test.Push(1);
			test.Push(2);
			foreach (Int32 value in test)
			{
				Assert.AreEqual(2, value);
				return;
			}
		} 

		[Test]
		public void TestStackToArray()
		{
			Stack<Int32> test = new Stack<int>();
			test.Push(1);
			test.Push(2);
			Int32[] array = test.ToArray();
			Assert.AreEqual(2, array[0]);
		}
	}
}
