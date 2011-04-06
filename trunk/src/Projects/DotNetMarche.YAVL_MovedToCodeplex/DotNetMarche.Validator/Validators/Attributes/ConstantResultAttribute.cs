using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	/// <summary>
	/// This attribute is used to generate a test attribute that always fail or pass
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class ConstantResultAttribute : BaseValidationAttribute {

		private Boolean mResult;

		public ConstantResultAttribute(
			String errorMessage,
			Boolean result) : base(errorMessage) {mResult = result;} 

		public override IValidator CreateValidator(IValueExtractor valueExtractor) {
			return new ConstantResultValidator(mResult);
		}
	}
}