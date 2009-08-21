using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Extras;
using NHibernate;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests.Extras.NHibernate
{
	[TestFixture]
	public class NhibernateExtrasTests
	{
		private ISessionFactory Factory = SessionHelper.CreateConfigurationForConfigFileName("Extras/Nhibernate/NhibernateConfig.xml");


		[Test]
		public void TestPropertyLength()
		{
			Core.Validator validator = ValidatorFromMetadata.GetValidatorFromSession(Factory);
			var rules = validator.GetRules(typeof(EntityBase));
			Assert.That(rules, Has.Count.EqualTo(1));
		}

		[Test]
		public void TestPropertyLength2()
		{
			Core.Validator validator = ValidatorFromMetadata.GetValidatorFromSession(Factory);
			EntityBase eb = new EntityBase() {MyName = new string('X', 51)};
			ValidationResult res = validator.ValidateObject(eb);
			Assert.That(res.Success, Is.False);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("Property MyName have a maximum length of 50"));
		} 
	}
}
