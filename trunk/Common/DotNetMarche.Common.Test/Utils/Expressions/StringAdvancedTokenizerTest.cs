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
			IOperatorsChecker<String> opChecker = new StandardOperatorsChecker();
			sut = new StringAdvancedTokenizer(opChecker);
		}

		#region Basic Test Suite

		[Test]  
		public void TestOnlyLiteral() 
		{
			CollectionAssert.AreEquivalent(new[] {"TEST"},sut.Tokenize("TEST") );
		}

		[Test] 
		public void TestSimpleAdd() 
		{
			CollectionAssert.AreEquivalent(new[] {"1", "+", "2"}, sut.Tokenize("1+2"));
		}

		[Test] 
		public void TestForMultiCharOperator() 
		{
			CollectionAssert.AreEquivalent(new[] { "A", "==", "B" }, sut.Tokenize("A==B"));
		}

		[Test]
		public void TestForSeparator()
		{
			CollectionAssert.AreEquivalent(new[] { "A", "B" }, sut.Tokenize("A B"));
		}

		[Test]
		public void TestForSeparatorMultichar()
		{
			CollectionAssert.AreEquivalent(new[] { "A", "B" }, sut.Tokenize("A  B"));
		}

		[Test]
		public void TestSimpleAddWithSpaces()
		{
			CollectionAssert.AreEquivalent(new[] { "1", "+", "2" }, sut.Tokenize("1 + 2"));
		}

		[Test]
		public void TestQuotedString()
		{
			CollectionAssert.AreEquivalent(new[] { "Test", "==", "'test space'" }, sut.Tokenize("Test == 'test space'"));
		}		
		
		[Test]
		public void TestStringOperators()
		{
			CollectionAssert.AreEquivalent(new[] { "Test", "like" , "'test space%'" }, sut.Tokenize("Test like 'test space%'"));
		}

		[Test]
		public void TestTwoQuotedString()
		{
			CollectionAssert.AreEquivalent(new[] { "Test", "+", "'test space'", "+", "'other space'" }, 
				sut.Tokenize("Test + 'test space' + 'other space'"));
		}

		[Test]
		public void TestQuotedStringDoubleQuotes()
		{
			CollectionAssert.AreEquivalent(new[] { "Test", "==", "'test sp'ace'" }, sut.Tokenize("Test == 'test sp''ace'"));
		}

		#endregion
	}
}
