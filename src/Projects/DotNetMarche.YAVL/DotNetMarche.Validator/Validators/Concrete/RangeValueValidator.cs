using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	public class RangeValueValidator : BaseValidator {

		Double mMinValue;
		Double mMaxValue;

		public RangeValueValidator(
			IValueExtractor	valueExtractor,
			Double				minValue,
			Double				maxValue)
			: base(valueExtractor) {

			mMinValue = minValue;
			mMaxValue = maxValue;
			}

		public override SingleValidationResult Validate(object objectToValidate) {
			object valueToCheck = mValueExtractor.ExtractValue(objectToValidate);
			Double parsedValueToCheck = ParseValue(valueToCheck);
			Boolean result = IsValueValid(parsedValueToCheck);
			if (result)
				return SingleValidationResult.GenericSuccess;
			else 
				return new SingleValidationResult(
					false, 
					String.Format("{0}-{1}", mMinValue,mMaxValue),
					valueToCheck);
		}

		private Double ParseValue(object value) {
			Double parsed;
			if (value.GetType() == typeof (Double))
				parsed = (Double) value;
			else 
				parsed = Convert.ToDouble(value);
			
			return parsed;
		}

		/// <summary>
		/// It does internal check
		/// </summary>
		/// <param name="valueToCheck"></param>
		/// <returns></returns>
		private Boolean IsValueValid(Double parsedValueToCheck) {
			return parsedValueToCheck >= mMinValue && parsedValueToCheck <= mMaxValue;  
		}

	}
}