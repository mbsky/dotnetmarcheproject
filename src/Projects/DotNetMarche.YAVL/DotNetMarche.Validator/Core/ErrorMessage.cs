using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
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

		public const String sTokenActualValue = "${ActualValue}";
		public const String sTokenExpectedValue = "${ExpectedValue}";

		#endregion

		#region Constructors

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceType"></param>
		public ErrorMessage(
			String errorMessage,
			Type resourceType) : this (errorMessage, resourceType.AssemblyQualifiedName)
		{
		}

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
		public ErrorMessage(Expression<Func<String>> messageLambda)
		{
			MemberExpression me = messageLambda.Body as MemberExpression;
			Type t = me.Member.DeclaringType;
			//now I know that this type is one generated by the visual studio
			PropertyInfo pinfo = t.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic);
			mMessage = me.Member.Name;
			mResourceTypeName = t.AssemblyQualifiedName;
			lock (mResMangaers)
			{
				if (!mResMangaers.ContainsKey(t.FullName))
				{
					mResMangaers.Add(t.FullName, pinfo.GetValue(null, null));
				}

			}
		}

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="resourceManager"></param>
		public ErrorMessage(
			String errorMessage,
			ResourceManager resourceManager)
			: this(errorMessage, resourceManager.BaseName)
		{
			lock (mResMangaers)
			{
				if (!mResMangaers.ContainsKey(resourceManager.BaseName))
				{
					mResMangaers.Add(resourceManager.BaseName, resourceManager);
				}

			}
		}

		/// <summary>
		/// Basic Constructor
		/// </summary>
		/// <param name="errorMessage"></param>
		public ErrorMessage(
			String errorMessage)
			: this(errorMessage, (String)null) { }

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
			return ToString(Thread.CurrentThread.CurrentUICulture);
		}

		public string ToString(CultureInfo cultureInfo)
		{
			return GetRawMessage(cultureInfo);
		}

		public string ToString(CultureInfo cultureInfo, SingleValidationResult result)
		{
			String rawMessage = GetRawMessage(cultureInfo);
			return FormatMessage(rawMessage, result);
		}

		public string ToString(SingleValidationResult result)
		{
			String rawMessage = ToString();
			return FormatMessage(rawMessage, result);
		}

		private static string FormatMessage(string rawMessage, SingleValidationResult result)
		{
			if (rawMessage.Contains("$"))
			{
				StringBuilder sb = new StringBuilder(rawMessage);
				sb.Replace(sTokenActualValue, result.ActualValue.ToString());
				sb.Replace(sTokenExpectedValue, result.ExpectedValue.ToString());
				rawMessage = sb.ToString();
			}
			return rawMessage;
		}

		private string GetRawMessage(CultureInfo cultureInfo)
		{
			if (ResourceTypeName == null)
				return Message;

			ResourceManager rm = GetResourceManager(ResourceTypeName);
			return rm.GetString(Message, cultureInfo);
		}

		/// <summary>
		/// This is the dictionary of resource manager objects that are responsible to 
		/// gets resources from resource files.
		/// </summary>
		private static Hashtable mResMangaers = new Hashtable();

		private static void AddResourceManager(String resourceTypeName)
		{
			lock (mResMangaers)
			{
				System.Resources.ResourceManager rm;
				Type ty = Type.GetType(resourceTypeName);
				rm = new System.Resources.ResourceManager(ty);
				mResMangaers.Add(resourceTypeName, rm);
			}

		}

		private static ResourceManager GetResourceManager(String resName)
		{
			if (!mResMangaers.ContainsKey(resName))
			{
				AddResourceManager(resName);
			}
			return (ResourceManager)mResMangaers[resName];
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