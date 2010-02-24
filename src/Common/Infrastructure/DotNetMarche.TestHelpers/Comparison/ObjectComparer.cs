using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace DotNetMarche.TestHelpers.Comparison
{
	/// <summary>
	/// This class compare two object with reflection.
	/// </summary>
	public static class ObjectComparer
	{
		private const BindingFlags BindingFlagsForReflection = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		/// <summary>
		/// Main entry point, compare two object and tells if the two objects 
		/// are equal.
		/// </summary>
		/// <param name="entity1"></param>
		/// <param name="entity2"></param>
		/// <returns></returns>
		public static Boolean AreEqual(Object entity1, Object entity2)
		{
			if (entity1.GetType() != entity2.GetType()) return false;
			List<String> ErrorList = FindDifferencies(entity1, entity2);
			return ErrorList.Count == 0;
		}

		/// <summary>
		/// Find differencies between objects
		/// </summary>
		/// <param name="entity1"></param>
		/// <param name="entity2"></param>
		/// <returns></returns>
		public static List<String> FindDifferencies(object entity1, object entity2)
		{
			List<String> ErrorList = new List<String>();
			FindDifferencies(entity1, entity2, "root", ErrorList);
			return ErrorList;
		}

		private static void FindDifferencies(object entity1, object entity2, String path, ICollection<string> errorList)
		{
			foreach (PropertyInfo pinfo in entity1.GetType().GetProperties(BindingFlagsForReflection))
			{
				Object value1 = pinfo.GetValue(entity1, new Object[] { });
				Object value2 = pinfo.GetValue(entity2, new Object[] { });
				string name = pinfo.Name;
				CompareValue(value1, value2, errorList, name, path, pinfo.PropertyType);
			}
			foreach (FieldInfo pinfo in entity1.GetType().GetFields(BindingFlagsForReflection))
			{
				if (pinfo.Name.Contains("BackingField")) continue;
				Object value1 = pinfo.GetValue(entity1);
				Object value2 = pinfo.GetValue(entity2);
				string name = pinfo.Name;
				CompareValue(value1, value2, errorList, name, path, pinfo.FieldType);
			}
		}

		private static void CompareValue(object value1, object value2, ICollection<string> errorList, string name, String path, Type type)
		{
			if (value1 == null && value2 == null) return;
			if (value1 == null)
			{
				errorList.Add(String.Format("property {0} is different, first object is null, second object is not null", name));
				return;
			}
			if (TypeIsDirectlyComparable(type))
			{
				if (!value1.Equals(value2))
				{
					errorList.Add(String.Format("{3}.{0} is different {1}!={2}", name, value1, value2, path));
				}
			}
			else
			{
				FindDifferencies(value1, value2, path + "." + name, errorList);
			}
		}

		private static bool TypeIsDirectlyComparable(Type type)
		{
			return type.IsPrimitive || type == typeof(String);
		}
	}
}
