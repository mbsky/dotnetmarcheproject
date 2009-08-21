using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DotNetMarche.Validator
{
	public class ValidationException : Exception
	{
		public ValidationException(IEnumerable<String> errorMessages, Exception innerException)
			: base(errorMessages.First(), innerException)
		{
			this.errorMessages.AddRange(errorMessages);
		}

		public ValidationException(IEnumerable<String> errorMessages) : this(errorMessages, null)
		{
		}

		public List<String> ErrorMessages { get
		{
			return errorMessages;
		}
		}
		private List<String> errorMessages = new List<String>();
	}
}
