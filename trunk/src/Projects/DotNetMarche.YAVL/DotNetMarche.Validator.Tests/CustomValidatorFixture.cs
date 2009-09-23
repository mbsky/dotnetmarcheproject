using System;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Tests.Utils;
using DotNetMarche.Validator.Validators;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;
using NUnit.Framework;
using Rhino.Mocks;

namespace ValidatorTest
{
	[TestFixture]
	public class CustomValidatorFixture
	{
		
		[Test]
		public void TestBasicValidationGood()
		{
			var newObject = new object();
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Stub(obj => obj.ExtractValue(null)).IgnoreArguments().Return(20);
			var rv = new ActionValidation<Int32>(ive, num => num > 10);
			Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
		}

		[Test]
		public void TestBasicValidationWrong()
		{
			var newObject = new object();
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Stub(obj => obj.ExtractValue(null)).IgnoreArguments().Return(4);
			var rv = new ActionValidation<Int32>(ive, num => num > 10);
			Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		}

		[Test]
		public void TestFullValidator()
		{
			Int32 value = 20;
			ActionValidation<Int32> validation = new ActionValidation<int>(new ObjectValueExtractor(), i => i > 40);

			var result = validation.Validate(value);
			Assert.That(result.Success, Is.False);
		}

		[Test]
		public void TestPutActualValueInRealMessage()
		{
			Int32 value = 20;
			Validator v = new Validator();
			v.AddRule(Rule.For<Int32>(i => i)
				.Message("Expected ${ExpectedValue} Actual ${ActualValue}")
			   .Custom<Int32>(i => i > 30));

			var result = v.ValidateObject(value);
			Assert.That(result.ErrorMessages[0], Is.EqualTo("Expected Custom function Actual 20"));
		}

		//[Test]
		//public void TestBasicRangeConversionFromCurrency()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockUtils.CreateExtractor(repository, 10M);
		//   var rv = new RangeValueValidator(ive, 0, 100);
		//   Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
		//   repository.VerifyAll();
		//}

		//[Test]
		//public void TestBasicRangeGood()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockUtils.CreateExtractor(repository, 3.5d);
		//   var rv = new RangeValueValidator(ive, 0, 100);
		//   Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
		//   repository.VerifyAll();
		//}

		//[Test]
		//public void TestBasicRangeTooHigh()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockUtils.CreateExtractor(repository, 110.0f);
		//   var rv = new RangeValueValidator(ive, 0, 100);
		//   Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		//   repository.VerifyAll();
		//}

		//[Test]
		//public void TestBasicRangeTooLow()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockUtils.CreateExtractor(repository, -10.0f);
		//   var rv = new RangeValueValidator(ive, 0, 100);
		//   Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		//   repository.VerifyAll();
		//}

		//[Test]
		//public void TestRangeLenght()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
		//   ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return("Valid");
		//   var rv = new RangeLengthValidator(ive, 100);
		//   Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
		//}

		//[Test]
		//public void TestRangeLenghtWring()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
		//   ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return("This is too length exeeds 10 chars");
		//   var rv = new RangeLengthValidator(ive, 10);
		//   Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		//}


		//[Test]
		//public void TestRangeNullReturnedNull()
		//{
		//   var newObject = new object();
		//   IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
		//   ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return(null);
		//   var rv = new RangeLengthValidator(ive, 10);
		//   Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		//}
	}
}