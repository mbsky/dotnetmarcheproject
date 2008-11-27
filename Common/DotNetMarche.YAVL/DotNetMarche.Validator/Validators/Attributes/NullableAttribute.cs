using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Attributes
{
	/// <summary>
	/// This attribute check if a field cannot be null. 
	/// </summary>
	class NullableAttribute : BaseValidationAttribute {

		public NullableAttribute(String errorMessage) : base(errorMessage) {} 

		public override IValidator CreateValidator(IValueExtractor valueExtractor) {
			throw new NotImplementedException();
		}
	}
}