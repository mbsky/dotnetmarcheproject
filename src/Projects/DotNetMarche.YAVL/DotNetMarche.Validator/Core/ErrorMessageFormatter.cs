using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// This class does all the formatting required to have a nicely error messages
	/// that supports localization.
	/// </summary>
	[Obsolete]
	public class ErrorMessageFormatter {

		#region Constants

		private const String sTokenActualValue = "${ActualValue}";
		private const String sTokenExpectedValue = "${ExpectedValue}";

		#endregion

		#region Singleton

		public static ErrorMessageFormatter instance = new ErrorMessageFormatter();

		private ErrorMessageFormatter() { 
			mResMangaers = new Dictionary<string,System.Resources.ResourceManager>();
		}

		#endregion

		#region Formatting

		public String FormatMessage(
			ErrorMessage				errorMessage,
			SingleValidationResult 	validationResult) {

			return FormatMessage(errorMessage, validationResult, 
			                     System.Threading.Thread.CurrentThread.CurrentCulture);
			}

		public String FormatMessage(
			ErrorMessage				errorMessage,
			SingleValidationResult	validationResult,
			CultureInfo					cultureInfo) {

			String baseMessage = GetBaseMessage(errorMessage, cultureInfo);
			String formattedBaseMessages = PostProcessMessage(baseMessage, validationResult, cultureInfo);
			return formattedBaseMessages;
			}

		#endregion

		#region Inner routines

		/// <summary>
		/// This is the dictionary of resource manager objects that are responsible to 
		/// gets resources from resource files.
		/// </summary>
		private Dictionary<String, System.Resources.ResourceManager> mResMangaers; 

		private void AddResourceManager(String resourceTypeName) {
			System.Resources.ResourceManager rm;
			Type ty = Type.GetType(resourceTypeName);
			rm = new System.Resources.ResourceManager(ty);
			mResMangaers.Add(resourceTypeName, rm);
		}

		private ResourceManager GetResourceManager(String resName) {
			if (!mResMangaers.ContainsKey(resName)) {
				AddResourceManager(resName);
			}
			return mResMangaers[resName];
		}

		private String GetBaseMessage(
			ErrorMessage	errorMessage, 
			CultureInfo		currentCulture) {

			if (errorMessage.ResourceTypeName == null) return errorMessage.Message; 
			System.Resources.ResourceManager rm = GetResourceManager(errorMessage.ResourceTypeName);
			return rm.GetString(errorMessage.Message, currentCulture); 
			}

		/// <summary>
		/// This method does a little of post processing handling to insert some basic content to the
		/// base message string.
		/// </summary>
		/// <param name="baseMessage"></param>
		/// <param name="validationResult"></param>
		/// <returns></returns>
		private String PostProcessMessage(
			String						baseMessage, 
			SingleValidationResult	validationResult,
			CultureInfo					cultureInfo) {
			
			StringBuilder sb = new StringBuilder();
			sb.Append(baseMessage);
			sb.Replace(sTokenActualValue, validationResult.ActualValue.ToString());
			sb.Replace(sTokenExpectedValue, validationResult.ExpectedValue.ToString()); 
			return sb.ToString();
			}

		#endregion
	}
}