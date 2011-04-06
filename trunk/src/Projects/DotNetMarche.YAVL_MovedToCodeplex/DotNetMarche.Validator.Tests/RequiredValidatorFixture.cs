using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Tests.Utils;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;
using Rhino.Mocks;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class RequiredValidatorFixture
	{
		private MockRepository repository;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			repository = new MockRepository();
		}

		[Test]
		public void TestNotNullValue()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, new object());
			var rv = new RequiredValidator(ive, null);
			Assert.IsTrue(rv.Validate(null), "Requried Validator does not check object difference from null");
			repository.VerifyAll();
		}

		[Test]
		public void TestNotNullValueInteger()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, 1);
			var rv = new RequiredValidator(ive, 0);
			Assert.IsTrue(rv.Validate(null), "Requried Validator does not recognize default value to be null");
			repository.VerifyAll();
		}

		[Test]
		public void TestNullValue()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, null);
			var rv = new RequiredValidator(ive, null);
			Assert.IsFalse(rv.Validate(null), "Requried Validator does not check null");
			repository.VerifyAll();
		}

		[Test]
		public void TestNullValueInteger()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, 1);
			var rv = new RequiredValidator(ive, 1);
			Assert.IsFalse(rv.Validate(null), "Requried Validator does not recognize default value to be null");
			repository.VerifyAll();
		}

		[Test]
		public void TestNullWithEmptyString()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, null);
			var rv = new RequiredValidator(ive, "");
			Assert.IsFalse(rv.Validate(null), "Requried Validator does not check for null when has a default value");
			repository.VerifyAll();
		}

		[Test]
		public void TestValueExtractorCorrectlyCalled()
		{
			var newObject = new object();
			IValueExtractor ive = MockUtils.CreateExtractor(repository, null, newObject);
			var rv = new RequiredValidator(ive, null);
			Assert.IsFalse(rv.Validate(newObject), "Requried Validator does not check null");
			repository.VerifyAll();
		}

		[Test]
		public void TestWithEmptyString()
		{
			IValueExtractor ive = MockUtils.CreateExtractor(repository, "");
			var rv = new RequiredValidator(ive, "");
			Assert.IsFalse(rv.Validate(null), "Requried Validator does not recognize \"\" as null");
			repository.VerifyAll();
		}
	}
}