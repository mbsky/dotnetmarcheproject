using System;
using System.Collections;
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
	public class NotEmptyValidator : BaseValidator
	{
		public NotEmptyValidator(IValueExtractor valueExtractor)
			: base(valueExtractor)
		{

		}

		public override SingleValidationResult Validate(object objectToValidate)
		{
			Object valueToCheck = mValueExtractor.ExtractValue(objectToValidate);
	
			if (valueToCheck is IList)
			{
				IList value = valueToCheck as IList;
				if (value.Count == 0)
				{
					return new SingleValidationResult(
							false,
							String.Format("Collection is empty"),
							valueToCheck,
							mValueExtractor.SourceName);
				}
			} else if (valueToCheck is IEnumerable)
			{
				IEnumerable value = valueToCheck as IEnumerable;
				if (value.Cast<Object>().Count() == 0)
				{
					return new SingleValidationResult(
							false,
							String.Format("Collection is empty"),
							valueToCheck,
							mValueExtractor.SourceName);
				}
			}

			return SingleValidationResult.GenericSuccess;
		}
	}
}
