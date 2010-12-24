using System;
using System.Diagnostics;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	/// <summary>
	/// Test various customization based on custom ValueExtractor.
	/// </summary>
	[TestFixture]
	public class ValueExtractorCustomizationFixture
	{
		/// <summary>
		/// Suppose that some business rules dictates that field1 * field2 must be in the
		/// range 100, 1000, we could use the standard RangeValidator changing the class used
		/// to do the extraction of value. In this situation we told to validation engine to use
		/// FullCustomValueExtractor1 type object to do the validation.
		/// </summary>
		[RangeValue("Error", 100, 1000,
			ValueExtractorType = typeof (FullCustomValueExtractor1))]
		internal class FullCustomValueExtractor1ClassTest
		{
			public Int32 field1;
			public Int32 field2;
		}

		/// <summary>
		/// This class show how to build a full custom value extractor.
		/// </summary>
		internal class FullCustomValueExtractor1 : IValueExtractor
		{
			#region IValueExtractor Members

			/// <summary>
			/// This custom value extractor is valid only for the object of type
			/// FullCustomValueExtractor1ClassTest, it is expecially build to 
			/// validate that object.
			/// </summary>
			/// <param name="objToValidate"></param>
			/// <returns></returns>
			public object ExtractValue(object objToValidate)
			{
				Debug.Assert(objToValidate.GetType() == typeof (FullCustomValueExtractor1ClassTest));
				var testObj = (FullCustomValueExtractor1ClassTest) objToValidate;
				return testObj.field1*testObj.field2;
			}

			public String SourceName
			{
				get { return ValidationResult.ValidationSourceObject; }
			}

			#endregion
		}

		[Test]
		public void TestCustomValueExtractorGood()
		{
			var obj = new FullCustomValueExtractor1ClassTest();
			obj.field1 = 10;
			obj.field2 = 30;
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj);
			Assert.IsTrue(res, "Object does not validate well");
		}

		[Test]
		public void TestCustomValueExtractorWrong()
		{
			var obj = new FullCustomValueExtractor1ClassTest();
			obj.field1 = 10;
			obj.field2 = 300;
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj);
			Assert.IsFalse(res, "Object does not validate well");
		}
	}
}