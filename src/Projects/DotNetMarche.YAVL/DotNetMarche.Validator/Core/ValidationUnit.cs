using System;
using System.Collections.Generic;
using System.Linq;
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
	public abstract class ValidationUnit
	{

		#region static creation methods

		public static ValidationUnit CreateValidationUnit(
			ErrorMessage message,
			IValidator validator)
		{
			return CreateValidationUnit(message, validator, true);
		}

		public static ValidationUnit CreateValidationUnit(
			ErrorMessage message,
			IValidator validator,
			bool recursive)
		{

			return new SingleValidatorValidationUnit(true, message, validator);
		}

		public static ValidationUnit CreateValidationUnit(ErrorMessage message, IMultipleValidator validator, bool recursive)
		{
			return new MultipleValidatorValidationUnit(true, message, validator);
		}


		/// <summary>
		/// Create a validation unit to validate recursively an object, remember that an object
		/// validation unit is not recursive.
		/// </summary>
		/// <param name="valueExtractor"></param>
		/// <param name="partName"></param>
		/// <param name="ruleMap"></param>
		/// <param name="isRecursive"></param>
		/// <returns></returns>
		public static ValidationUnit CreateObjectValidationUnit(
			IValueExtractor valueExtractor,
			String partName,
			Dictionary<Type, ValidationUnitCollection> ruleMap)
		{
			return new MultipleValidatorValidationUnit(
				false, new ErrorMessage("Field " + partName + " does not validate"),
				new ObjectValidator(valueExtractor, ruleMap));

		}

		#endregion

		protected ValidationUnit(bool isFirstLevelValidationUnit, ErrorMessage errorMessage)
		{
			this._mIsFirstLevelValidationUnit = isFirstLevelValidationUnit;
			this._errorMessage = errorMessage;
		}


		/// <summary>
		/// This property told if this validation unit is added to do recursive validation.
		/// </summary>
		public Boolean IsFirstLevelValidationUnit
		{
			get { return _mIsFirstLevelValidationUnit; }
		}
		private Boolean _mIsFirstLevelValidationUnit;

		/// <summary>
		/// Base attribute that contains all base information on the validation.
		/// </summary>
		public ErrorMessage ErrorMessage
		{
			get { return _errorMessage; }
		}
		private ErrorMessage _errorMessage;



		/// <summary>
		/// This method retrieve the error message if the validation fail
		/// </summary>
		/// <returns></returns>
		public String GetErrorMessage(
			SingleValidationResult result,
			CultureInfo cultureInfo)
		{
			return ErrorMessageFormatter.instance.FormatMessage(ErrorMessage, result, cultureInfo);
		}

		/// <summary>
		/// this is the real function that performs validation.
		/// </summary>
		/// <param name="actualResult"></param>
		/// <param name="objToValidate"></param>
		/// <param name="validationFlag"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public abstract Boolean Validate(
			ValidationResult actualResult,
			object objToValidate,
			ValidationFlags validationFlag,
			CultureInfo cultureInfo);
	}

	/// <summary>
	/// 
	/// </summary>
	public class MultipleValidatorValidationUnit : ValidationUnit
	{
		public IMultipleValidator Validator
		{
			get { return mValidator; }
		}
		private IMultipleValidator mValidator;

		public MultipleValidatorValidationUnit(bool isRecursive, ErrorMessage errorMessage, IMultipleValidator mValidator)
			: base(isRecursive, errorMessage)
		{
			this.mValidator = mValidator;
		}

		/// <summary>
		/// this is the real function that performs validation.
		/// </summary>
		/// <param name="actualResult"></param>
		/// <param name="objToValidate"></param>
		/// <param name="validationFlag"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public override Boolean Validate(
			ValidationResult actualResult,
			object objToValidate,
			ValidationFlags validationFlag,
			CultureInfo cultureInfo)
		{

			var res = mValidator.Validate(objToValidate, validationFlag).ToList();
			foreach (SingleValidationResult singleValidationResult in res)
			{
				actualResult.AddErrorMessage(
					GetErrorMessage(singleValidationResult, cultureInfo), singleValidationResult.SourceName);
			}

			return res.Count == 0;
		}
	}

	public class SingleValidatorValidationUnit : ValidationUnit
	{
		/// <summary>
		/// The real validator associated with the validation attribute.
		/// </summary>
		public IValidator Validator
		{
			get { return mValidator; }
		}
		private IValidator mValidator;

		public SingleValidatorValidationUnit(bool isRecursive, ErrorMessage errorMessage, IValidator mValidator)
			: base(isRecursive, errorMessage)
		{
			this.mValidator = mValidator;
		}

		/// <summary>
		/// this is the real function that performs validation.
		/// </summary>
		/// <param name="actualResult"></param>
		/// <param name="objToValidate"></param>
		/// <param name="validationFlag"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public override Boolean Validate(
			ValidationResult actualResult,
			object objToValidate,
			ValidationFlags validationFlag,
			CultureInfo cultureInfo)
		{

			SingleValidationResult res = mValidator.Validate(objToValidate);
			if (!res)
			{
				actualResult.Success = false;
				actualResult.AddErrorMessage(GetErrorMessage(res, cultureInfo), res.SourceName);
			}
			return res;
		}
	}
}