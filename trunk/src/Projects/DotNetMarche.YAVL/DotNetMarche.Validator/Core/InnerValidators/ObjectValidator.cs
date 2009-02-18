using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Core.InnerValidators
{
	class ObjectValidator : IValidator {

		internal ObjectValidator( 
			IValueExtractor valueExtractor,
			Dictionary<Type, ValidationUnitCollection> ruleMap) {
		
			mValueExtractor = valueExtractor;
			mRuleMap = ruleMap;

			}

		protected IValueExtractor mValueExtractor;
		protected Dictionary<Type, ValidationUnitCollection> mRuleMap;

		/// <summary>
		/// Validate an object
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <returns></returns>
		public SingleValidationResult Validate(object objectToValidate) {
			//First of all retrieve the object, if it is null validate.
			object obj = mValueExtractor.ExtractValue(objectToValidate);
			if (obj == null) return SingleValidationResult.GenericSuccess; 
			return InnerValidate(obj);
		}

		public SingleValidationResult InnerValidate(object obj) {
			//Check if this object support a validation, if we do not have a rule the object should
			//be considered valid.
			ValidationUnitCollection vc = mRuleMap[obj.GetType()];
			if (vc.Count == 0) return SingleValidationResult.GenericSuccess; 
			ValidationResult res = new ValidationResult();
			vc.ValidateObject(res, obj, ValidationFlags.StopOnFirstError | ValidationFlags.RecursiveValidation);
			if (res) 
				return SingleValidationResult.GenericSuccess;
			
			return new SingleValidationResult(false, res.ErrorMessages[0], "");
		}
	}
}