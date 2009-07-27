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
			var rv = new RangeLengthValidator(ive, 1, 10);
			Assert.IsFalse(rv.Validate(null));
		}

		[Test]
		public void TestInvalidString()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return("");
			var rv = new RangeLengthValidator(ive, 1, 10);
			SingleValidationResult res = rv.Validate(null);
			Assert.IsFalse(res);
			Assert.That(res.ExpectedValue, Text.Contains("[1,10]"));
		}

		[Test]
		public void TestInvalidStringTooLength()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(o => o.ExtractValue(null)).Return("This string exceeds 10 charachters");
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
			var rv = new RangeLengthValidator(ive, 1, 10);
			SingleValidationResult res = rv.Validate(null);
			Assert.IsTrue(res);
		}
	}
}