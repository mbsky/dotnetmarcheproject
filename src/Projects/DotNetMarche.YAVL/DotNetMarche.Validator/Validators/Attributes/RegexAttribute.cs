using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class RegexAttribute : BaseValidationAttribute
	{
		public RegexAttribute(String regexExpression, string errorMessage, string resourceTypeName)
			: base(errorMessage, resourceTypeName)
		{
			RegexExpression = regexExpression;
		}

		public String RegexExpression { get; set; }

		public RegexAttribute(String regexExpression, string errorMessage) : this(regexExpression, errorMessage, null)
		{
		}

		public override DotNetMarche.Validator.Interfaces.IValidator CreateValidator(DotNetMarche.Validator.Interfaces.IValueExtractor valueExtractor)
		{
			return new RegexValidator(valueExtractor, RegexExpression);
		}
	}
}
