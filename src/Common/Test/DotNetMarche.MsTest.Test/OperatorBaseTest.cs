using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotNetMarche.MsTest.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetMarche.MsTest.Test
{
	/// <summary>
	/// Summary description for OperatorBaseTest
	/// </summary>
	[TestClass]
	public class OperatorBaseTest
	{
		public OperatorBaseTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestBasic1()
		{
			IConstraint c = Is.LesserThan(100).Or.GreaterThan(200);
			Assert.IsTrue(c.Validate(99));
			Assert.IsTrue(c.Validate(201));
			Assert.IsFalse(c.Validate(150));
		}

		[TestMethod]
		public void TestBasic2()
		{
			IConstraint c = Is.LesserThan(100) | Is.GreaterThan(200) & Is.GreaterThan(300);
			Assert.IsTrue(c.Validate(99));
		}

		[TestMethod]
		public void TestBasicParenthesis()
		{
			IConstraint c = (Is.LesserThan(100) | Is.GreaterThan(200)) & Is.GreaterThan(300);
			Assert.IsFalse(c.Validate(99));
		}
	}
}
