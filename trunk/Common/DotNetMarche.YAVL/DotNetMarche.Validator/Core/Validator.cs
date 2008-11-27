using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator;
using System.Reflection;

namespace DotNetMarche.Validator.Core
{
	/// <summary>
	/// This is the real class that does all the validation.
	/// </summary>
	public class Validator {

		#region InternalStructures

		/// <summary>
		/// Reflection on the types are cached in a static dictionary, this to avoid to scan
		/// with reflection at every call.
		/// </summary>
		private Dictionary<Type, ValidationUnitCollection> mValidationRules; 

		public Validator() {
			mValidationRules = new Dictionary<Type,ValidationUnitCollection>();
		}

		#endregion

		#region Validation Interfaces

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objToValidate"></param>
		/// <param name="stopOnFirstError"></param>
		/// <returns></returns>
		public ValidationResult ValidateObject(
			object				objToValidate,
			ValidationFlags	validationFlags) {

			ValidationUnitCollection rules = GetRules(objToValidate);
			ValidationResult result = new ValidationResult();
			return rules.ValidateObject(result, objToValidate, validationFlags); 
			}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objToValidate"></param>
		/// <returns></returns>
		public ValidationResult ValidateObject(
			object	objToValidate) {
			return ValidateObject(objToValidate, 0);
			}

		#endregion

		#region Inner validation routines

		private ValidationUnitCollection GetRules(object objToValidate) {
			if (!mValidationRules.ContainsKey(objToValidate.GetType())) {
				ScanTypeForAttribute(objToValidate.GetType());
			}
			return mValidationRules[objToValidate.GetType()];
		}

		/// <summary>
		/// This function scan type of object to find all validators related to object themselves.
		/// This new version check actually for each type in object graph.
		/// </summary>
		/// <param name="ty"></param>
		public void ScanTypeForAttribute(Type ty) {
			ScanTypeForAttribute(ty, true);
		}

		/// <summary>
		/// This function scan type of object to find all validators related to object themselves.
		/// This new version check actually for each type in object graph.
		/// </summary>
		/// <param name="ty"></param>
		public void ScanTypeForAttribute(Type ty, Boolean recursive) {
			//Create and add the collection to this type
			TypeScanner ts = new TypeScanner(ty);
			if (recursive) {
				ts.RecursiveScan(mValidationRules);
			}	
			else {
				ValidationUnitCollection vc = ts.Scan();
				mValidationRules.Add(ty, vc);
			}	
		}
		#endregion

		#region Programmatic validation

		/// <summary>
		/// This is the base class to add validation programmatically, due to dynamic discovery of
		/// the attributes, when a type is inserted through this interface no dynamic scan for attribute
		/// is done.
		/// </summary>
		/// <param name="objType"></param>
		/// <param name="validationUnit"></param>
		public void AddValidationRule(
			Type					objType, 
			ValidationUnit		validationUnit) {
		
			if (!mValidationRules.ContainsKey(objType))
				mValidationRules.Add(objType, new ValidationUnitCollection());
			mValidationRules[objType].Add(validationUnit);
			}

		#endregion
	}
}