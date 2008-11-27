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
	public  class FieldInfoValueExtractor : IValueExtractor {

		private FieldInfo mFieldInfo;

		public FieldInfoValueExtractor(FieldInfo fi) {
			mFieldInfo = fi;
		}
		
		public object ExtractValue(object objToValidate) {
			return mFieldInfo.GetValue(objToValidate);
		}
	}
}