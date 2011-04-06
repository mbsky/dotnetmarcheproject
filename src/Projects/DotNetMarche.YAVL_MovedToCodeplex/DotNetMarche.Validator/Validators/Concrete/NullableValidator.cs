using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	class NullableValidator : BaseValidator  {

		NullableValidator(IValueExtractor valueExtractor) : base(valueExtractor) {}

		public override SingleValidationResult Validate(object objectToValidate) {
			throw new NotImplementedException();
		}
	

	}
}