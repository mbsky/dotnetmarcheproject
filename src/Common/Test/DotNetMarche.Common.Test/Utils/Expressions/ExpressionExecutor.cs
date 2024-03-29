﻿
using DotNetMarche.Utils.Expressions;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Utils.Expressions
{
	[TestFixture]
	public class ExpressionExecutor
	{

		private Conversion converter;
		private PostfixExecutor sut;
		[TestFixtureSetUp]
		public void SetUp()
		{
			converter = new Conversion();
			sut = new PostfixExecutor();
		}

		private double Execute(string expression)
		{
			return sut.Execute(converter.InfixToPostfix(expression));
		}

		[Test]
		public void TestSimpleMath1()
		{
			Assert.That(Execute("3 + 5 * 4"), Is.EqualTo(23));
		}
 
		[Test]
		public void TestSimpleMath2()
		{
			Assert.That(Execute("3 * 5 + 4 + 6 / 3"), Is.EqualTo(21));
		}

		[Test]
		public void TestSimpleMath3()
		{
			Assert.That(Execute("3 * ( 5 + 4 + 6 ) / 3"), Is.EqualTo(15));
		}

		[Test]
		public void TestSimpleMath4()
		{
			Assert.That(Execute("3 * (5 + 4 + 6) / 3"), Is.EqualTo(15));
		}
	}
}
