using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class ValidatorAdvancedValidationFixture
	{
		private const String cSimpleErrorMessage = "error";
		private const String cAlternativeErrorMessage = "alternativeError";
		private const String cDefaultValueSimple1Property = "defvalue";

		#region TestClasses 

		internal class ClassTest
		{
			[Required(cSimpleErrorMessage, null)]
			public String Property { get; set; }
		}

		internal class OwnAClassTest
		{
			public String Apro { get; set; }

			public ClassTest Reference { get; set; }

			public ClassTest Reference2 { get; set; }
		}

		internal class AnotherClassTest
		{
			[Required(cSimpleErrorMessage, null)]
			public String Property1 { get; set; }

			[Required(cSimpleErrorMessage, null)]
			public String Property2 { get; set; }
		}

		internal class OwnAnotherClassTest
		{
			public AnotherClassTest Reference { get; set; }
		}


		#endregion

		[Test]
		public void VerifyValidateRecursiveObject()
		{
			var rng = new OwnAClassTest() {Reference = new ClassTest(), Reference2 = new ClassTest()};
			var v = new Core.Validator();
			
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Reference.Property"));
			res = v.ValidateObject(rng);
			Assert.IsTrue(res);
		}
			
		[Test]
		public void VerifyThatWithRecursiveValidationStopOnFirstErrorIsDisabled()
		{
			var rng = new OwnAClassTest() { Reference = new ClassTest(), Reference2 = new ClassTest() };
			var v = new Core.Validator();

			ValidationResult res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors, Has.Count.EqualTo(2));
		}

		[Test]
		public void VerifyThatWithRecursiveValidationStopOnFirstError()
		{
			var rng = new OwnAClassTest() { Reference = new ClassTest(), Reference2 = new ClassTest() };
			var v = new Core.Validator();

			ValidationResult res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation | ValidationFlags.StopOnFirstError);
			Assert.That(res.Errors, Has.Count.EqualTo(1));
		}

		[Test]
		public void VerifyThatWithRecursiveValidationDoesNotStopOnFirstErrorOnSubValidation()
		{
			var rng = new OwnAnotherClassTest() { Reference = new AnotherClassTest()};
			var v = new Core.Validator();

			ValidationResult res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors, Has.Count.EqualTo(2));
		}

	}
}
