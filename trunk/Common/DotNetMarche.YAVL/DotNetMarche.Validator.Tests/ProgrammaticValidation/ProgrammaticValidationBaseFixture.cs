using System;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;
using DotNetMarche.Validator.ValueExtractors.Composite;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests.ProgrammaticValidation
{
	/// <summary>
	/// Test the building of programmatic validation of classes.
	/// </summary>
	[TestFixture]
	public class ProgrammaticValidationBaseFixture
	{
		internal class ProgValid1
		{
			public Int32 intField1;
			public Int32 intField2;
			public String StringProperty { get; set; }
		}

		/// <summary>
		/// Ability to programmatically define rules can be used to do complex validation.
		/// Suppose that business rules dictate that the sum of intField1 and intField2 must
		/// be in a range it is possibile to use standard RangeValueValidator using a 
		/// specific ValueExtractor that atuomatically sum two values.
		/// </summary>
		[Test]
		public void DefineComplexRule1()
		{
			IValueExtractor ve = new BinaryOperatorValueExtractor(
				new NamedValueExtractor("intField1"),
				new NamedValueExtractor("intField2"),
				BinaryOperatorValueExtractor.MathOperation.Addition);
			var v = new Core.Validator();
			v.AddValidationRule(
				typeof (ProgValid1),
				ValidationUnit.CreateValidationUnit(
					ErrorMessage.empty,
					new RangeValueValidator(ve, 100, 1000)));
			var obj = new ProgValid1();
			obj.intField1 = 10;
			obj.intField2 = 200;

			Assert.IsTrue(v.ValidateObject(obj), "Programmatic validation does not work");
		}

		[Test]
		public void TestProgrammaticNullValidator()
		{
			var v = new Core.Validator();
			v.AddValidationRule(
				typeof (ProgValid1),
				ValidationUnit.CreateValidationUnit(
					ErrorMessage.empty,
					new RequiredValidator(new NamedValueExtractor("StringProperty"))));
			var obj = new ProgValid1();
			Assert.IsFalse(v.ValidateObject(obj), "Programmatic validation does not work");
			obj.StringProperty = "notnullvalue";
			Assert.IsTrue(v.ValidateObject(obj), "Programmatic validation does not work");
		}
	}
}