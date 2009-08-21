using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotNetMarche.MsTest.Constraints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetMarche.MsTest;

namespace DotNetMarche.MsTest.Test
{
	/// <summary>
	/// Summary description for ConstraintBaseTest
	/// </summary>
	[TestClass]
	public class ConstraintBaseTest
	{
		public ConstraintBaseTest()
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
		public void EqualsConstraint()
		{
			IConstraint sut = new EqualsConstraint(42);
			Assert.IsTrue(sut.Validate(42));
		}

		[TestMethod]
		public void GreaterThenConstraint()
		{
			IConstraint sut = new GreaterThanConstraint(10);
			Assert.IsTrue(sut.Validate(11));
		}

		[TestMethod]
		public void GreaterThenConstraintFalse()
		{
			IConstraint sut = new GreaterThanConstraint(10);
			Assert.IsFalse(sut.Validate(10));
		}

		[TestMethod]
		public void LessThenConstraint()
		{
			IConstraint sut = new LessThanConstraint(10);
			Assert.IsTrue(sut.Validate(9));
		}

		[TestMethod]
		public void LessThenConstraintFalse()
		{
			IConstraint sut = new LessThanConstraint(10);
			Assert.IsFalse(sut.Validate(10));
		}

		[TestMethod]
		public void LessThenOrEqualConstraint()
		{
			IConstraint sut = new LessThanOrEqualConstraint(10);
			Assert.IsTrue(sut.Validate(9));
		}

		[TestMethod]
		public void LessThenOrEqualConstraintFalse()
		{
			IConstraint sut = new LessThanOrEqualConstraint(10);
			Assert.IsFalse(sut.Validate(11));
		}

		[TestMethod]
		public void LessThenOrEqualConstraintLimit()
		{
			IConstraint sut = new LessThanOrEqualConstraint(10);
			Assert.IsTrue(sut.Validate(10));
		}

		[TestMethod]
		public void GreaterThenOrEqualConstraint()
		{
			IConstraint sut = new GreaterThanOrEqualConstraint(10);
			Assert.IsTrue(sut.Validate(11));
		}

		[TestMethod]
		public void GreaterThenOrEqualConstraintFalse()
		{
			IConstraint sut = new GreaterThanOrEqualConstraint(10);
			Assert.IsFalse(sut.Validate(9));
		}

		[TestMethod]
		public void GreaterThenOrEqualConstraintLimit()
		{
			IConstraint sut = new GreaterThanOrEqualConstraint(10);
			Assert.IsTrue(sut.Validate(10));
		}
	}
}
