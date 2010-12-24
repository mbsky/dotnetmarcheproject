using System;
using System.Diagnostics;
using System.Reflection;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.ValueExtractors;
using DotNetMarche.Validator.ValueExtractors.Composite;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	internal class ClassTest1
	{
		public Int32 field;
		public Int32 otherfield;

		public String Property { get; set; }

		public String OtherProperty { get; set; }
	}

	[TestFixture(Description = "Test field and property value extractor")]
	public class ValueExtractorTestFixture
	{
		private FieldInfo fi;
		private PropertyInfo pi;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			fi = typeof (ClassTest1).GetField("field", BindingFlags.Public | BindingFlags.Instance);
			pi = typeof (ClassTest1).GetProperty("Property", BindingFlags.Public | BindingFlags.Instance);
			Debug.Assert(fi != null, "Wrong reflection");
			Debug.Assert(pi != null, "Wrong reflection");
		}

		[Test]
		public void ExtractByBinaryOperatorAdd()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Addition);
			var obj = new ClassTest1();
			obj.field = 10;
			obj.otherfield = 34;
			Assert.AreEqual(44, ve.ExtractValue(obj), "Binary Extractor does not work.");
		}

		[Test]
		public void VerifySimbolicNameAddition()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Addition);
			Assert.That(ve.SourceName, Is.EqualTo("field + otherfield"));
		}

		[Test]
		public void VerifySimbolicNameDivision()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Division);
			Assert.That(ve.SourceName, Is.EqualTo("field / otherfield"));
		}

		[Test]
		public void VerifySimbolicNameMultiplication()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Multiplication);
			Assert.That(ve.SourceName, Is.EqualTo("field * otherfield"));
		}

		[Test]
		public void VerifySimbolicNameSubtraction()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Subtraction);
			Assert.That(ve.SourceName, Is.EqualTo("field - otherfield"));
		}
		[Test]
		public void ExtractByBinaryOperatorDivision()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Division);
			var obj = new ClassTest1();
			obj.field = 10;
			obj.otherfield = 2;
			Assert.AreEqual(5, ve.ExtractValue(obj), "Binary Extractor does not work.");
		}

		[Test]
		public void ExtractByBinaryOperatorMultiplication()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Multiplication);
			var obj = new ClassTest1();
			obj.field = 10;
			obj.otherfield = 34;
			Assert.AreEqual(340, ve.ExtractValue(obj), "Binary Extractor does not work.");
		}

		[Test]
		public void ExtractByBinaryOperatorSubtract()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				BinaryOperatorValueExtractor.MathOperation.Subtraction);
			var obj = new ClassTest1();
			obj.field = 10;
			obj.otherfield = 34;
			Assert.AreEqual(-24, ve.ExtractValue(obj), "Binary Extractor does not work.");
		}

		/// <summary>
		/// Ask for an unknown operator should throw an exception.
		/// </summary>
		[Test, ExpectedException(typeof (ArgumentException))]
		public void ExtractByBinaryOperatorUnknownOperator()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("field"),
				new NamedValueExtractor("otherfield"),
				(BinaryOperatorValueExtractor.MathOperation) 10000);
			var obj = new ClassTest1();
			obj.field = 10;
			obj.otherfield = 34;
			ve.ExtractValue(obj);
		}

		[Test]
		public void ExtractFieldByName()
		{
			NamedValueExtractor nve;
			nve = new NamedValueExtractor("field");
			var test = new ClassTest1();
			test.field = 30;
			Assert.AreEqual(test.field, nve.ExtractValue(test), "NamedValueExtractor does not dynamically get field");
		}

		[Test]
		public void NamedValueExtractorWithInvalidField()
		{
			NamedValueExtractor nve;
			nve = new NamedValueExtractor("xxx");
			var test = new ClassTest1();
			test.field = 30;
			Assert.Throws(typeof (ArgumentException), () => nve.ExtractValue(test));
		}

		[Test]
		public void ExtractFieldByProperty()
		{
			NamedValueExtractor nve;
			nve = new NamedValueExtractor("Property");
			var test = new ClassTest1();
			test.Property = "StringTest";
			Assert.AreEqual(test.Property, nve.ExtractValue(test), "NamedValueExtractor does not dynamically get Property");
		}

		[Test]
		public void ExtractFieldValue()
		{
			FieldInfoValueExtractor fe;
			fe = new FieldInfoValueExtractor(fi);
			var test = new ClassTest1();
			test.field = 34;
			Assert.AreEqual(34, fe.ExtractValue(test), "FieldExtractor Does not extract field values");
		}		
		
		[Test]
		public void ExtractFieldValueCorrectlyFindSourceName()
		{
			FieldInfoValueExtractor fe;
			fe = new FieldInfoValueExtractor(fi);
			Assert.That(fe.SourceName, Is.EqualTo("field"));
		}

		[Test]
		public void ExtractPropertyValue()
		{
			PropertyInfoValueExtractor pe;
			pe = new PropertyInfoValueExtractor(pi);
			var test = new ClassTest1();
			test.Property = "StringTest";
			Assert.AreEqual("StringTest", pe.ExtractValue(test), "FieldExtractor Does not extract property values");
		}

		[Test]
		public void ExtractPropertyValueCorrectlyFindSourceName()
		{
			PropertyInfoValueExtractor pe;
			pe = new PropertyInfoValueExtractor(pi);
			Assert.That(pe.SourceName, Is.EqualTo("Property"));
		}

		[Test]
		public void ExtractLambdaValidation()
		{
			LambdaExtractor<ClassTest1> sut = new LambdaExtractor<ClassTest1>(c => c.OtherProperty, "OtherProperty");
			Assert.That(sut.SourceName, Is.EqualTo("OtherProperty"));
		}

		[Test]
		public void ExtractLambdaValidationDefaultSourceName()
		{
			LambdaExtractor<ClassTest1> sut = new LambdaExtractor<ClassTest1>(c => c.OtherProperty);
			Assert.That(sut.SourceName, Is.EqualTo(ValidationResult.ValidationSourceObject));
		}
	}
}