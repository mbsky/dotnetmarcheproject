using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Tests.ResourcesFiles;
using DotNetMarche.Validator.ValueExtractors;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class VariousEntitiesTest
	{

		[Test]
		public void ValidationResultConvertToBoolean()
		{
			Assert.IsTrue(SingleValidationResult.GenericSuccess);
			Assert.IsFalse(SingleValidationResult.GenericError);
		}

		[Test]
		public void TestLocalizationIt()
		{
			ErrorMessage sut =
				new ErrorMessage("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests");
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("It-It")), Is.EqualTo("Questa è una stringa di test"));
		}

		[Test]
		public void TestLocalizationDe()
		{
			ErrorMessage sut =
				new ErrorMessage("Test", "DotNetMarche.Validator.Tests.ResourcesFiles.TestRes, DotNetMarche.Validator.Tests");
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("De")), Is.EqualTo("German Value"));
		}

		[Test]
		public void TestLocalizationItProperty()
		{
			ErrorMessage sut =
				new ErrorMessage("Test", TestRes.ResourceManager);
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("It-It")), Is.EqualTo("Questa è una stringa di test"));
		}

		[Test]
		public void TestLocalizationDeProperty()
		{
			ErrorMessage sut =
				new ErrorMessage("Test", TestRes.ResourceManager);
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("De")), Is.EqualTo("German Value"));
		}

		[Test]
		public void TestLocalizationItFullFluent()
		{
			ErrorMessage sut =new ErrorMessage(() => TestRes.Test);
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("It-It");
			Assert.That(sut.ToString(), Is.EqualTo("Questa è una stringa di test"));
		}

		[Test]
		public void TestLocalizationItFullFluentDe()
		{
			ErrorMessage sut =new ErrorMessage(() => TestRes.Test);
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("De");
			Assert.That(sut.ToString(), Is.EqualTo("German Value"));
		}

[Test]
public void TestLocalizationItFullFluentExplicitCulture()
{
	ErrorMessage sut = new ErrorMessage(() => TestRes.Test);
	Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("En-Us");
	Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("It")), Is.EqualTo("Questa è una stringa di test"));
}

		[Test]
		public void TestLocalizationItFullFluentExplicitCultureDe()
		{
			ErrorMessage sut = new ErrorMessage(() => TestRes.Test);
			Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("En-Us");
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("De")), Is.EqualTo("German Value"));
		}

		[Test]
		public void TestMessageWithResult()
		{
			ErrorMessage sut = new ErrorMessage("Expected " + ErrorMessage.sTokenExpectedValue + " actual " + ErrorMessage.sTokenActualValue);
			SingleValidationResult res = new SingleValidationResult(false, "Exp", "Act");
			Assert.That(sut.ToString(res),
				Is.EqualTo("Expected " + "Exp" + " actual " + "Act"));
		}

		[Test]
		public void TestLocalizationWithResultLocalized()
		{
			ErrorMessage sut = new ErrorMessage(() => TestRes.TestToken);
			SingleValidationResult res = new SingleValidationResult(false, "Exp", "Act");
			Assert.That(sut.ToString(res),
				Is.EqualTo("Expected " + "Exp" + " Actual " + "Act"));
		}

		[Test]
		public void TestLocalizationWithResultLocalizedIt()
		{
			ErrorMessage sut = new ErrorMessage(() => TestRes.TestToken);
			SingleValidationResult res = new SingleValidationResult(false, "Exp", "Act");
			Assert.That(sut.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("It"), res),
				Is.EqualTo("Atteso " + "Exp" + " Attuale " + "Act"));
		}

		[Test]
		public void TestObjectValueExtractor()
		{
ObjectValueExtractor sut = new ObjectValueExtractor();
			Assert.That(sut.ExtractValue(sut), Is.EqualTo(sut));
		}
	}
}
