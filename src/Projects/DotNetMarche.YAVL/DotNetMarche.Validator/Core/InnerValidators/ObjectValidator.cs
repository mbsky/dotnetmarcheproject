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
				List<SingleValidationResult> results = new List<SingleValidationResult>();
				if (obj is IEnumerable)
				{
					//the object is IEnumerable, we need to validate inner objects
					Int32 index = 0;

					foreach (var innerObj in obj as IEnumerable)
					{
						var errors = ValidateSingleObject(innerObj, validationFlags);
						foreach (var validationError in errors)
						{
							results.Add(new SingleValidationResult(
									false, validationError.Message, "", string.Format("{0}[{1}].{2}",
									mValueExtractor.SourceName, index, validationError.SourceName)));

						}
						index++;
					}
				}
				else
				{
					//Check if this object support a validation, if we do not have a rule the object should
					//be considered valid.
					var errors = ValidateSingleObject(obj, validationFlags);
					foreach (var validationError in errors)
					{
						results.Add(new SingleValidationResult(
													false, validationError.Message, "", mValueExtractor.SourceName + "." + validationError.SourceName));

					}
				}
				return results;


			}

			return new SingleValidationResult[] { };
		}

		protected IEnumerable<ValidationError> ValidateSingleObject(object obj, ValidationFlags validationFlags)
		{
			Type type = obj.GetType();
			if (mRuleMap.ContainsKey(type))
			{
				ValidationUnitCollection vc = mRuleMap[type];
				if (vc.Count > 0)
				{
					ValidationResult res = new ValidationResult();

					vc.ValidateObject(res, obj, validationFlags);
					if (!res)
					{
						return res.Errors;
					}
				}
			}

			return new ValidationError[] { };

		}
	}
}