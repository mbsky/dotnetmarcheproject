using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Tests.Utils;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;
using Rhino.Mocks;

namespace ValidatorTest
{
	[TestFixture]
	public class RangeValidatorFixture
	{
		private MockRepository repository;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			repository = new MockRepository();
		}

		[Test]
		public void TestBasicRangeConversion()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractor(repository, "10");
			var rv = new RangeValueValidator(ive, 0, 100);
			Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
			repository.VerifyAll();
		}

		[Test]
		public void TestBasicRangeConversionFromCurrency()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractor(repository, 10M);
			var rv = new RangeValueValidator(ive, 0, 100);
			Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
			repository.VerifyAll();
		}

		[Test]
		public void TestBasicRangeGood()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractor(repository, 3.5d);
			var rv = new RangeValueValidator(ive, 0, 100);
			Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
			repository.VerifyAll();
		}

		[Test]
		public void TestBasicRangeTooHigh()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractorForBasicRange(repository, 110.0f);
			var rv = new RangeValueValidator(ive, 0, 100);
			Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
			repository.VerifyAll();
		}

		[Test]
		public void TestBasicRangeTooLow()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractorForBasicRange(repository, -10.0f);
			var rv = new RangeValueValidator(ive, 0, 100);
			Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
			repository.VerifyAll();
		}

		[Test]
		public void TestRangeLenght()
		{
			var newObject = new object();
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return("Valid");
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 100);
			Assert.IsTrue(rv.Validate(newObject), "Range incorrect validation");
		}

		[Test]
		public void TestRangeLenghtWring()
		{
			var newObject = new object();
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return("This is too length exeeds 10 chars");
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 10);
			Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		}


		[Test]
		public void TestRangeNullReturnedNull()
		{
			var newObject = new object();
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).IgnoreArguments().Return(null);
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 10);
			Assert.IsFalse(rv.Validate(newObject), "Range incorrect validation");
		}
	}
}