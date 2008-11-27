using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator
{
	/// <summary>
	/// This class represent the result of a single validation
	/// </summary>
	public struct SingleValidationResult {
		
		public Boolean Success;
		public object  ExpectedValue;
		public object	ActualValue;

		public SingleValidationResult(
			Boolean	success,
			object	expectedValue,
			object	actualValue) {
		
			Success			= success;
			ExpectedValue	= expectedValue;
			ActualValue		= actualValue; 
			}

		public static implicit operator Boolean(SingleValidationResult res) {
			return res.Success;
		}

		public static SingleValidationResult GenericSuccess = new SingleValidationResult(true, "", "");
		public static SingleValidationResult GenericError = new SingleValidationResult(false, "", "");
	}
}