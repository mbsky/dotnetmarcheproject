using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Configuration.Xml;
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
		}
	}
}
