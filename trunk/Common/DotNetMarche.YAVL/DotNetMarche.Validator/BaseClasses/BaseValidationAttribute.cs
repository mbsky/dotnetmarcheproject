using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;

namespace DotNetMarche.Validator.BaseClasses
{
	public abstract class BaseValidationAttribute : Attribute {

		#region Constructors

		internal protected BaseValidationAttribute(
			String errorMessage) {
			mErrorMessage = errorMessage;
			}

		internal protected BaseValidationAttribute(
			String errorMessage, 
			String resourceTypeName) {

			mErrorMessage			= errorMessage;
			mResourceTypeName		= resourceTypeName;
			}
		#endregion

		/// <summary>
		/// Each validation can have different error messages telling whath 
		/// goes wrong with validation. Moreover all error messages can be taken from
		/// resource files to be localized.
		/// </summary>
		public String ErrorMessage {
			get {return mErrorMessage;}
		}
		protected String mErrorMessage = String.Empty;

		/// <summary>
		/// When you desire to get error messages from a resource file it is necessary to 
		/// specify the resource type. when this property is not null then ErrorMessage property
		/// identify the index of the resource to be used.
		/// </summary>
		public String ResourceTypeName {
			get {return mResourceTypeName;}
		}
		protected String mResourceTypeName = null;

		/// <summary>
		/// Caller can specify a custom IValueExtractor for a validation attribute, this can be
		/// done specifying OverrideValidator property in base validation attribute.
		/// </summary>
		public Type ValueExtractorType {
			get { return mValueExtractorType; }
			set { mValueExtractorType = value; }
		}
		private Type mValueExtractorType = null;
	
		/// <summary>
		/// Tells if the ValueExtractor is overriden by the attribute.
		/// </summary>
		/// <returns></returns>
		public Boolean IsValueExtractorOverriden {
			get{return mValueExtractorType != null;}
		}

		/// <summary>
		/// Retrieve the value extractor when overriden
		/// </summary>
		/// <returns></returns>
		public IValueExtractor CreateValueExtractor() {
			return (IValueExtractor) Activator.CreateInstance(mValueExtractorType);
		}

		/// <summary>
		/// this is the real routine that is overriden by derived classes
		/// </summary>
		/// <param name="valueExtractor"></param>
		/// <returns></returns>
		public abstract IValidator CreateValidator(IValueExtractor valueExtractor);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual ErrorMessage CreateErrorMessage() {
			return new ErrorMessage(ErrorMessage, ResourceTypeName); 
		}
	}
}