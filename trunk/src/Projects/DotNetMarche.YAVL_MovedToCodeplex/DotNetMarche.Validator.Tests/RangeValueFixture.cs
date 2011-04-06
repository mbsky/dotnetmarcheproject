using System;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Tests.Utils;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;
using Rhino.Mocks;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class RangeValueFixture
	{
		private MockRepository repository;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			repository = new MockRepository();
		}

		[Test]
		public void TestNullString()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return(null);
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 1, 10);
			Assert.IsFalse(rv.Validate(null));
		}

		[Test]
		public void TestInvalidString()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return("");
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 1, 10);
			SingleValidationResult res = rv.Validate(null);
			Assert.IsFalse(res);
			Assert.That(res.ExpectedValue, Text.Contains("[1,10]"));
		}

		[Test]
		public void TestValueThatIsNotAString()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return(new Object());
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 1, 10);
			Assert.Throws(typeof (ArgumentException), () => rv.Validate(null));
			
		}

		[Test]
		public void TestInvalidStringTooLength()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return("This string exceeds 10 charachters");
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 1, 10);
			SingleValidationResult res = rv.Validate(null);
			Assert.IsFalse(res);
			Assert.That(res.ExpectedValue, Text.Contains("[1,10]"));
		}

		[Test]
		public void TestValidString()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return("String OK");
			ive.Expect(o => o.SourceName).Return("property");
			var rv = new RangeLengthValidator(ive, 1, 10);
			SingleValidationResult res = rv.Validate(null);
			Assert.IsTrue(res);
		}
	}
}