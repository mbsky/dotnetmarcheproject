using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Utils;

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

		/// <summary>
		/// Initializes a new instance of the <see cref="LambdaExtractor&lt;T&gt;"/> class, prefer
		/// to use the <see cref="LambdaExtractor{T}(Func{T, Object}, string)"/> because it does not
		/// require a call to Expression.Compile() method.
		/// </summary>
		/// <remarks>This version accepts an Expression to grab property name, but this makes 
		/// the creation of the rule a little bit slow because it has to call method Compile
		/// on the expression</remarks>
		/// <param name="extractor">The extractor.</param>
		public LambdaExtractor(Expression<Func<T, object>> extractor)
		{
			String propertyName = extractor.GetMemberName();
			Init(extractor.Compile(), propertyName ?? ValidationResult.ValidationSourceObject);
		}

		public LambdaExtractor(Func<T, object> extractor, string sourceName)
		{
			Init(extractor, sourceName);
		}

		private void Init(Func<T, object> extractor, string sourceName)
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
