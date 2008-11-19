using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetMarche.Common.Test.Utils.Expressions.AuxClasses;
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

		/// <summary>
		/// The sut that converts string to expression.
		/// </summary>
		private ExpressionConverterExt<String, String, Expression> sutEx;

		/// <summary>
		/// 
		/// </summary>
		private PostfixExpressionToLambda<Customer> sutpfex = new PostfixExpressionToLambda<Customer>();

		/// <summary>
		/// The converter from string to expression
		/// </summary>
		private ITokenConverter<string, Expression> sutConvEx;

		[TestFixtureSetUp]
		public void SetUp()
		{
			ITokenConverter<String, String> converter = new StringTokenConverter();
			ITokenizer<String, String> tokenizer = new StringBasicTokenizer();
			IOperatorsChecker<String> opChecker = new StandardOperatorsChecker();
			sut = new ExpressionConverterExt<String, String, String>(opChecker, tokenizer, converter);

			QueryToObjectOperatorsChecker opCheckerex = new QueryToObjectOperatorsChecker();
			sutConvEx = new StringToExpressionTokenConverter<Customer>();
			ITokenizer<String, String> tokenizerex = new StringAdvancedTokenizer(opCheckerex);

			sutEx = new ExpressionConverterExt<String, String, Expression>(opCheckerex, tokenizerex, sutConvEx);
		}

		[Test]
		public void TestBasicAdd()
		{
			IList<String> postfix = sut.InfixToPostfix("A + B");
			CollectionAssert.AreEquivalent(postfix, new String[] { "A", "B", "+" });
		}

		[Test, Explicit("Still fail, need advanced tokenizer")]
		public void TestWithCustomerEntity()
		{
			IList<String> postfix = sut.InfixToPostfix("Name == 'Gian Maria'");
			CollectionAssert.AreEquivalent(postfix, new String[] { "Name", "'Gian Maria'", "==" });
		}

		#region StringToExpressionConverterTest

		[Test]
		public void BasicConstantConversion()
		{
			Expression exp = sutConvEx.Convert("1");
			Assert.That(exp, Is.TypeOf(typeof (ConstantExpression)));
		}

		[Test]
		public void BasicConstantConversionInteger()
		{
			ConstantExpression exp = sutConvEx.Convert("1") as ConstantExpression;
			Assert.That(exp.Value, Is.EqualTo("1"));
		}

		/// <summary>
		/// Name is a property of the Customer object, should be transformed in a Member expression
		/// </summary>
		[Test]
		public void BasicPropertyFinder()
		{
			Expression exp = sutConvEx.Convert("Name");
			Assert.That(exp, Is.TypeOf(typeof (MemberExpression)));
		}
		#endregion

		#region PostfixExpressionToLambda

		private Customer aCustomer = new Customer() {Name = "Gian Maria", Age = 10};

		/// <summary>
		/// This test verify that a simple expression with only name access the Name
		/// property of the object.
		/// </summary>
		[Test]
		public void TestBasicMemberAccess()
		{
			Expression<Func<Customer, String>> exp = sutpfex.Execute<String>(new[] { "Name" });
			Func<Customer, String> f = exp.Compile();
			Assert.That(f(aCustomer), Is.EqualTo("Gian Maria"));
		}

		#endregion

	}
}
