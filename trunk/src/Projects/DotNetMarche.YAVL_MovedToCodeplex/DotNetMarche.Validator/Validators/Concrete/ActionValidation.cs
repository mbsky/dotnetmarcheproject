using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	/// <summary>
	/// This represent a rule that validate with a custom action 
	/// expressed with a lambda or delegate.
	/// </summary>
	public class ActionValidation<T>  : BaseValidator  
	{
		private Func<T, Boolean> _validationFunction;
		public ActionValidation(IValueExtractor valueExtractor, Func<T, Boolean> validationFunction) : base(valueExtractor)
		{
			_validationFunction = validationFunction;
		}

		public override SingleValidationResult Validate(object objectToValidate)
		{
			object extractedValue = mValueExtractor.ExtractValue(objectToValidate);
			T valueToCheck = (T) extractedValue;
			if (_validationFunction(valueToCheck))
			{
				return SingleValidationResult.GenericSuccess;
			} else
			{
				return new SingleValidationResult(false, "Custom function", valueToCheck, mValueExtractor.SourceName);
			}
		}
	}
}
