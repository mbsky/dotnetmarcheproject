#pragma warning disable 0649
using System;
using System.Collections.Generic;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Tests.ResourcesFiles;
using DotNetMarche.Validator.Validators;
using DotNetMarche.Validator.Validators.Attributes;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;
using NUnit.Framework;
using Rhino.Mocks;

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

		internal class Simple2Property
		{
			
			public String SProperty { get; set; }

			public Int32 IProperty { get; set; }

			public Int32? NullableInt { get; set; }

			public DateTime? NullableDateTime { get; set; }
		}

		internal class Simple1Field1Property
		{
			[Required(cSimpleErrorMessage)] 
			public String field;

			[Required(cAlternativeErrorMessage, cDefaultValueSimple1Property)]
			public String Property { get; set; }
		}

		internal class RFTestClass1
		{
			[RangeValue(cSimpleErrorMessageRange, 0, 100)] 
			public Int32 field;

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
		public void RangeTestWrongObjectErrorContainsPropertyName()
		{
			var rng = new RFTestClass1();
			rng.Property = 10000.0f;
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(rng);
			Assert.IsFalse(res, "Range Object does wrong validation.");
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Property"));
		}

		[Test]
		public void CoreTestIValidatorReturnsSourceName()
		{
			var rng = new RFTestClass1();
			IValueExtractor extractor = MockRepository.GenerateStub<IValueExtractor>();
			extractor.Expect(obj => obj.ExtractValue(rng)).Return(100000.0f);
			extractor.Expect(obj => obj.SourceName).Return("TESTPNAME");
			var v = new RangeValueValidator(extractor, 0.0, 1.0);
			SingleValidationResult res = v.Validate(rng);
			Assert.IsFalse(res, "Range Object does wrong validation.");
			Assert.That(res.SourceName, Is.EqualTo("TESTPNAME"));
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
		public void TestValidateWithException()
		{
			var s1f = new Simple1Field();
			var v = new Core.Validator();
			Assert.Throws<ValidationException>(() => v.CheckObject(s1f));
		}

		/// <summary>
		/// Verifies that exception contains the source names of the properties that
		/// returns errors.
		/// </summary>
		[Test]
		public void TestValidateWithExceptionForSourceMessages()
		{
			var obj = new Simple1Property();
			var v = new Core.Validator();
			try
			{
				v.CheckObject(obj);
			}
			catch (ValidationException ex)
			{
				Assert.That(ex.Errors, Has.Count.EqualTo(1));
				Assert.That(ex.Errors[0].SourceName, Is.EqualTo("Property"));
				return;
			}
			Assert.Fail("Exception expected");
		}

		[Test]
		public void VerifySingleValidationResultSourceName()
		{
			Assert.Throws<ArgumentException>(() => new SingleValidationResult(false, "", "", ""));
		}

		[Test]
		public void TestValidatorResultForMessages()
		{
			var vr = new ValidationResult(true);
			vr.AddErrorMessage("TEST", "Property");
			vr.AddErrorMessage("TESTQ", "Property");
			Assert.That(vr.ErrorMessages, Is.EquivalentTo(new String[]
			                                              	{
			                                              		"TEST",
			                                              		"TESTQ"
			                                              	}
			                              	));
		}

		[Test]
		public void TestValidationExceptionForMessages()
		{
			var vr = new ValidationException(new[] { new ValidationError("1", ""), new ValidationError("2", "") });
			
			Assert.That(vr.ErrorMessages, Is.EquivalentTo(new []{"1", "2"}));
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
					.Required().Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1f);
			Assert.IsFalse(res, "Object does not validate well");
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0], Is.EqualTo("ErrorMessage"));
		}

		[Test]
		public void TestGoodObjectFluent3()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.field = "Test";
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>()
					.OnMember("field")
					.LengthInRange(10, 40)
					.Message("ErrorMessage"));
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
				.Required().OnMember("field")
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

		[Test]
		public void VerifyRequiredDoesNotValidateEmptyString()
		{
		var s1F = new Simple2Property();
			s1F.SProperty = String.Empty;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple2Property>()
				.OnMember<Simple2Property>(s => s.SProperty)
	          .Required(String.Empty)
				.Message("ErrorMessage"));

			ValidationResult res = v.ValidateObject(s1F);
			Assert.IsFalse(res, "Empty string should not pass required validation");
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
		public void TestFluentForErrorMessageWithLambda()
		{
			var s1f = new Simple1FieldWithoutAttribute();
			s1f.intField = -50;
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<Simple1FieldWithoutAttribute>(o => o.intField)
				.IsInRange(0, 100)
				.Message(() => TestRes.Test));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].Message, Is.EqualTo("This is a test message"));
		}

		[Test]
		public void TestFluentForSourceWithLambda()
		{
			var s1f = new Simple1Property();
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.ForMember<Simple1Property>(o => o.Property)
				.Required()
				.Message(() => TestRes.Test));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Property"));
		}
		
		[Test]
		public void TestFluentForSourceWithLambdaInt()
		{
			var s1f = new Simple2Property();
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.ForMember<Simple2Property>(o => o.IProperty)
				.IsInRange(10, 100)
				.Message(() => TestRes.Test));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("IProperty"));
		}

		[Test]
		public void TestFluentForSourceWithLambdaAndNullableInt()
		{
			var s1f = new Simple2Property();
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.ForMember<Simple2Property>(o => o.NullableInt)
				.IsInRange(10, 100)
				.Message(() => TestRes.Test));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("NullableInt"));
		}

		[Test]
		public void TestFluentForSourceWithLambdaAndNullableDateTime()
		{
			var s1f = new Simple2Property();
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.ForMember<Simple2Property>(o => o.NullableDateTime)
				.Required()
				.Message(() => TestRes.Test));
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("NullableDateTime"));
		}
		
		[Test]
		public void TestFluentForSourceWithLambdaAndConvertExpression()
		{
			var s1f = new Simple2Property();
			Core.Validator v = new Core.Validator();
			v.AddRule(
				Rule.For<Simple2Property>(dto => dto.NullableDateTime)
					.Custom<DateTime?>(date => false)
					.Message(() => TestRes.Test));
			
			ValidationResult res = v.ValidateObject(s1f);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("NullableDateTime"));
		}

		[Test]
		public void TestFluentInvalidNoProperty()
		{
			Core.Validator v = new Core.Validator();
			Assert.Throws<ArgumentException>(() =>
			                                 v.AddRule(Rule.ForMember<Simple1Field>(o => o.field)
			                                           	.IsInRange(10, 100)
			                                           	.Message(() => TestRes.Test)));
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

		[Test]
		public void VerifyWithCustomValidationCAnSelectPropertyName()
		{
			var s1pf = new Simple1Field1Property() {field = "a", Property="b"};
			var v = new Core.Validator();
			v.AddRule(Rule.For<Simple1Field1Property>(l => l, "Property")
				 .Custom<Simple1Field1Property>(l => l.Property == "bla bla bla")
				 .Message("Property is not equal to bla bla bla"));
			
			var res = v.ValidateObject(s1pf);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Property")); 
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