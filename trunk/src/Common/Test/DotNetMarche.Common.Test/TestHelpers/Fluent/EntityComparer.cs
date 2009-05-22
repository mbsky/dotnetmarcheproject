using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest;
using Nablasoft.Test.UnitTest;
using NUnit.Framework;


namespace DotNetMarche.Common.Test.TestHelpers.Fluent
{
	[TestFixture]
	public class EntityComparer {

		[Test]
		public void TestSimpleEquals() {
			SimpleTwoProps obja = new SimpleTwoProps("test", 15);
			SimpleTwoProps objb = new SimpleTwoProps("test", 15);
			Assert.That(obja, MyIs.AllPropertiesEqualTo(objb));
		}

		[Test]
		public void TestSimpleEqualsSingleProperty() {
			SimpleTwoProps obja = new SimpleTwoProps("test", 15);
			Assert.That(obja, MyIs.Property("PropA", "test"));
		}

		[Test]
		public void TestNotEquals() {
			SimpleTwoProps obja = new SimpleTwoProps("test", 15);
			SimpleTwoProps objb = new SimpleTwoProps("test", 16);
			Assert.That(obja, MyIs.Not.AllPropertiesEqualTo(objb));
		}

		[Test]
		public void TestNotEquals2() {
			SimpleTwoProps obja = new SimpleTwoProps("test", 15);
			SimpleTwoProps objb = new SimpleTwoProps("testb", 15);
			Assert.That(obja, MyIs.Not.AllPropertiesEqualTo(objb));
		}

		[Test]
		public void TestComplexEquals() {
			ComposedTwoProps obja = new ComposedTwoProps("propv" , new SimpleTwoProps("test", 15));
			ComposedTwoProps objb = new ComposedTwoProps("propv", new SimpleTwoProps("test", 15));
			Assert.That(obja, MyIs.AllPropertiesEqualTo(objb));
		}


		[Test]
		public void TestComplexNotEquals() {
			ComposedTwoProps obja = new ComposedTwoProps("propv", new SimpleTwoProps("test", 15));
			ComposedTwoProps objb = new ComposedTwoProps("propv", new SimpleTwoProps("tesf", 15));
			Assert.That(obja, MyIs.Not.AllPropertiesEqualTo(objb));
		}

		[Test]
		public void TestSimpleEqualsConditionedProperty() {
			SimpleThreeProps obja = new SimpleThreeProps("test", 15, "RR");
			SimpleThreeProps objb = new SimpleThreeProps("test", 16, "DIFFERENT");
			Assert.That(obja, MyIs.SomePropertiesEqualTo(objb, "PropA"));
		}

		[Test]
		public void TestSimpleEqualsConditionedProperty2() {
			SimpleThreeProps obja = new SimpleThreeProps("test", 15, "EQ");
			SimpleThreeProps objb = new SimpleThreeProps("test", 16, "EQ");
			Assert.That(obja, MyIs.SomePropertiesEqualTo(objb, "PropA", "ThirdProperty"));
		}

		[Test]
		public void TestSimpleEqualsConditionedProperty3() {
			SimpleThreeProps obja = new SimpleThreeProps("test", 15, "EQ");
			SimpleThreeProps objb = new SimpleThreeProps("test", 1, "EQ");
			Assert.That(obja, MyIs.SomePropertiesEqualTo(objb, "PropA", "ThirdProperty")
			                  	.And.Property("PropB").GreaterThan(14));
		}
	}
}