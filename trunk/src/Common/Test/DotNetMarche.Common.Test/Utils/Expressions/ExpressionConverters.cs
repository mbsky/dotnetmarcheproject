using System;
using DotNetMarche.Utils.Expressions;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Utils.Expressions
{
	[TestFixture]
	public class ExpressionConverters
	{

		private Conversion sut;

		[TestFixtureSetUp]
		public void SetUp()
		{
			sut = new Conversion();
		}

		#region Standard Converter

		[Test]
		public void TestSimpleConversion1()
		{
			String Expression = "( a + b )";
			Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b +"));
		}

        [Test]
        public void TestBasicMemberAccessConversion()
        {
            String Expression = "( a.b )";
            Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b ."));
        }

        [Test]
        public void TestComplexMemberAccessConversion()
        {
            String Expression = "( a.b == c.d.e )";
            Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b . c d e . . =="));
        }

		[Test]
		public void TestSimpleconversion2()
		{
			String Expression = "( ( a + b ) + c )";
			Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b + c +"));
		}

		[Test]
		public void TestSimpleconversion3()
		{
			String Expression = "( a + ( b + c ) )";
			Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b c + +"));
		}

		[Test]
		public void TestNotFullyBracket1()
		{
			Assert.That(sut.InfixToPostfix("a + b"), Is.EqualTo("a b +"));
		}


		[Test]
		public void TestPrecedenceFullyParentesized1()
		{
			String Expression = "( a + ( b * c ) )";
			Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b c * +"));
		}

		[Test]
		public void TestComplex()
		{

			Assert.That(sut.InfixToPostfix(
				"( ( ( a + b ) * ( e - f ) ) + g )"),
				Is.EqualTo("a b + e f - * g +"));
		}

		[Test]
		public void TestPrecedenceOperatorA()
		{
			String Expression = " a * b + c";
			Assert.That(sut.InfixToPostfix(Expression), Is.EqualTo("a b * c +"));
		}

		[Test]
		public void TestPrecedenceOperatorB()
		{
			Assert.That(sut.InfixToPostfix("a + b * c"), Is.EqualTo("a b c * +"));
		}

		[Test]
		public void TestComplexNotFullyParentesizedA()
		{
			Assert.That(sut.InfixToPostfix("a * b + c * d + r"), Is.EqualTo("a b * c d * r + +"));
		}

		[Test]
		public void TestComplexNotFullyParentesizedB()
		{
			Assert.That(sut.InfixToPostfix("a + b * c + d * r"), Is.EqualTo("a b c * d r * + +"));
		}

		[Test]
		public void TestComplexNotFullyParentesizedC()
		{
			Assert.That(sut.InfixToPostfix("a * b + c + d / e"), Is.EqualTo("a b * c d e / + +"));
			//"3 * 5 + 4 + 6 / 3"
		}


		/// <summary>
		/// Code @ is treated as a unary operator
		/// </summary>
		[Test]
		public void TestPrefixUnaryOperator()
		{
			Assert.That(sut.InfixToPostfix("a * @ b"), Is.EqualTo("a b @ *"));
		}

		[Test]
		public void TestPrefixUnaryTwo()
		{
			Assert.That(sut.InfixToPostfix("a * ( @ b + c )"), Is.EqualTo("a b @ c + *"));
		}

		[Test]
		public void TestPrefixUnaryWithPrecedence()
		{
			Assert.That(sut.InfixToPostfix("a + @ b * c"), Is.EqualTo("a b @ c * +"));
		}

		/// <summary>
		/// Test unary operator used with the result of an parentesized operation
		/// </summary>
		[Test]
		public void TestPrefixBracket()
		{
			Assert.That(sut.InfixToPostfix("a + @ ( b * c )"), Is.EqualTo("a b c * @ +"));
		}

		[Test]
		public void TestLikeOperator()
		{
			Assert.That(sut.InfixToPostfix("a like b"), Is.EqualTo("a b like"));
		}

		[Test]
		public void Test()
		{

		}

		#endregion
	}
}
