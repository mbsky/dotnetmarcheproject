using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Core.InnerValidators
{
	public class ObjectValidator : IMultipleValidator
	{

		internal ObjectValidator(
			IValueExtractor valueExtractor,
			Dictionary<Type, ValidationUnitCollection> ruleMap)
		{

			mValueExtractor = valueExtractor;
			mRuleMap = ruleMap;

		}

		protected IValueExtractor mValueExtractor;
		protected Dictionary<Type, ValidationUnitCollection> mRuleMap;

		/// <summary>
		/// Validate an object
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <param name="validationFlags"></param>
		/// <returns></returns>
		public IEnumerable<SingleValidationResult> Validate(object objectToValidate, ValidationFlags validationFlags)
		{
			//First of all retrieve the object, if it is null validate.
			object obj = mValueExtractor.ExtractValue(objectToValidate);
			if (obj != null)
			{
				//Check if this object support a validation, if we do not have a rule the object should
				//be considered valid.
				ValidationUnitCollection vc = mRuleMap[obj.GetType()];
				if (vc.Count > 0)
				{
					ValidationResult res = new ValidationResult();

					vc.ValidateObject(res, obj, validationFlags);
					if (!res)
					{
						foreach (ValidationError validationError in res.Errors)
						{
							yield return new SingleValidationResult(
								false, validationError.Message, "", mValueExtractor.SourceName + "." + validationError.SourceName);
						}
					}
				}
			}
		}
	}
}