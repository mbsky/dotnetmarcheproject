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
	}
}
