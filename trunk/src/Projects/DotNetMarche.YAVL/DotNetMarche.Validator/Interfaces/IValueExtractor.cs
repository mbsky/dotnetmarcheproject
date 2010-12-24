using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator.Interfaces
{
	/// <summary>
	/// Base interface of an object that is able to extract the value to 
	/// validate from other object
	/// </summary>
	public interface IValueExtractor
	{

		object ExtractValue(object objToValidate);

		/// <summary>
		/// Identify with a string the name of the source. If the extractor is for
		/// property or field it will return the name of the property or field.
		/// </summary>
		/// <returns></returns>
		String SourceName {get;}
	}
}