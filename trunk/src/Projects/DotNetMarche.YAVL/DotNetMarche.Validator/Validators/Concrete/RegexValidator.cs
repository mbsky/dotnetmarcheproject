using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.Validators.Concrete
{
	public class RegexValidator : BaseValidator
	{
		public String RegexExpression { get; set; }


		public RegexValidator(IValueExtractor valueExtractor, String regexExpression) : base(valueExtractor)
		{
			this.RegexExpression = regexExpression;
		}

		public override SingleValidationResult Validate(object objectToValidate)
		{
			String value = base.Extract<String>(objectToValidate);
			if (Regex.IsMatch(value, RegexExpression))
			return SingleValidationResult.GenericSuccess;
			return new SingleValidationResult(false, String.Format("Match regex {0}", RegexExpression), value, mValueExtractor.SourceName);
		}
	}
}
