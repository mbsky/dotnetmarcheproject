using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator.Interfaces
{
	/// <summary>
	/// Base interface of an object that is able to extract the value to 
	/// validate from other object
	/// </summary>
	public interface IValueExtractor {
		object ExtractValue(object objToValidate);
	}
}