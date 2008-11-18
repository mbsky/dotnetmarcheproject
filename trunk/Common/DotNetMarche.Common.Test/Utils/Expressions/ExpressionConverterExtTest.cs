using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using DotNetMarche.Utils.Expressions;
using DotNetMarche.Utils.Expressions.Concrete;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Utils.Expressions
{
	[TestFixture]
	public class ExpressionConverterExtTest
	{

		private ExpressionConverterExt<String, String, String> sut;

		[TestFixtureSetUp]
		public void SetUp()
		{
			ITokenConverter<String, String> converter = new StringTokenConverter();
			ITokenizer<String, String> tokenizer = new StringBasicTokenizer();
			IOperatorsChecker<String> opChecker = new StandardOperatorsChecker();
			sut = new ExpressionConverterExt<String, String, String>(opChecker, tokenizer, converter);


		}

		[Test]
		public void TestBasicAdd()
		{
			IList<String> postfix = sut.InfixToPostfix("A + B");
			CollectionAssert.AreEquivalent(postfix, new String[] {"A", "B", "+"});
		}

		[Test, Explicit("Still fail, need advanced tokenizer")]
		public void TestWithCustomerEntity()
		{
				IList<String> postfix = sut.InfixToPostfix("Name == 'Gian Maria'");
				CollectionAssert.AreEquivalent(postfix, new String[] { "Name", "'Gian Maria'", "==" });
		}

	}
}
