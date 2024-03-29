using System;
using System.Collections.Generic;
using System.Text;
using Nablasoft.Test.UnitTest;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent
{
	[TestFixture]
	public class ExpressionTest {

		[Test]
		public void TestStandardOr() {
            Assert.That(4, MyIs.EqualTo(5).Or.EqualTo(4));
		}

		[Test]
		public void TestStackEnumeration() {
			Stack<Int32> test = new Stack<int>();
			test.Push(1);
			test.Push(2);
			foreach(Int32 value in test) {
				Assert.AreEqual(2, value);
				return;
			}
		}

		[Test]
		public void TestStackToArray() {
			Stack<Int32> test = new Stack<int>();
			test.Push(1);
			test.Push(2);
			Int32[] array = test.ToArray();
			Assert.AreEqual(2, array[0]);
		}
	}
}