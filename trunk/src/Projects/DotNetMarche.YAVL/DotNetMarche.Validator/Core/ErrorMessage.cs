using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// Represent all the information needed to format a message.
	/// </summary>
	public class ErrorMessage
	{

		#region Constants

		private const String sTokenActualValue = "${ActualValue}";
		private const String sTokenExpectedValue = "${ExpectedValue}";

		#endregion

		#region Constructors

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceTypeName"></param>
		public ErrorMessage(
			String errorMessage,
			String resourceTypeName)
		{

			mMessage = errorMessage;
			mResourceTypeName = resourceTypeName;
		}

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceTypeName"></param>
		public ErrorMessage(
			String errorMessage)
			: this(errorMessage, null) { }

		#endregion

		#region Properties

		/// <summary>
		/// Empty error message
		/// </summary>
		public static ErrorMessage empty = new ErrorMessage("");

		/// <summary>
		/// Each validation can have different error messages telling whath 
		/// goes wrong with validation. Moreover all error messages can be taken from
		/// resource files to be localized.
		/// </summary>
		public String Message
		{
			get { return mMessage; }
		}
		protected String mMessage = String.Empty;

		/// <summary>
		/// When you desire to get error messages from a resource file it is necessary to 
		/// specify the resource type. when this property is not null then ErrorMessage property
		/// identify the index of the resource to be used.
		/// </summary>
		public String ResourceTypeName
		{
			get { return mResourceTypeName; }
		}
		protected String mResourceTypeName = null;

		#endregion

		#region Message Formatting.

		/// <summary>
		/// This is the basic message formatting, uses the thread current culture
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(Thread.CurrentThread.CurrentCulture);
		}

		public string ToString(CultureInfo cultureInfo)
		{
			if (ResourceTypeName == null)
				return Message;
			ResourceManager rm = GetResourceManager(ResourceTypeName);
			return rm.GetString(Message, cultureInfo);
		}

		public string ToString(CultureInfo cultureInfo, SingleValidationResult result)
		{
			String rawMessage;
			if (ResourceTypeName == null)
			{
				rawMessage = Message;
			}
			else
			{
				ResourceManager rm = GetResourceManager(ResourceTypeName);
				rawMessage = rm.GetString(Message, cultureInfo);
			}
			if (rawMessage.Contains("$"))
			{
				StringBuilder sb = new StringBuilder(rawMessage);
				sb.Replace(sTokenActualValue, result.ActualValue.ToString());
				sb.Replace(sTokenExpectedValue, result.ExpectedValue.ToString());
				rawMessage = sb.ToString();
			}
			return rawMessage;
		}

		/// <summary>
		/// This is the dictionary of resource manager objects that are responsible to 
		/// gets resources from resource files.
		/// </summary>
		private static Dictionary<String, System.Resources.ResourceManager> mResMangaers;

		private static void AddResourceManager(String resourceTypeName)
		{
			System.Resources.ResourceManager rm;
			Type ty = Type.GetType(resourceTypeName);
			rm = new System.Resources.ResourceManager(ty);
			mResMangaers.Add(resourceTypeName, rm);
		}

		private static ResourceManager GetResourceManager(String resName)
		{
			if (!mResMangaers.ContainsKey(resName))
			{
				AddResourceManager(resName);
			}
			return mResMangaers[resName];
		}

		#endregion

		#region Operators

		public static implicit operator ErrorMessage(String message)
		{
			return new ErrorMessage(message);
		}

		#endregion


	}
}