using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Validator
{
	public class ValidationError
	{
		public ValidationError(string message, string sourceName)
		{
			Message = message;
			SourceName = sourceName;
		}

		public ValidationError(string message) : this(message, ValidationResult.ValidationSourceObject)
		{
		}

		/// <summary>
		/// The error message.
		/// </summary>
		public String Message { get; set; }

		/// <summary>
		/// usually is the name of the property that causes validation error.
		/// </summary>
		public String SourceName { get; set; }
	}
}
