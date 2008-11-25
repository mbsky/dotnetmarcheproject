using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Utils.Expressions;
using DotNetMarche.Utils.Expressions.Concrete;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Utils.Expressions
{
	[TestFixture]
	public class StringAdvancedTokenizerTest
	{

		private StringAdvancedTokenizer sut;

		[TestFixtureSetUp]
		public void SetUp()
		{
			IOperatorsChecker<String> opChecker = new QueryToObjectOperatorsChecker();
			sut = new StringAdvancedTokenizer(opChecker);
		}

		#region Basic Test Suite

		[Test]  
		public void TestOnlyLiteral() 
		{
			CollectionAssert.AreEqual(new[] {"TEST"},sut.Tokenize("TEST") );
		}

		[Test] 
		public void TestSimpleAdd() 
		{
			CollectionAssert.AreEqual(new[] {"1", "+", "2"}, sut.Tokenize("1+2"));
		}

		[Test] 
		public void TestForMultiCharOperator() 
		{
			CollectionAssert.AreEqual(new[] { "A", "==", "B" }, sut.Tokenize("A==B"));
		}

		[Test]
		public void TestForSeparator()
		{
			CollectionAssert.AreEqual(new[] { "A", "B" }, sut.Tokenize("A B"));
		}

		[Test]
		public void TestForSeparatorMultichar()
		{
			CollectionAssert.AreEqual(new[] { "A", "B" }, sut.Tokenize("A  B"));
		}

		[Test]
		public void TestSimpleAddWithSpaces()
		{
			CollectionAssert.AreEqual(new[] { "1", "+", "2" }, sut.Tokenize("1 + 2"));
		}

        [Test]
        public void TestMemberAccessOperator()
        {
            CollectionAssert.AreEqual(new[] { "Custormer", ".", "Name" }, sut.Tokenize("Custormer.Name"));
        }
		[Test]
		public void TestQuotedString()
		{
			CollectionAssert.AreEqual(new[] { "Test", "==", "test space" }, sut.Tokenize("Test == 'test space'"));
		}		
		
		[Test]
		public void TestStringOperatorsAndOperatorShouldNotMatchInQuotes()
		{
			CollectionAssert.AreEqual(new[] { "Test", "like" , "test space%" }, sut.Tokenize("Test like 'test space%'"));
		}

		[Test]
		public void TestStringOperators()
		{
			CollectionAssert.AreEqual(new[] { "Test", "like", "test space" }, sut.Tokenize("Test like 'test space'"));
		}

		[Test]
		public void TestTwoQuotedString()
		{
			CollectionAssert.AreEqual(new[] { "Test", "+", "test space", "+", "other space" }, 
				sut.Tokenize("Test + 'test space' + 'other space'"));
		}

		[Test]
		public void TestQuotedStringDoubleQuotes()
		{
			CollectionAssert.AreEqual(new[] { "Test", "==", "test sp'ace" }, sut.Tokenize("Test == 'test sp''ace'"));
		}

		[Test]
		public void TestNotEqual()
		{
			CollectionAssert.AreEqual(new[] { "Test", "!=", "test space" }, sut.Tokenize("Test != 'test space'"));
		}

		#endregion

		#region Parenthesis

		[Test]
		public void TestBaseParenthesis()
		{
			CollectionAssert.AreEqual(new[] { "1", "+", "(", "2", "+", "3", ")" },
				sut.Tokenize("1 + ( 2 + 3 )"));
		}

		[Test]
		public void TestBaseParenthesis2()
		{
			CollectionAssert.AreEqual(new[] { "1", "+", "(", "2", "+", "3", ")" },
				sut.Tokenize("1+(2+3)"));
		}

		#endregion


		#region Parameters

		[Test]
		public void TestParameter()
		{
			CollectionAssert.AreEqual(new[] { "1", "+", ":param" },
				sut.Tokenize("1 + :param"));
		}

		[Test]
		public void TestParameterNoSpace()
		{
			CollectionAssert.AreEqual(new[] { "1", "+", ":param" },
				sut.Tokenize("1+:param"));
		}
		
		#endregion
	}
}
