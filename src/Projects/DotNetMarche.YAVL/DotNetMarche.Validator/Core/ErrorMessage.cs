using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// Represent all the information needed to format a message.
	/// </summary>
	public class ErrorMessage {

		/// <summary>
		/// Empty error message
		/// </summary>
		public static ErrorMessage empty = new ErrorMessage("");

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceTypeName"></param>
		public ErrorMessage(
			String errorMessage,
			String resourceTypeName) {
			
			mMessage					= errorMessage;
			mResourceTypeName		= resourceTypeName ;
			}

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceTypeName"></param>
		public ErrorMessage(
			String errorMessage) : this (errorMessage, null) {}

		/// <summary>
		/// Each validation can have different error messages telling whath 
		/// goes wrong with validation. Moreover all error messages can be taken from
		/// resource files to be localized.
		/// </summary>
		public String Message {
			get {return mMessage;}
		}
		protected String mMessage = String.Empty;

		/// <summary>
		/// When you desire to get error messages from a resource file it is necessary to 
		/// specify the resource type. when this property is not null then ErrorMessage property
		/// identify the index of the resource to be used.
		/// </summary>
		public String ResourceTypeName {
			get {return mResourceTypeName;}
		}
		protected String mResourceTypeName = null;
	}
}