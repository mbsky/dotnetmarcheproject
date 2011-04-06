using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class ValidationResultFixture
	{
		[Test]
		public void ConversionToBoolFalse()
		{
			var vr = new ValidationResult(false);
			Assert.IsFalse(vr, "Validation Result does not convert well to Boolean");
		}

		[Test]
		public void ConversionToBoolTrue()
		{
			var vr = new ValidationResult();
			Assert.IsTrue(vr, "Validation Result does not convert well to Boolean");
		}

        [Test]
        public void VerifyValidationExceptionMessage()
        {
            var ve = new ValidationException(
                new ValidationError("ERROR1"),
                new ValidationError("ERROR2"));
            Assert.That(ve.Message, Is.EqualTo("ERROR1\nERROR2"));
        }
	}
}