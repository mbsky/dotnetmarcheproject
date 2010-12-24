using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using DotNetMarche.Validator;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// this class encapsulate the logic to handle a collection of validation units. All validation
	/// units must belong to the same object.
	/// </summary>
	public class ValidationUnitCollection
	{

		#region Collection Wrapping Methods

		private List<ValidationUnit> mList = new List<ValidationUnit>();

		public void Add(ValidationUnit validationUnit)
		{
			mList.Add(validationUnit);
		}

		public Int32 Count
		{
			get { return mList.Count; }
		}

		public ValidationUnit this[Int32 index]
		{
			get { return mList[index]; }
			//set { mList[index] = value; }
		}

		#endregion

		public ValidationResult ValidateObject(
			ValidationResult validationResult,
			object objToValidate,
			ValidationFlags validationFlags)
		{

			ValidateObject(validationResult, objToValidate, validationFlags,
						   System.Threading.Thread.CurrentThread.CurrentUICulture);
			return validationResult;
		}

		/// <summary>
		/// Validate an object modifying an existing Validation Result.
		/// </summary>
		/// <param name="actualResult">The result object that contains validation result, it will
		/// be populated by this function</param>
		/// <param name="objToValidate">The object to be validated.</param>
		/// <param name="validationFlags">Options for comparing objects.</param>
		/// <param name="cultureInfo">The cultureinfo used to validate the object.</param>
		public void ValidateObject(
			ValidationResult actualResult,
			object objToValidate,
			ValidationFlags validationFlags,
			CultureInfo cultureInfo)
		{

			foreach (ValidationUnit vu in mList)
			{
				if (ValdationFlagsUtils.RecursiveValidation(validationFlags) || vu.IsFirstLevelValidationUnit)
				{
					Boolean validationResult = vu.Validate(actualResult, objToValidate, validationFlags, cultureInfo);
					if (!validationResult && ValdationFlagsUtils.StopOnFirstError(validationFlags)) break;
				}
			}
		}

	}
}