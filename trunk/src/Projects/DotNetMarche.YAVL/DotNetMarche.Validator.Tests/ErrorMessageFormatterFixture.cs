using System;
using System.Globalization;
using System.Threading;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class ErrorMessageFormatterFixture
	{
		[Test]
		public void TestBasicResourceManager()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("En-us");
			var ra = new RequiredAttribute("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests", null);
			String msg = ErrorMessageFormatter.instance.FormatMessage(ra.CreateErrorMessage(),
			                                                          SingleValidationResult.GenericError);
			Assert.AreEqual("This is a test message", msg);
		}

		[Test]
		public void TestBasicResourceManagerExplicitLocalization()
		{
			var ra = new RequiredAttribute("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests", null);
			String msg = ErrorMessageFormatter.instance.FormatMessage(ra.CreateErrorMessage(),
			                                                          SingleValidationResult.GenericError,
			                                                          new CultureInfo("IT-it"));
			Assert.AreEqual("Questa è una stringa di test", msg, "Error message get no localized");
		}

		[Test]
		public void TestBasicResourceManagerLocalization()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("IT-it");
			var ra = new RequiredAttribute("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests", null);
			String msg = ErrorMessageFormatter.instance.FormatMessage(ra.CreateErrorMessage(),
			                                                          SingleValidationResult.GenericError);
			Assert.AreEqual("Questa è una stringa di test", msg, "Error message get no localized");
		}

		[Test]
		public void TestBasicResourceManagerLocalizationCultureResourceNotFound()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("Ru-ru");
			var ra = new RequiredAttribute("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests", null);
			String msg = ErrorMessageFormatter.instance.FormatMessage(ra.CreateErrorMessage(),
			                                                          SingleValidationResult.GenericError);
			Assert.AreEqual("This is a test message", msg, "Error message get no localized");
		}

		[Test]
		public void TestBasicResourceManagerWithNullExpectedAndActual()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("En-us");
			ErrorMessage msg = new ErrorMessage("Expected ${ExpectedValue} Actual ${ActualValue}");
			msg.ToString(new SingleValidationResult(false, null, null));
		}
	}
}