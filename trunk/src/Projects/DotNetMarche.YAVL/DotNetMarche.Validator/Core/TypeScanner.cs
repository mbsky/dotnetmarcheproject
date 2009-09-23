using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.Core
{
	public class TypeScanner
	{

		private Type mType;
		public TypeScanner(Type ty)
		{
			mType = ty;
		}

		/// <summary>
		/// This function scan type of object to find all validators related to object themselves.
		/// </summary>
		/// <param name="ty"></param>
		public ValidationUnitCollection Scan()
		{

			ValidationUnitCollection vc = new ValidationUnitCollection();
			PopulateValidationUnitcollection(vc);
			return vc;
		}

		/// <summary>
		/// This function scan type of object to find all validators related to object themselves but
		/// it descend into the graph of dependencies to check all internal objects.
		/// Since this method needs to descent into the 
		/// object graph, it does not return a single validationUnitCollection but fully
		/// populate a complete list of related object.
		/// </summary>
		/// <param name="currentTypeMapRules">This is the dictionary to populate.</param>
		public void RecursiveScan(
			Dictionary<Type, ValidationUnitCollection> currentTypeMapRules)
		{

			List<Type> loadedTypes = new List<Type>();
			RecursivePopulate(mType, currentTypeMapRules, loadedTypes);
			List<Type> analyzedTypes = new List<Type>();
			RecursiveSetObjectValidator(mType, currentTypeMapRules, loadedTypes, analyzedTypes);
		}

		#region Type Scanner Routines

		/// <summary>
		/// This function populate the collection
		/// </summary>
		/// <param name="vc"></param>
		/// <returns>This function return the inner list of all type that are
		/// used in field And/Or property</returns>
		private void PopulateValidationUnitcollection(
			ValidationUnitCollection vc)
		{

			ScanForTypeAttributes(mType, vc);
			ScanForField(mType, vc, null);
			ScanForProperty(mType, vc, null);
		}

		/// <summary>
		/// Given an array of object of validation attributes create all
		/// requested ValidationUnits.
		/// </summary>
		/// <param name="vc"></param>
		///<param name="valueExtractor">The value extractor proposed by the caller
		/// the real value extractor can be overriden by the attribute.</param>
		/// <param name="validationfields"></param>
		private static void BuildValidationUnitFromAttributeList(
			ValidationUnitCollection vc,
			IValueExtractor valueExtractor,
			object[] validationfields)
		{

			foreach (BaseValidationAttribute va in validationfields)
			{

				if (va.IsValueExtractorOverriden)
					valueExtractor = va.CreateValueExtractor();
				vc.Add(new ValidationUnit(
							va.CreateErrorMessage(),
							va.CreateValidator(valueExtractor)));
			}
		}

		/// <summary>
		/// Scan the type and check if there are some attributes defined on the whole type.
		/// </summary>
		/// <param name="typeToCheck"></param>
		/// <param name="vc"></param>
		private void ScanForTypeAttributes(
			Type typeToCheck,
			ValidationUnitCollection vc)
		{

			object[] validationfields = typeToCheck.GetCustomAttributes(typeof(BaseValidationAttribute), false);
			BuildValidationUnitFromAttributeList(vc, new ObjectValueExtractor(), validationfields);
		}

		/// <summary>
		/// Scan all fields of the type and populate both the collection of rules and other types encountered.
		/// </summary>
		/// <param name="vc"></param>
		/// <param name="fieldTypes"></param>
		private void ScanForField(
			Type typeToCheck,
			ValidationUnitCollection vc,
			List<Type> fieldTypes)
		{

			FieldInfo[] afi = typeToCheck.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fi in afi)
			{
				object[] validationfields = fi.GetCustomAttributes(typeof(BaseValidationAttribute), false);
				BuildValidationUnitFromAttributeList(vc, new FieldInfoValueExtractor(fi), validationfields);
				PopulateDescendantList(fi.FieldType, fieldTypes);
			}
		}

		private void ScanForProperty(
			Type typeToCheck,
			ValidationUnitCollection vc,
			List<Type> propertyTypes)
		{

			PropertyInfo[] api = typeToCheck.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo pi in api)
			{
				object[] validationfields = pi.GetCustomAttributes(typeof(BaseValidationAttribute), false);
				BuildValidationUnitFromAttributeList(vc, new PropertyInfoValueExtractor(pi), validationfields);
				PopulateDescendantList(pi.PropertyType, propertyTypes);
			}
		}

		/// <summary>
		/// Check if the type is not a primitive one, and if it is not contained in the list it will be added.
		/// </summary>
		/// <param name="ty"></param>
		/// <param name="fieldTypes"></param>
		private void PopulateDescendantList(
			Type ty,
			List<Type> fieldTypes)
		{
			if (fieldTypes != null && !IsBasicType(ty) && !fieldTypes.Contains(ty))
				fieldTypes.Add(ty);
		}

		private Boolean IsBasicType(Type ty)
		{
			return ty.Assembly.FullName.StartsWith("mscorlib");
		}
		#endregion

		#region Recursive Type Scanners

		/// <summary>
		/// Recursive scan type to find all validator defined for each object of the graph.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="loadList"></param>
		/// <param name="typeToCheck"></param>
		private void RecursivePopulate(
			Type typeToCheck,
			Dictionary<Type, ValidationUnitCollection> list,
			IList<Type> loadList)
		{

			//first check if the type was already loaded or is in ignorelist.
			if (list.ContainsKey(typeToCheck)) return;

			//Type is not contained so it is necessary to check, first prepare parameter collections 
			List<Type> relatedTypes = new List<Type>();
			ValidationUnitCollection vc = new ValidationUnitCollection();
			list.Add(typeToCheck, vc);
			loadList.Add(typeToCheck);

			//Then scan for property or fields.
			ScanForTypeAttributes(typeToCheck, vc);
			ScanForField(typeToCheck, vc, relatedTypes);
			ScanForProperty(typeToCheck, vc, relatedTypes);
			relatedTypes.ForEach(delegate(Type ty)
			{
				RecursivePopulate(ty, list, loadList);
			});
		}

		/// <summary>
		/// Starting from current type this routine scan all field and property of the type to 
		/// find if some property of type contains a validatable object, when found that object is rescanned
		/// </summary>
		/// <param name="ruleMap"></param>
		/// <param name="typeToCheck"></param>
		/// <param name="listOfLoadedTypes">Since some objects could be already loaded, we check
		/// all properties and fields of the new types of object that are inserted.</param>
		/// <param name="listOfScannedTypes">This is an inner list that check type already scanned,
		/// consider what happended if object A has three property of type B, we do not want to 
		/// scan Three times objectB and setting an excessive number of validator.</param>
		private void RecursiveSetObjectValidator(
			Type typeToCheck,
			Dictionary<Type, ValidationUnitCollection> ruleMap,
			IList<Type> listOfLoadedTypes,
			IList<Type> listOfScannedTypes)
		{

			//If this type was not loaded there is no need to check, probably its set or rules is already loaded
			if (!listOfLoadedTypes.Contains(typeToCheck)) return;

			listOfScannedTypes.Add(typeToCheck);

			System.Diagnostics.Debug.Assert(ruleMap.ContainsKey(typeToCheck), "We are scanning an object that where not scanned for rules");
			//now we should check for each property and field, we need to find
			foreach (FieldInfo fi in typeToCheck.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				if (ruleMap.ContainsKey(fi.FieldType))
				{
					IValueExtractor extractor = new FieldInfoValueExtractor(fi);
					ruleMap[typeToCheck].Add(
						ValidationUnit.CreateObjectValidationUnit(
							extractor, fi.Name, ruleMap));
				}
				if (!listOfScannedTypes.Contains(fi.FieldType))
					RecursiveSetObjectValidator(fi.FieldType, ruleMap, listOfLoadedTypes, listOfScannedTypes);
			}

			foreach (PropertyInfo pi in typeToCheck.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (ruleMap.ContainsKey(pi.PropertyType))
				{
					IValueExtractor extractor = new PropertyInfoValueExtractor(pi);
					ruleMap[typeToCheck].Add(
						ValidationUnit.CreateObjectValidationUnit(
							extractor, pi.Name, ruleMap));
				}
				if (!listOfScannedTypes.Contains(pi.PropertyType))
					RecursiveSetObjectValidator(pi.PropertyType, ruleMap, listOfLoadedTypes, listOfScannedTypes);
			}
		}


		#endregion

	}
}