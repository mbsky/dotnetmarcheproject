using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using DotNetMarche.Validator.Core.InnerValidators;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// This class contains basic definition of a validation unit. A validation unit is a class
	/// capable to validate a object with a rule providing basic formatting.
	/// </summary>
	public class ValidationUnit {

		#region static creation methods

		public static ValidationUnit CreateValidationUnit(
			ErrorMessage		message,
			IValidator			validator) {
		
			return new ValidationUnit(message, validator);
			}

		#endregion

		/// <summary>
		/// Create a validation unit to validate recursively an object
		/// </summary>
		/// <param name="valueExtractor"></param>
		/// <param name="partName"></param>
		/// <param name="ruleMap"></param>
		/// <returns></returns>
		public static ValidationUnit CreateObjectValidationUnit(
			IValueExtractor valueExtractor,
			String			 partName,
			Dictionary<Type, ValidationUnitCollection> ruleMap) {
			
			ValidationUnit vu = new ValidationUnit(
				new ErrorMessage("Field " + partName + " does not validate"),
				new ObjectValidator(valueExtractor, ruleMap));
			vu.mIsRecursive = true;
			return vu;
			}
		
		public ValidationUnit(
			ErrorMessage errorMessage,
			IValidator validator) {
			mErrorMessage = errorMessage;
			mValidator = validator;
			mIsRecursive = false;
			}

		/// <summary>
		/// This property told if this validation unit is added to do recursive validation.
		/// </summary>
		public Boolean IsRecursive {
			get { return mIsRecursive; }
		}
		private Boolean mIsRecursive;

		/// <summary>
		/// Base attribute that contains all base information on the validation.
		/// </summary>
		public ErrorMessage ErrorMessage {
			get { return mErrorMessage; }
		}
		private ErrorMessage mErrorMessage;

		/// <summary>
		/// The real validator associated with the validation attribute.
		/// </summary>
		public IValidator Validator {
			get { return mValidator; }
		}
		private IValidator mValidator;

		/// <summary>
		/// This method retrieve the error message if the validation fail
		/// </summary>
		/// <returns></returns>
		public String GetErrorMessage(
			SingleValidationResult	result,
			CultureInfo					cultureInfo) {
			return ErrorMessageFormatter.instance.FormatMessage(ErrorMessage, result, cultureInfo);
			}

		public virtual Boolean Validate(
			ValidationResult	actualResult,
			object				objToValidate,
			ValidationFlags	validationFlag,
			CultureInfo			cultureInfo) {

			SingleValidationResult res = mValidator.Validate(objToValidate);
			if (!res) {
				actualResult.Success = false;
				actualResult.AddErrorMessage(GetErrorMessage(res, cultureInfo));
			}
			return res;
			}
	}
}