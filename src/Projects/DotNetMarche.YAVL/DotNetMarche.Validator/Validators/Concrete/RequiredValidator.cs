using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	/// <summary>
	/// Mark a field/property that is required.
	/// </summary>
	public class RequiredValidator : BaseValidator {

		private object mNullValue;
		public RequiredValidator(IValueExtractor valueExtractor, object nullValue)
			: base(valueExtractor) {

			mNullValue = nullValue;
		}

		public RequiredValidator(IValueExtractor valueExtractor) : this(valueExtractor, null) {}

		public override SingleValidationResult Validate(object objectToValidate) {
			object valueToCheck = mValueExtractor.ExtractValue(objectToValidate);
			Boolean result = IsValueValid(valueToCheck);
			if (result)
				return SingleValidationResult.GenericSuccess;
			else 
				return SingleValidationResult.GenericError;
		}

		/// <summary>
		/// It does internal check
		/// </summary>
		/// <param name="valueToCheck"></param>
		/// <returns></returns>
		private Boolean IsValueValid(object valueToCheck) {
			if (valueToCheck == null) return false;
			return !object.Equals(valueToCheck, mNullValue);
		}

	}
}