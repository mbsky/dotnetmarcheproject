using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Core.InnerValidators
{
	public class ObjectValidator : BaseMultipleValidator
	{

		internal ObjectValidator(
			IValueExtractor valueExtractor,
			Dictionary<Type, ValidationUnitCollection> ruleMap)
			: base(valueExtractor)
		{

			mValueExtractor = valueExtractor;
			mRuleMap = ruleMap;

		}
		protected Dictionary<Type, ValidationUnitCollection> mRuleMap;

		/// <summary>
		/// Validate an object
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <param name="validationFlags"></param>
		/// <returns></returns>
		public override IEnumerable<SingleValidationResult>
			Validate(object objectToValidate, ValidationFlags validationFlags)
		{
			//First of all retrieve the object, if it is null validate.
			object obj = Extract<Object>(objectToValidate);
			if (obj != null)
			{
				if (obj is IEnumerable)
				{
					//the object is IEnumerable, we need to validate inner objects
					Int32 index = 0;
					List<SingleValidationResult> results = new List<SingleValidationResult>();
					foreach (var innerObj in obj as IEnumerable)
					{
						var partialRet = ValidateSingleObject(innerObj, validationFlags);
						foreach (var singleValidationResult in partialRet)
						{
							results.Add(singleValidationResult);
						}
						index++;
					}
					return results;
				}
		
					//Check if this object support a validation, if we do not have a rule the object should
					//be considered valid.
					return ValidateSingleObject(obj, validationFlags);
			}

			return new SingleValidationResult[] {};
		}

		protected IEnumerable<SingleValidationResult> ValidateSingleObject(object obj, ValidationFlags validationFlags)
		{
			ValidationUnitCollection vc = mRuleMap[obj.GetType()];
			if (vc.Count > 0)
			{
				ValidationResult res = new ValidationResult();

				vc.ValidateObject(res, obj, validationFlags);
				if (!res)
				{
					List<SingleValidationResult> retValue = new List<SingleValidationResult>();
					foreach (ValidationError validationError in res.Errors)
					{
						retValue.Add(new SingleValidationResult(
											false, validationError.Message, "", mValueExtractor.SourceName + "." + validationError.SourceName));
					}
					return retValue;
				}
			}
			return new SingleValidationResult[] { };
			
		}
	}
}