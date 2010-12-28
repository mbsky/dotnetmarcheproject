using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class NotEmptyAttribute : BaseValidationAttribute
	{
		public NotEmptyAttribute(string errorMessage) : base(errorMessage)
		{
		}

		public NotEmptyAttribute(string errorMessage, string resourceTypeName) : base(errorMessage, resourceTypeName)
		{
		}

		public override DotNetMarche.Validator.Interfaces.IValidator CreateValidator(DotNetMarche.Validator.Interfaces.IValueExtractor valueExtractor)
		{
			return new NotEmptyValidator(valueExtractor);
		}
	}
}
