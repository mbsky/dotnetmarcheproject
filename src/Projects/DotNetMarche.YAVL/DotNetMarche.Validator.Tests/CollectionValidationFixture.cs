using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Validators;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class CollectionValidationFixture
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


		internal class OwnCollectionAnotherClassTest
		{
			[NotEmpty("List property should be not empty")]
			public List<AnotherClassTest> List { get; set; }
		}

		internal class OwnCollectionAnotherClassTestWithNoAttribute
		{
			public List<AnotherClassTest> List { get; set; }
		}

		internal class OwnCollectionOfBasicElement
		{
			[NotEmpty("List could not be empty")]
			public List<Int32> List { get; set; }

			[Required]
			public String Property { get; set; }
		}

		internal class OwnCollectionOfBasicElementIEnumerable
		{
			public IEnumerable<int> SearchUnits
			{
				get
				{
					foreach (var i in list)
					{
						yield return i;
					}
				}

			}

			private List<Int32> list { get; set; }

			public OwnCollectionOfBasicElementIEnumerable(params Int32[] values)
			{
				list = new List<int>();
				list.AddRange(values);
			}
		}

		internal class OwnSecondLevel
		{
			public List<OwnCollectionOfBasicElementIEnumerable> Collection { get; set; }
		}

		#endregion

		[Test]
		public void VerifyValidateRecursiveObject()
		{
			var rng = new OwnAClassTest() { Reference = new ClassTest(), Reference2 = new ClassTest() };
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
			var rng = new OwnAnotherClassTest() { Reference = new AnotherClassTest() };
			var v = new Core.Validator();

			ValidationResult res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors, Has.Count.EqualTo(2));
		}

		[Test]
		public void VerifyCollectionValidation()
		{
			var rng = new OwnCollectionAnotherClassTest() { List = new List<AnotherClassTest>() };
			rng.List.Add(new AnotherClassTest() { Property1 = "bla", Property2 = "BEA" });
			rng.List.Add(new AnotherClassTest() { Property1 = "bla" }); //this is invalid
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res);
		}

		[Test]
		public void VerifyCollectionOfInt32DoesNotValidateInnerInteger()
		{
			var rng = new OwnCollectionOfBasicElement() { List = new List<Int32>(), Property = "TEST" };
			rng.List.Add(11);
			rng.List.Add(12);
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}	
		
		[Test]
		public void VerifyNotValidateIEnumerableProperties()
		{
			var rng = new OwnCollectionOfBasicElementIEnumerable() { };
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}

		[Test]
		public void VerifyNotValidateIEnumerableWhenLambdaConditionIsUsed()
		{
			var rng = new OwnCollectionOfBasicElementIEnumerable(23) { };
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>(l => l.SearchUnits)
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0).Message("error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}
		
		[Test]
		public void VerifyInnerIEnuerablePropertyOfInnerObjectIsNotValidated()
		{
			var rng = new OwnSecondLevel() { Collection = new List<OwnCollectionOfBasicElementIEnumerable>()};
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable(1));
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable());
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>(l => l.SearchUnits)
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0)
						  .Message("Error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res); //Second list does not contains elements
		}

		[Test]
		public void VerifyInnerIEnuerablePropertyCorrectError()
		{
			var rng = new OwnSecondLevel() { Collection = new List<OwnCollectionOfBasicElementIEnumerable>() };
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable(1));
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable());
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>(l => l.SearchUnits)
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0)
						  .Message("Error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Collection[1].SearchUnits")); //Second list does not contains elements
		}

		[Test]
		public void VerifyInnerIEnuerablePropertyPropertyValidation()
		{
			var rng = new OwnSecondLevel() { Collection = new List<OwnCollectionOfBasicElementIEnumerable>() };
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable(1));
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable());
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>()
				.OnMember("SearchUnits")
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0)
						  .Message("Error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Collection[1].SearchUnits")); //Second list does not contains elements
		}

		[Test]
		public void VerifyInnerIEnuerablePropertyWithExpressionDefinition()
		{
			var rng = new OwnSecondLevel() { Collection = new List<OwnCollectionOfBasicElementIEnumerable>() };
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable(1));
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable());
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>()
				.OnMember < OwnCollectionOfBasicElementIEnumerable>(l => l.SearchUnits)
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0)
						  .Message("Error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Collection[1].SearchUnits")); //Second list does not contains elements
		}

		[Test]
		public void VerifyInnerValidationGrabCorrectNameWhenPassedAsArgument()
		{
			var rng = new OwnSecondLevel() { Collection = new List<OwnCollectionOfBasicElementIEnumerable>() };
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable(1));
			rng.Collection.Add(new OwnCollectionOfBasicElementIEnumerable());
			var v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionOfBasicElementIEnumerable>(l => l.SearchUnits, "SearchUnitsXX")
				 .Custom<IEnumerable<Int32>>(sl =>
						  sl.Count() > 0)
						  .Message("Error"));
			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("Collection[1].SearchUnitsXX")); //Second list does not contains elements
		}	
	

		[Test]
		public void VerifyCollectionOfInt32ValidateNotEmpty()
		{
			var rng = new OwnCollectionOfBasicElement() { List = new List<Int32>(), Property = "TEST" };
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res);
		}

		[Test]
		public void VerifyCollectionNotEmptyValidation()
		{
			var rng = new OwnCollectionAnotherClassTest() { List = new List<AnotherClassTest>() };
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsFalse(res);
		}

		[Test]
		public void VerifyCollectionNotEmptyValidationFluent()
		{
			var rng = new OwnCollectionAnotherClassTestWithNoAttribute() { List = new List<AnotherClassTest>() };
			Core.Validator v = new Core.Validator();
			v.AddRule(Rule.For<OwnCollectionAnotherClassTestWithNoAttribute>()
					.OnMember("List")
					.NotEmpty()
					.Message("List should have at least one element"));
			ValidationResult res = v.ValidateObject(rng);
			Assert.IsFalse(res);
			Assert.That(res.ErrorMessages, Has.Count.EqualTo(1));
			Assert.That(res.ErrorMessages[0],
				Is.EqualTo("List should have at least one element"));

		}

		[Test]
		public void VerifyCollectionWhenNoRuleIsDefined()
		{
			var rng = new OwnCollectionAnotherClassTestWithNoAttribute() { List = new List<AnotherClassTest>() };
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.IsTrue(res);
		}

		[Test]
		public void VerifyCollectionValidationIndexingOfPropertyInErrorList()
		{
			var rng = new OwnCollectionAnotherClassTest() { List = new List<AnotherClassTest>() };
			rng.List.Add(new AnotherClassTest() { Property1 = "bla", Property2 = "BEA" });
			rng.List.Add(new AnotherClassTest() { Property1 = "bla" }); //this is invalid
			var v = new Core.Validator();

			var res = v.ValidateObject(rng, ValidationFlags.RecursiveValidation);
			Assert.That(res.Errors[0].SourceName, Is.EqualTo("List[1].Property2"));
		}
	}
}
