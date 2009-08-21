using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Configuration.Xml;
using DotNetMarche.Validator.Validators.Concrete;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests.ConfigurationTests
{
	[TestFixture]
	public class XmlConfiguratorTest
	{
		[Test]
		public void SmokeCanConfigure()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationA.xml");
		}

		[Test]
		public void TestSimpleConfiguration()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationA.xml");
			var validator = sut.CreateValidator();
			var rules = validator.GetRules(typeof (BaseValidatorFixture.Simple1FieldWithoutAttribute));
			Assert.That(rules, Has.Count.EqualTo(3));
		} 
 
		[Test]
		public void TestSimpleConfigurationFull()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationA.xml");
			var validator = sut.CreateValidator();
			var rules = validator.GetRules(typeof(BaseValidatorFixture.Simple1FieldWithoutAttribute));
			Assert.That(rules.Count, Is.EqualTo(3));
			Assert.That(rules[0].Validator, Is.InstanceOf<RequiredValidator>());
			Assert.That(rules[1].Validator, Is.InstanceOf<RangeLengthValidator>());
			Assert.That(rules[2].Validator, Is.InstanceOf<RangeValueValidator>());
		}

		[Test]
		public void TestSimpleConfigurationFullValidate()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationA.xml");
			var validator = sut.CreateValidator();
			BaseValidatorFixture.Simple1FieldWithoutAttribute obj = new BaseValidatorFixture.Simple1FieldWithoutAttribute();
			obj.field = "This string exceeds 10 chars and is not valid.";
			ValidationResult res = validator.ValidateObject(obj);
			Assert.That(res.Success, Is.False);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("Too Lenght"));
		}

		[Test]
		public void TestLocalizationOfMessages()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationB.xml");
			BaseValidatorFixture.Simple1FieldWithoutAttribute obj = new BaseValidatorFixture.Simple1FieldWithoutAttribute();
			obj.field = "This string exceeds 10 chars and is not valid.";
			ValidationResult res = sut.CreateValidator().ValidateObject(obj);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("Field is too long"));

		}
	}
}
