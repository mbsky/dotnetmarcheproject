using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class RangeLengthAttribute : BaseValidationAttribute
	{

		public RangeLengthAttribute(string errorMessage, string resourceTypeName, int minValue, int maxValue) : base(errorMessage, resourceTypeName)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public Int32 MinValue { get; set; }
		public Int32 MaxValue { get; set; }

		public RangeLengthAttribute(string errorMessage, int minValue, int maxValue) : base(errorMessage)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public RangeLengthAttribute(string errorMessage, int maxValue)
			: base(errorMessage)
		{
			MinValue = 0;
			MaxValue = maxValue;
		}

		public RangeLengthAttribute(int maxValue)
			: base("")
		{
			MinValue = 0;
			MaxValue = maxValue;
		}

		public override DotNetMarche.Validator.Interfaces.IValidator CreateValidator(DotNetMarche.Validator.Interfaces.IValueExtractor valueExtractor)
		{
			return new RangeLengthValidator(valueExtractor, MinValue, MaxValue);
		}
	}
}
