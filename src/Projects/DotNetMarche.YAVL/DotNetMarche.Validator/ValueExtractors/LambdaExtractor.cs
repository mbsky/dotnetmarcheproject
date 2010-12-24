using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.ValueExtractors
{
	public class LambdaExtractor<T> : IValueExtractor 
	{
		public Func<T, Object> _Extractor { get; set; }
		private String _sourceName;

		public String SourceName
		{
			get { return _sourceName; }
		}

		public LambdaExtractor(Func<T, object> extractor)
			: this(extractor, ValidationResult.ValidationSourceObject)
		{
		}

		public LambdaExtractor(Func<T, object> extractor, string sourceName)
		{
			_Extractor = extractor;
			_sourceName = sourceName;
		}

		#region IValueExtractor Members

		public object ExtractValue(object objToValidate)
		{
			return _Extractor((T) objToValidate);
		}

		#endregion
	}
}
