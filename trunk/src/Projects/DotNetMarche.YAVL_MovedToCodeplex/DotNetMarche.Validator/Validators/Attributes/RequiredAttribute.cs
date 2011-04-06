using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class RequiredAttribute : BaseValidationAttribute {

		#region Constructors

		object mNullValue;
		private void Init(object nullValue) {
			mNullValue = nullValue;
		}

		public RequiredAttribute() : base(null) {
			Init(null);
		} 

		public RequiredAttribute(String errorMessage) : base(errorMessage) {
			Init(null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RequiredAttribute"/> class.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="nullValue">The value that is used to consider that property value
		/// null.</param>
		public RequiredAttribute(String errorMessage, object nullValue) : base(errorMessage) {
			Init(nullValue);
		} 

		public RequiredAttribute(
			String errorMessage, 
			String resourceTypeName, 
			object nullValue) : base(errorMessage, resourceTypeName) {
			Init(nullValue);
			} 

		#endregion

		public override IValidator CreateValidator(IValueExtractor valueExtractor) {
			return new RequiredValidator(valueExtractor, mNullValue); 
		}
	}
}