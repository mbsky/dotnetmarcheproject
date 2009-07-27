using System;
using DotNetMarche.Validator.Validators;
using DotNetMarche.Validator.Validators.Attributes;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	/// <summary>
	/// This class test basic functionality of the Validator main class
	/// </summary>
	[TestFixture]
	public class BaseValidatorFixture
	{
		private const String cSimpleErrorMessage = "error";
		private const String cAlternativeErrorMessage = "alternativeError";
		private const String cDefaultValueSimple1Property = "defvalue";

		private const String cSimpleErrorMessageRange = "Value ${ActualValue} does not fall in range ${ExpectedValue}";

		/// <summary>
		/// Simple class with only a field 
		/// </summary>
		internal class Simple1Field
		{
			[Required(cSimpleErrorMessage)] public String field;
		}

		/// <summary>
		/// Simple class with only a field 
		/// </summary>
		internal class Simple1FieldWithoutAttribute
		{
			public String field;
			public Int32 intField;
		}

		internal class Simple1Property
		{
			[Required(cSimpleErrorMessage, cDefaultValueSimple1Property)]
			public String Property { get; set; }
		}

		internal class Simple1Field1Property
		{
			[Required(cSimpleErrorMessage)] public String field;

			[Required(cAlternativeErrorMessage, cDefaultValueSimple1Property)]
			public String Property { get; set; }
		}

		internal class RFTestClass1
		{
			[RangeValue(cSimpleErrorMessageRange, 0, 100)] public Int32 field;

			[RangeValue(cSimpleErrorMessageRange, -100, 200)]
			public Single Property { get; set; }
		}

		[Test]
		public void RangeTestGoodObject()
		{
			var rng = new RFTestClass1();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(rng);
			Assert.IsTrue(res, "Range Object does wrong validation.");
		}

		[Test]
		public void RangeTestWrongObject()
		{
			var rng = new RFTestClass1();
			rng.Property = 10000.0f;
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(rng);
			Assert.IsFalse(res, "Range Object does wrong validation.");
			Assert.AreEqual(1, res.ErrorMessages.Count, "Wrong number of errors.");
			String expected = cSimpleErrorMessageRange;
			expected = expected.Replace("${ActualValue}", rng.Property.ToString());
			expected = expected.Replace("${ExpectedValue}", "-100-200");
			Assert.AreEqual(expected, res.ErrorMessages[0], "Wrong formatting.");
		}

		[Test]
		public void TestGoodObject()
		{
			var s1f = new Simple1Field();
			s1f.field = "value";
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsTrue(res, "Object does not validate well");
		}		
		
		[Test]
		public void TestGoodObjectFluent()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.field = null;
			Core.Validator v = new Core.Validator();
			v.AddRule<Simple1FieldWithoutAttribute>(
				new RequiredValidator(new NamedValueExtractor("field")),
				"ErrorMessage");
			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}

		[Test]
		public void TestGoodObjectFluent2()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.field = null;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>()
	          	.OnMember("field")
	          	.Required.Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}

		[Test]
		public void TestGoodObjectFluentExtractorBeforeRequired()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.field = null;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>()
				.Required.OnMember("field")
				.Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}	

		[Test]
		public void TestGoodObjectFluentRange()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.intField = 101;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>()
				.OnMember("intField")
	          	.IsInRange(0, 100)
				.Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}

		/// <summary>
		/// Insert in fluent interface the validator before the extractor.
		/// </summary>
		[Test]
		public void TestGoodObjectFluentInsertRangeBeforeExtractor()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.intField = 101;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>()
				.IsInRange(0, 100)
				.OnMember("intField")
				.Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}		
		
		/// <summary>
		/// Insert in fluent interface the validator before the extractor.
		/// </summary>
		[Test]
		public void TestGoodObjectFluentLambdaExtractor()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.intField = 50;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>(o => o.intField)
				.IsInRange(0, 100)
				.Message("ErrorMessage"));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsTrue(res, "Object does not validate well");
			s1f.intField = -1;
			res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
		}

		[Test]
		public void TestWrongObject()
		{
			var s1f = new Simple1Field();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.AreEqual(1, res.ErrorMessages.Count, "Wrong number of error detected");
			Assert.AreEqual(cSimpleErrorMessage, res.ErrorMessages[0]);
		}

		/// <summary>
		/// Test the object that has field correctly setted but property is null because it has default null value
		/// this test check that only the right numers of errors will be returned.
		/// </summary>
		[Test]
		public void TestWrongObjectNum1()
		{
			var s1pf = new Simple1Field1Property();
			s1pf.Property = cDefaultValueSimple1Property;
			s1pf.field = "notnullvalue";
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1pf);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.AreEqual(1, res.ErrorMessages.Count, "Wrong number of error detected");
			Assert.AreEqual(cAlternativeErrorMessage, res.ErrorMessages[0]);
		}

		/// <summary>
		/// Test the object that has both field and property set with wrong value
		/// </summary>
		[Test]
		public void TestWrongObjectNum2()
		{
			var s1pf = new Simple1Field1Property();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1pf);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.AreEqual(2, res.ErrorMessages.Count, "Wrong number of error detected");
			Assert.IsTrue(res.ErrorMessages.Contains(cAlternativeErrorMessage));
			Assert.IsTrue(res.ErrorMessages.Contains(cSimpleErrorMessage));
		}

		/// <summary>
		/// Test if validator really stops at the first error that encounter.
		/// </summary>
		[Test]
		public void TestWrongObjectNum2WithStopOnError()
		{
			var s1pf = new Simple1Field1Property();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1pf, ValidationFlags.StopOnFirstError);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.AreEqual(1, res.ErrorMessages.Count, "Wrong number of error detected");
		}

		/// <summary>
		/// Test the object that has 1 string property and has default value setted.
		/// </summary>
		[Test]
		public void TestWrongObjectWithDefaultValue()
		{
			var s1p = new Simple1Property();
			s1p.Property = cDefaultValueSimple1Property;
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(s1p);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.AreEqual(1, res.ErrorMessages.Count, "Wrong number of error detected");
			Assert.AreEqual(cSimpleErrorMessage, res.ErrorMessages[0]);
		}
	}
}