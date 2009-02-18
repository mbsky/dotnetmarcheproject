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
}