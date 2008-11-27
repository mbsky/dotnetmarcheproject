using System;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	/// <summary>
	/// Test for Graph validation.
	/// </summary>
	[TestFixture]
	public class GraphValidationFixture
	{
		internal class Contained1
		{
			[Required]
			public String requiredProperty { get; set; }
		}

		/// <summary>
		/// This class has no constraint
		/// </summary>
		internal class Contained2
		{
			public String requiredProperty { get; set; }
		}

		internal class Container1
		{
			[Required] public Contained1 containedField;
		}

		internal class Container2
		{
			public Container1 containedField;
		}

		internal class Container3
		{
			public Contained2 containedFieldNotRequired;
		}

		/// <summary>
		/// Field of the first class is not null, but the object contained is not valid.
		/// Now we ask to descend object graph validation.
		/// </summary>
		[Test]
		public void BasicFieldGraphTestRecursive()
		{
			var obj = new Container1();
			obj.containedField = new Contained1();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj, ValidationFlags.StopOnFirstError | ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res, "Object field does not validate.");
		}

		/// <summary>
		/// We have an inner object that has no validation rule.
		/// </summary>
		[Test]
		public void BasicFieldGraphTestRecursiveInternalClassNoRule()
		{
			var obj = new Container3();
			obj.containedFieldNotRequired = new Contained2();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj, ValidationFlags.StopOnFirstError | ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}

		/// <summary>
		/// We have two properties, the second property is null, so we must check if the
		/// validation engine correctly stop walking the graph when an object is null
		/// </summary>
		[Test]
		public void BasicFieldGraphTestRecursiveInternalClassNull()
		{
			var obj = new Container3();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj, ValidationFlags.StopOnFirstError | ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}

		/// <summary>
		/// Field of the first class is not null, but the object contained is not valid. The 
		/// default validation is not to descend graph to validate the inner object.
		/// </summary>
		[Test]
		public void BasicFieldGraphTestValidationError()
		{
			var obj = new Container1();
			obj.containedField = new Contained1();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj);
			Assert.IsTrue(res, "Validate object method defaults to not validate contained objects");
			res = v.ValidateObject(obj.containedField);
			Assert.IsFalse(res, "inner object must not validate");
		}

		/// <summary>
		/// Try to validate the inner class.
		/// </summary>
		[Test]
		public void BasicFieldGraphTestValidationResult()
		{
			var obj = new Container1();
			obj.containedField = new Contained1();
			obj.containedField.requiredProperty = "This is valid";
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj, ValidationFlags.StopOnFirstError | ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res, "Object field should validate.");
		}

		[Test]
		public void BasicNullTest()
		{
			var obj = new Container1();
			var v = new Core.Validator();
			ValidationResult res = v.ValidateObject(obj);
			Assert.IsFalse(res, "Property is null should not validate");
		}
	}
}