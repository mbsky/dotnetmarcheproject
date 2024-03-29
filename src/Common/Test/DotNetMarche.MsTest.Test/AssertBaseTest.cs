﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotNetMarche.MsTest.Constraints;
using DotNetMarche.MsTest.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetMarche.MsTest.Test
{
	/// <summary>
	/// Summary description for AssertBaseTest
	/// </summary>
	[TestClass]
	public class AssertBaseTest
	{
		public AssertBaseTest()
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
		public void BaseAssert()
		{
			Int32 obj = 42;
			obj.Assert(new EqualsConstraint(42));
		}

		[TestMethod]
		public void BaseAssertFluent()
		{
			Int32 obj = 42;
			FluentAssert.That(obj, new EqualsConstraint(42));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelper()
		{
			Int32 obj = 42;
			FluentAssert.That(obj, Is.EqualsTo(42));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperGt()
		{
			Int32 obj = 42;
			FluentAssert.That(obj, Is.GreaterThan(41));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperLt()
		{
			Int32 obj = 42;
			FluentAssert.That(obj, Is.LesserThan(43));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperLtOrGt()
		{
			Int32 obj = 42;
			FluentAssert.That(obj, Is.LesserThan(100).Or.GreaterThan(200));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperLtOrGtR()
		{
			Int32 obj = 400;
			FluentAssert.That(obj, Is.LesserThan(100).Or.GreaterThan(200));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperLtOrGtNoWay()
		{
			Int32 obj = 150;
			FluentAssert.That(obj, Is.LesserThan(200).And.GreaterThan(100));
		}

		[TestMethod]
		public void BaseAssertFluentWithSyntaxHelperLtOrGtRo()
		{
			Int32 obj = 400;
			Assert.IsTrue(obj < 100 || obj > 200);
		}
	}
}
