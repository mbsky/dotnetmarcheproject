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
		private object  _expectedValue;
		private object	_actualValue;

		public SingleValidationResult(
			Boolean	success,
			object	expectedValue,
			object	actualValue) {
		
			Success			= success;
			_expectedValue	= expectedValue ?? String.Empty;
			_actualValue		= actualValue ?? String.Empty; 
			}

		public static implicit operator Boolean(SingleValidationResult res) {
			return res.Success;
		}

		public static SingleValidationResult GenericSuccess = new SingleValidationResult(true, "", "");
		public static SingleValidationResult GenericError = new SingleValidationResult(false, "", "");

		public object ExpectedValue
		{
			get { return _expectedValue; }
			set { _expectedValue = value; }
		}

		public object ActualValue
		{
			get { return _actualValue; }
			set { _actualValue = value; }
		}
	}
}