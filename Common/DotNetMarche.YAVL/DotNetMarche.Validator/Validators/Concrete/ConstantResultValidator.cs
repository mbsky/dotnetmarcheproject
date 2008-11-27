using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.Validators.Concrete
{
	/// <summary>
	/// this validator is used only for internal test, it returns always true or false.
	/// </summary>
	public class ConstantResultValidator : BaseValidator  {

		public Boolean mResult;

		public ConstantResultValidator(Boolean result) : base(new ObjectValueExtractor()) {
			mResult = result;
		}

		public override SingleValidationResult Validate(object objectToValidate) {
			if (mResult)
				return SingleValidationResult.GenericSuccess;
			else
				return SingleValidationResult.GenericError;
		}
	

	}
}