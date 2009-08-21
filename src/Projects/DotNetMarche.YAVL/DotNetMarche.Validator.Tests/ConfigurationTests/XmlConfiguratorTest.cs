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
			Assert.That(rules, Has.Count.EqualTo(2));
		}

		[Test]
		public void TestSimpleConfigurationFull()
		{
			XmlConfigurator sut = new XmlConfigurator("ConfigurationTests/SampleXmlFiles/ConfigurationA.xml");
			var validator = sut.CreateValidator();
			var rules = validator.GetRules(typeof(BaseValidatorFixture.Simple1FieldWithoutAttribute));
			Assert.That(rules[0], Is.InstanceOf<RequiredValidator>());
			Assert.That(rules[1], Is.InstanceOf<RangeValueValidator>());
		}
	}
}
