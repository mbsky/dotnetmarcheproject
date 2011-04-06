using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Attributes;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;
using Rhino.Mocks;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class RegExFixture
	{
		private class Test
		{
			[Regex("Test.*good", "Error")]
			public String Property { get; set; }
		}

		[Test]
		public void TestBasicValidation()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(m => m.SourceName).Return("Property");
			ive.Expect(m => m.ExtractValue(null))
				.IgnoreArguments().Return("Testregex");
			var rv = new RegexValidator(ive, "Test.*good");
			Assert.That((Boolean) rv.Validate(null), Is.False);
		}

		[Test]
		public void TestBasicValidationGood()
		{
			IValueExtractor ive = MockRepository.GenerateStub<IValueExtractor>();
			ive.Expect(m => m.SourceName).Return("Property");
			ive.Expect(m => m.ExtractValue(null))
				.IgnoreArguments().Return("Testregexgood");
			var rv = new RegexValidator(ive, "Test.*good");
			Assert.That((Boolean) rv.Validate(null), Is.True);
		}

		[Test]
		public void TestBasicValidationAttribute()
		{
			Test test = new Test();
			test.Property = "Testregex";
			Validator .Core.Validator v = new Core.Validator();
			Assert.That((Boolean)v.ValidateObject(test), Is.False);
		}

		[Test]
		public void TestBasicValidationAttributeGood()
		{
			Test test = new Test();
			test.Property = "Testregexgood";
			Validator.Core.Validator v = new Core.Validator();
			Assert.That((Boolean)v.ValidateObject(test), Is.True);
		}
	}
}
