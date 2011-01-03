using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DotNetMarche.Validator
{
	public class ValidationException : Exception
	{
		public ValidationException(IEnumerable<ValidationError> errorMessages, Exception innerException)
			: base(errorMessages
			.Select(m => m.Message)
			.Aggregate((s1, s2) =>s1 + "\n" + s2), innerException)
		{
			this.Errors.AddRange(errorMessages);
		}

		public ValidationException(IEnumerable<ValidationError> errorMessages)
			: this(errorMessages, null)
		{
		}

        public ValidationException(params ValidationError[] errorMessages)
            : this(errorMessages, null)
        {
        }

		public List<ValidationError> Errors
		{
			get
			{
				return errors;
			}
		}
		private List<ValidationError> errors = new List<ValidationError>();

		public List<String> ErrorMessages
		{
			get
			{
				return Errors.Select(em => em.Message).ToList();
			}
		}
	}
}
