using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest;
using Nablasoft.Test.UnitTest;
using NUnit.Framework;


namespace DotNetMarche.Common.Test.TestHelpers.Fluent
{
	[TestFixture]
	public class BracketTest {

		[Test]
		public void TestBasic1() {
			Assert.That(3, MyIs.EqualTo(2).Or['('].EqualTo(3).And.LessThan(5)[')']);
		}

		[Test]
		public void TestBasic2() {
			Assert.That(3, MyIs.EqualTo(3).And['('].EqualTo(2).Or.LessThan(5)[')']);
		}
      
		[Test, Explicit]
		public void TestBasicOperator()
		{
			Assert.That(3, MyIs.EqualTo(2) & (MyIs.EqualTo(3) | MyIs.LessThan(5)));
		}

		[Test, Explicit]
		public void TestBasicOperatorn()
		{
			Assert.That(3, Is.EqualTo(2));
		}
		[Test]
		public void TestBasicOperator2()
		{
			Assert.That(3, (MyIs.EqualTo(3) & MyIs.EqualTo(6)) | MyIs.LessThan(5));
		}

		/// <summary>
		/// This test will fail, we are interested in error messages
		/// </summary>
		[Test, Explicit]
		public void TestChainWithAndDescription() {
			List<AnotherEntity> al = CreateTestListOne();

			Assert.That(al, MyHas.None.Property("PropertyB", 1).Or
			                	['(']
			                	.One.Property("PropertyB", 1)
			                	.And
			                	.Count(3).Property("PropertyB", 2)
			                	[')']);
		}



		private static List<AnotherEntity> CreateTestListOne() {
			List<AnotherEntity> al = new List<AnotherEntity>();
			al.Add(new AnotherEntity("A", 1));
			al.Add(new AnotherEntity("q", 2));
			al.Add(new AnotherEntity("E", 2));

			return al;
		}

	}
}