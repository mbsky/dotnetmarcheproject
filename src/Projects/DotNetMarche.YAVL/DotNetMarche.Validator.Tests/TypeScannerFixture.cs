using System;
using System.Collections.Generic;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Tests;
using DotNetMarche.Validator.Validators.Attributes;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class TypeScannerFixture
	{
		/// <summary>
		/// Simple class with only a field 
		/// </summary>
		internal class Simple1Field
		{
			[Required("error")] public String field;
		}

		internal class Simple1Property
		{
			[Required("error")]
			public String Property { get; set; }
		}

		internal class Simple1Field1Property
		{
			[Required("error")] public String field;

			[Required("error")]
			public String Property { get; set; }
		}

		[ConstantResult("", true)]
		internal class SimpleTypeValidation
		{
			public String field;

			public String Property { get; set; }
		}

		/// <summary>
		/// Used to achieve 100% coverage, the ConstantResultValidator is a validator used
		/// to give a constant fail or success error.
		/// </summary>
		[Test]
		public void TestConstantResultValidator()
		{
			var cvr = new ConstantResultValidator(true);
			Assert.IsTrue(cvr.Validate(null));
			var cvrfalse = new ConstantResultValidator(false);
			Assert.IsFalse(cvrfalse.Validate(null));
		}

		/// <summary>
		/// Check if the type scanner is able to descend into object graph structure to 
		/// find validation attribute for all types property of an object.
		/// </summary>
		[Test]
		public void TestScanForDescendantObject()
		{
			var obj = new GraphValidationFixture.Container1();
			var ts = new TypeScanner(obj.GetType());
			var vc = new Dictionary<Type, ValidationUnitCollection>();
			ts.RecursiveScan(vc);
			Assert.AreEqual(2, vc.Count, "Wrong Number of Attributes scanned");
			var lt = new List<Type>();
			lt.AddRange(vc.Keys);
			Assert.AreEqual(typeof (GraphValidationFixture.Container1), lt[0], "Wrong Attribute Type");
			Assert.AreEqual(typeof (GraphValidationFixture.Contained1), lt[1], "Wrong Attribute Type");
		}

		/// <summary>
		/// Container2 has a property of container1 that has a property of type contained1 so 
		/// we expect for the scanner to find 3 classes.
		/// </summary>
		[Test]
		public void TestScanForDescendantObject2()
		{
			var obj = new GraphValidationFixture.Container2();
			var ts = new TypeScanner(obj.GetType());
			var vc = new Dictionary<Type, ValidationUnitCollection>();
			ts.RecursiveScan(vc);
			Assert.AreEqual(3, vc.Count, "Wrong Number of Attributes scanned");
			var lt = new List<Type>();
			lt.AddRange(vc.Keys);
			Assert.AreEqual(typeof (GraphValidationFixture.Container2), lt[0], "Wrong Attribute Type");
			Assert.AreEqual(typeof (GraphValidationFixture.Container1), lt[1], "Wrong Attribute Type");
			Assert.AreEqual(typeof (GraphValidationFixture.Contained1), lt[2], "Wrong Attribute Type");
		}

		/// <summary>
		/// Check if the type scanner is able to descend into object graph structure to 
		/// find validation attribute for all types property of an object. Chek for 
		/// repeated scan to be sure that the object if scanned multiple times does not
		/// find duplicate validation rule.
		/// </summary>
		[Test]
		public void TestScanForDescendantObjectDuplicateCheck()
		{
			var obj = new GraphValidationFixture.Container1();
			var ts = new TypeScanner(obj.GetType());
			var vc = new Dictionary<Type, ValidationUnitCollection>();
			ts.RecursiveScan(vc);
			ts.RecursiveScan(vc);
			Assert.AreEqual(2, vc.Count, "Wrong Number of Attributes scanned");
			var lt = new List<Type>();
			lt.AddRange(vc.Keys);
			Assert.AreEqual(typeof (GraphValidationFixture.Container1), lt[0], "Wrong Attribute Type");
			Assert.AreEqual(typeof (GraphValidationFixture.Contained1), lt[1], "Wrong Attribute Type");
		}

		[Test]
		public void TestScanForSingleField()
		{
			var obj = new Simple1Field();
			var ts = new TypeScanner(obj.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.AreEqual(1, vc.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (RequiredValidator), vc[0].Validator, "Wrong Attribute Type");
		}

		[Test]
		public void TestScanForSingleFieldRepeateScanner()
		{
			var obj = new Simple1Field();
			var ts = new TypeScanner(obj.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.AreEqual(1, vc.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (RequiredValidator), vc[0].Validator, "Wrong Attribute Type");
			ValidationUnitCollection anotherscan = ts.Scan();
			Assert.AreEqual(1, anotherscan.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (RequiredValidator), anotherscan[0].Validator, "Wrong Attribute Type");
		}

		[Test]
		public void TestScanForSingleProperty()
		{
			var obj = new Simple1Property();
			var ts = new TypeScanner(obj.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.AreEqual(1, vc.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (RequiredValidator), vc[0].Validator, "Wrong Attribute Type");
		}

		[Test]
		public void TestScanForSinglePropertyAndSingleField()
		{
			var obj = new Simple1Field1Property();
			var ts = new TypeScanner(obj.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.AreEqual(2, vc.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (RequiredValidator), vc[0].Validator, "Wrong Attribute Type");
			Assert.IsInstanceOfType(typeof (RequiredValidator), vc[1].Validator, "Wrong Attribute Type");
		}

		[Test(Description = "Test if the scanner scan the attributes of type")]
		public void TestScanForTypeValidator()
		{
			var obj = new SimpleTypeValidation();
			var ts = new TypeScanner(obj.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.AreEqual(1, vc.Count, "Wrong Number of Attributes scanned");
			Assert.IsInstanceOfType(typeof (ConstantResultValidator), vc[0].Validator, "Wrong Attribute Type");
		}
	}
}