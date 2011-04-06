using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.ValueExtractors
{
	/// <summary>
	/// This class is able to extract value from property or field
	/// based only on name
	/// </summary>
	public class NamedValueExtractor : IValueExtractor {

		IValueExtractor	innerValueExtractor	= null;
		String				mPartName;

		public NamedValueExtractor(String partName) {
			mPartName = partName;
		}

		public String SourceName
		{
			get { return mPartName; }
		}

		/// <summary>
		/// Extract value, it extract the part of the object specified by mPartName 
		/// and work on the type of the object passed in the first call.
		/// </summary>
		/// <param name="objToValidate"></param>
		/// <returns></returns>
		public object ExtractValue(object objToValidate) {
			if (innerValueExtractor == null)
				innerValueExtractor = CreateInnerExtractor(objToValidate.GetType());
			return innerValueExtractor.ExtractValue(objToValidate);   
		}

		/// <summary>
		/// This function scan the inner type to find the named property/field.
		/// </summary>
		/// <param name="type"></param>
		private IValueExtractor CreateInnerExtractor(Type type) {
			IValueExtractor value = ScanForProperty(type);
			if (value == null) value = ScanForField(type);
			if (value == null) 
				throw new ArgumentException(String.Format("The type {0} does not contain no property or field named {1}", type, mPartName));
			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private IValueExtractor ScanForProperty(Type type) {
			PropertyInfo pi = type.GetProperty(mPartName);
			if (pi != null)
				return new PropertyInfoValueExtractor(pi);
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private IValueExtractor ScanForField(Type type) {
			FieldInfo fi = type.GetField(mPartName);
			if (fi != null)
				return new FieldInfoValueExtractor(fi);
			return null;
		}
	}
}