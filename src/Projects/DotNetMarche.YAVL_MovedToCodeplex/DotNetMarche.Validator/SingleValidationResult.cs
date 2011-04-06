using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator
{
	/// <summary>
	/// This class represent the result of a single validation
	/// </summary>
	public struct SingleValidationResult
	{


		public Boolean Success;
		private object _expectedValue;
		private object _actualValue;
		private String _sourceName;

		public SingleValidationResult(
			Boolean success,
			object expectedValue,
			object actualValue,
			String sourceName)
		{
			if (!success && String.IsNullOrEmpty(sourceName))
				throw new ArgumentException("Source Name must be specified for failing result", "sourceName");
			Success = success;
			_expectedValue = expectedValue ?? String.Empty;
			_actualValue = actualValue ?? String.Empty;
			_sourceName = sourceName ?? String.Empty;
		}

		public static implicit operator Boolean(SingleValidationResult res)
		{
			return res.Success;
		}

		public static SingleValidationResult GenericSuccess = new SingleValidationResult(true, "", "", "");
	
		public object ExpectedValue
		{
			get { return _expectedValue; }
		}

		public object ActualValue
		{
			get { return _actualValue; }
		}

		public String SourceName
		{
			get { return _sourceName; }
		}
	}
}