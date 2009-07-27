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

		public LambdaExtractor(Func<T, object> extractor)
		{
			_Extractor = extractor;
		}

		#region IValueExtractor Members

		public object ExtractValue(object objToValidate)
		{
			return _Extractor((T) objToValidate);
		}

		#endregion
	}
}
