using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetMarche.Common.Test.Utils.Expressions.AuxClasses;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using DotNetMarche.Utils.Expressions;
using DotNetMarche.Utils.Expressions.Concrete;
using DotNetMarche.Utils.Linq;
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
			IOperatorsChecker<String> opChecker = new QueryToObjectOperatorsChecker();
			ITokenizer<String, String> tokenizer = new StringAdvancedTokenizer(opChecker);
			sut = new ExpressionConverterExt<String, String, String>(opChecker, tokenizer, converter);

			QueryToObjectOperatorsChecker opCheckerex = new QueryToObjectOperatorsChecker();
			sutConvEx = new StringToExpressionTokenConverter<Customer>();
			ITokenizer<String, String> tokenizerex = new StringAdvancedTokenizer(opCheckerex);

			sutEx = new ExpressionConverterExt<String, String, Expression>(opCheckerex, tokenizerex, sutConvEx);
		}

		#region Test basic conversion

		[Test]
		public void TestBasicAdd()
		{
			IList<String> postfix = sut.InfixToPostfix("A + B");
			CollectionAssert.AreEquivalent(postfix, new [] { "A", "B", "+" });
		}

		[Test]
		public void TestPrecedence()
		{
			IList<String> postfix = sut.InfixToPostfix("A + B * C");
			CollectionAssert.AreEquivalent(new [] { "A", "B", "C", "*", "+" }, postfix);
		}

		[Test]
		public void TestPrecedenceParentesis()
		{
			IList<String> postfix = sut.InfixToPostfix("(A + B) * C");
			CollectionAssert.AreEquivalent(new[] { "A", "B", "+" , "C", "*" }, postfix );
		}

		[Test]
		public void TestPrecedenceNot()
		{
			IList<String> postfix = sut.InfixToPostfix("!A && B");
			CollectionAssert.AreEquivalent(new[] { "A", "!", "B", "&&"}, postfix);
		}

		[Test]
		public void TestPrecedenceNot2()
		{
			IList<String> postfix = sut.InfixToPostfix("!A && !B");
			CollectionAssert.AreEquivalent(new[] { "A", "!", "B", "!", "&&" }, postfix);
		}

		[Test]
		public void TestPrecedenceAndOr()
		{
			IList<String> postfix = sut.InfixToPostfix("A && B || C");
			CollectionAssert.AreEquivalent(new[] { "A", "B", "&&", "C", "||" }, postfix);
		}

		[Test]
		public void TestPrecedenceAndOr2()
		{
			IList<String> postfix = sut.InfixToPostfix("A || B && C");
			CollectionAssert.AreEquivalent(new[] { "A", "B", "C", "&&", "||" }, postfix);
		}

		#endregion

		#region StringToExpressionConverterTest

		[Test]
		public void BasicConstantConversion()
		{
			Expression exp = sutConvEx.Convert("1");
			Assert.That(exp, Is.TypeOf(typeof(ConstantExpression)));
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
			Assert.That(exp, Is.TypeOf(typeof(MemberExpression)));
		}
		#endregion

		#region PostfixExpressionToLambda

		private Customer aCustomer = new Customer() { Name = "Gian Maria", Age = 10 };

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

		/// <summary>
		/// This test verify that a simple expression with only name access the Name
		/// property of the object.
		/// </summary>
		[Test]
		public void TestAConstant()
		{
			Expression<Func<Customer, String>> exp = sutpfex.Execute<String>(new[] { "TESTO" });
			Func<Customer, String> f = exp.Compile();
			Assert.That(f(aCustomer), Is.EqualTo("TESTO"));
		}

		[Test]
		public void TestConditionEqual()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "Name", "Gian Maria", "==" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestConditionEqualFalse()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "Name", "Giandd Maria", "==" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.False);
		}

		[Test]
		public void TestConditionGt()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "Age", "14", ">" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.False);
		}

		[Test]
		public void TestConditionGt2()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "14", "Age", ">" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestConditionNot()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "Name", "Gian Maria", "!=" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.False);
		}



		/// <summary>
		/// if you use a property that does not exists the expression should throw an exception.
		/// </summary>
		[Test, ExpectedException]
		public void TestConditionWithWrongProperty()
		{
			Expression<Func<Customer, Boolean>> exp = sutpfex.Execute<Boolean>(new[] { "14", "NotExistingProperty", ">" });
			Func<Customer, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer), Is.True);
		}

		#endregion

		#region Parser

		[Test]
		public void TestConditionEqualDynLinq()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name == 'Gian Maria'");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test, ExpectedException]
		public void TestDynNotExistingProperty()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("NotExistingProperty == 'Gian Maria'");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestDynNotEqual()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name != 'Gian Maria'");
			Assert.That(f(aCustomer), Is.False);
		}

		[Test]
		public void TestDynLesserThanOrEqual()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Age <= 14");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestDynNotCondition()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("!(Name == 'Gian Maria')");
			Assert.That(f(aCustomer), Is.False);
		}

		[Test]
		public void TestDynNotCondition2()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("!(Name == 'Gianf Maria')");
			Assert.That(f(aCustomer), Is.True);
		}

		#endregion

		#region Logic Operators

		[Test]
		public void TestLogicAnd()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name == 'Gian Maria' && Age > 5");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestLogicAndWithParenthesis()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name == 'Gian Maria' && !(Age < 10)");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestPrecedenceAndOrString()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name == 'Gian Maria' && Age == 10 || 1 == 0");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestPrecedenceAndOrString2()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("Name == 'Gian Maria' || 1 == 0 && Age != 10");
			Assert.That(f(aCustomer), Is.True);
		}

		[Test]
		public void TestPrecedenceAndOrString2P()
		{
			Func<Customer, Boolean> f = DynamicLinq.ParseToFunction<Customer, Boolean>("(Name == 'Gian Maria' || 1 == 0 )&& Age != 10");
			Assert.That(f(aCustomer), Is.False);
		}


		#endregion

		#region Test Parameters

		[Test]
		public void BasicAddWithParameter()
		{
			Expression<Func<Customer, Object, Boolean>> exp = sutpfex.Execute<Object, Boolean>(new[] { "Name", ":name", "==" });
			Func<Customer, Object, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer, "Gian Maria"), Is.True);
		}		
		
		[Test]
		public void BasicAddWithParameterIsFalse()
		{
			Expression<Func<Customer, Object, Boolean>> exp = sutpfex.Execute<Object, Boolean>(new[] { "Name", ":name", "==" });
			Func<Customer, Object, Boolean> f = exp.Compile();
			Assert.That(f(aCustomer, "Gianf Maria"), Is.False);
		}
	
		#endregion

	}
}
