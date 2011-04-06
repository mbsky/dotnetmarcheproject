using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.ValueExtractors
{
	/// <summary>
	/// Since we can validate both property or field it is necessary to have a single
	/// interface to cache fieldinfo or propertyinfo
	/// </summary>
	public class PropertyInfoValueExtractor : IValueExtractor {

		private PropertyInfo mPropertyInfo;

		public PropertyInfoValueExtractor(PropertyInfo pi) {
			mPropertyInfo = pi;
		}

		public String SourceName
		{
			get { return mPropertyInfo.Name; }
		}

		public object ExtractValue(object objToValidate) {
			return mPropertyInfo.GetValue(objToValidate, null);
		}
	}
}