using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Validator
{
	public class ValidationResult {

		#region Constructors

		public ValidationResult() {
			Init(true);
		}

		public ValidationResult(Boolean successStatus) {
			Init(successStatus);
		}

		private void Init(Boolean successStatus) {
			this.Success = successStatus;
		}

		#endregion

		#region Constants

		public const string ValidationSourceObject = "[Object]";

		#endregion

		public List<ValidationError> Errors
		{
			get
			{
				return errors;
			}
		}
		private List<ValidationError> errors = new List<ValidationError>();

		public IList<string> ErrorMessages
		{
			get
			{
				return Errors.Select(em => em.Message).ToList();
			}
		}

		/// <summary>
		/// Add an error to the list of validation error already found.
		/// </summary>
		/// <param name="errorMessage">The message that describes the error.</param>
		/// <param name="sourceName">The source that causes validation error.</param>
		internal void AddErrorMessage(String errorMessage, String sourceName)
		{
			errors.Add(new ValidationError(errorMessage, sourceName));
			Success = false;
		}

		/// <summary>
		/// Tells if operation is successful or unsuccessful
		/// </summary>
		public Boolean Success {
			get { return mSuccess; }
			set { mSuccess = value; }
		}
		private Boolean mSuccess;

		#region Conversion Operators

		/// <summary>
		/// Permits to tread ValidationResult as a boolean.
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		public static implicit operator Boolean(ValidationResult res) {
			return res.Success;
		}

		#endregion


	}
}