using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.ValueExtractors
{
	/// <summary>
	/// This extractor is a pass-through used for the attribute defined at
	/// type level. 
	/// </summary>
	public class ObjectValueExtractor : IValueExtractor {

		public String SourceName
		{
			get { return ValidationResult.ValidationSourceObject; }
		}
		
		public object ExtractValue(object objToValidate) {
			return objToValidate;
		}
	}
}