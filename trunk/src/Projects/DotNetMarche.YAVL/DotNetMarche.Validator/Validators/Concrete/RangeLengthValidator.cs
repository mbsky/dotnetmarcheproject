using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	/// <summary>
	/// used for string, to give a minimum and maximum lenght to the string
	/// </summary>
	public class RangeLengthValidator : BaseValidator
	{
		public Int32 MinLenght { get; set; }
		public Int32 MaxLength { get; set; }

		public RangeLengthValidator(IValueExtractor valueExtractor, int minLenght, int maxLength) : base(valueExtractor)
		{
			MinLenght = minLenght;
			MaxLength = maxLength;
		}

		public override SingleValidationResult Validate(object objectToValidate)
		{
			Object  valueToCheck = mValueExtractor.ExtractValue(objectToValidate);
			if (valueToCheck == null) return CreateErrrorReturnValue("null");
			String s = valueToCheck as string;
			if (s == null) throw new ArgumentException(String.Format("The extractor {0} extract a value that is not string.", mValueExtractor));
			if (s.Length >= MinLenght && s.Length <= MaxLength)
				return SingleValidationResult.GenericSuccess;
			else
				return CreateErrrorReturnValue(s);
		}

		private SingleValidationResult CreateErrrorReturnValue(string actualValue)
		{
			return new SingleValidationResult(
				false, 
				String.Format("String of lenght [{0},{1}]", MinLenght, MaxLength),
				actualValue);
		}
	}
}
