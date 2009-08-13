using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Validators;
using DotNetMarche.Validator.Validators.Attributes;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests
{
	[TestFixture]
	public class InheritanceTest
	{
		[Test]
		public void ScannerInheritedTest()
		{
			var v = new Core.Validator();
			ChildEntity bes = new ChildEntity();
			var ts = new TypeScanner(bes.GetType());
			ValidationUnitCollection vc = ts.Scan();
			Assert.That(vc, Has.Count.EqualTo(2));
			Assert.That(vc[1].ErrorMessage.Message, Is.EqualTo("This is a required field"));
		}

		[Test]
		public void ValidateInherited()
		{
			var v = new Core.Validator();
			BaseEntity bes = new BaseEntity();
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.False);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("This is a required field"));
		}

		public void ValidateUnknownEntity()
		{
			var v = new Core.Validator();
			AnEntity bes = new AnEntity();
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.True);
		}

		[Test]
		public void ValidateInherited2()
		{
			var v = new Core.Validator();
			ChildEntity bes = new ChildEntity();
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.False);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("This is a required field"));
		}		
		
		[Test]
		public void ValidateInheritedGood()
		{
			var v = new Core.Validator();
			ChildEntity bes = new ChildEntity();
			bes.RequiredString = "this";
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.True);
		}

		[Test]
		public void ValidateInheritedFluent()
		{
			var v = new Core.Validator();
			ChildEntityNa bes = new ChildEntityNa();
			v.AddRule(Rule.For<ChildEntityNa>(e => e.RequiredString)
			          	.Required.Message("Required!!!!"));
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.False);
			Assert.That(res.ErrorMessages[0], Is.EqualTo("Required!!!!"));
		}

		[Test]
		public void ValidateInheritedFluentTrue()
		{
			var v = new Core.Validator();
			ChildEntityNa bes = new ChildEntityNa();
			v.AddRule(Rule.For<ChildEntityNa>(e => e.RequiredString)
						.Required.Message("Required!!!!"));
			bes.RequiredString = "is here!!";
			ValidationResult res = v.ValidateObject(bes);
			Assert.That(res.Success, Is.True);
		}
	}

	public class AnEntity
	{
		public String RequiredString { get; set; }
	}

	public class BaseEntity
	{
		[Required("This is a required field")]
		public String RequiredString { get; set; }
	}

	public class ChildEntity : BaseEntity
	{
		[RangeLength(100)]
		public String Message { get; set; }

		public ChildEntity()
		{
			Message = "";
		}
	}

	public class BaseEntityNa
	{
		
		public String RequiredString { get; set; }
	}

	public class ChildEntityNa : BaseEntityNa
	{
		
		public String Message { get; set; }
	}
}
