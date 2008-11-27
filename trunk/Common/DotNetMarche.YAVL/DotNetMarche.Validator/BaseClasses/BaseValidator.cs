using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.BaseClasses
{
	public abstract class BaseValidator : IValidator {

		public abstract SingleValidationResult Validate(object objectToValidate);

		protected BaseValidator(IValueExtractor valueExtractor) {
			mValueExtractor = valueExtractor;
		}
		protected IValueExtractor mValueExtractor;
			
	}
}