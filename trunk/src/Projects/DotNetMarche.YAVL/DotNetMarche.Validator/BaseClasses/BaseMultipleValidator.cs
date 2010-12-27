using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.BaseClasses
{
	public abstract class BaseMultipleValidator : IMultipleValidator {

		protected BaseMultipleValidator(IValueExtractor valueExtractor)
		{
			mValueExtractor = valueExtractor;
		}
		protected IValueExtractor mValueExtractor;
			
		protected T Extract<T>(Object obj)
		{
			return (T) mValueExtractor.ExtractValue(obj);
		}

		public abstract IEnumerable<SingleValidationResult> Validate(object objectToValidate, ValidationFlags validationFlags);
	}
}