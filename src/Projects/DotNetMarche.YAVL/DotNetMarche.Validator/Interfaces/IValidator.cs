using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;

namespace DotNetMarche.Validator.Interfaces
{
	/// <summary>
	/// This is the interface that a validator should implement to 
	/// validate an object.
	/// </summary>
	public interface IValidator {

		/// <summary>
		/// Validator main method is used to validate a single object 
		/// and return a simple ValidationResult.
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <returns></returns>
		SingleValidationResult Validate(object objectToValidate);
		 
	}


	/// <summary>
	/// Interface that identify a validator capable of validating something and returning
	/// more than a single validation result. 
	/// </summary>
	public interface IMultipleValidator
	{
		/// <summary>
		/// Validates the specified object to validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="validationFlags">Validation flags, since the object does multiple validation
		/// it should know actual validation flags.</param>
		/// <returns>The list of errors, if the return is an empty <see cref="IEnumerable{SingleValidationResult}"/>
		/// </returns>
		IEnumerable<SingleValidationResult> Validate(Object objectToValidate, ValidationFlags validationFlags);
	}
}