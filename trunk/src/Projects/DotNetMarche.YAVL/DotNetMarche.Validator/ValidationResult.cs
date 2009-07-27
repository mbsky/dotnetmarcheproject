using System;
using System.Collections.Generic;
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

		/// <summary>
		/// All error messages are included into a list.
		/// </summary>
		protected List<String> mErrorMessage = new List<String>();
		
		/// <summary>
		/// Provide readonly access to the list of errors.
		/// </summary>
		public IList<String> ErrorMessages {
			get {return mErrorMessage.AsReadOnly();}
		}
		
		/// <summary>
		/// Add an error.
		/// </summary>
		/// <param name="errorMessage"></param>
		internal void AddErrorMessage(String errorMessage) {
			mErrorMessage.Add(errorMessage);
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