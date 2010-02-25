using System;
using System.Collections;
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
	public class ObjectComparer
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
			ObjectComparer comparer = new ObjectComparer();
			return comparer.Compare(entity1, entity2);
		}

		/// <summary>
		/// Main entry point, compare two object and tells if the two objects 
		/// are equal.
		/// </summary>
		/// <param name="entity1"></param>
		/// <param name="entity2"></param>
		/// <returns></returns>
		public Boolean Compare(Object entity1, Object entity2)
		{
			if (entity1.GetType() != entity2.GetType()) return false;
			List<String> errorList = FindDifferencies(entity1, entity2);
			return errorList.Count == 0;
		}

		/// <summary>
		/// Find differencies between objects
		/// </summary>
		/// <param name="entity1"></param>
		/// <param name="entity2"></param>
		/// <returns></returns>
		public List<String> FindDifferencies(object entity1, object entity2)
		{
			List<String> errorList = new List<String>();
			FindDifferencies(entity1, entity2, "root", errorList);
			return errorList;
		}

		private void FindDifferencies(object entity1, object entity2, String path, ICollection<string> errorList)
		{
			foreach (PropertyInfo pinfo in entity1.GetType().GetProperties(BindingFlagsForReflection))
			{
				if (IgnoreList.Contains(pinfo.Name)) continue;
				Object value1 = pinfo.GetValue(entity1, new Object[] { });
				Object value2 = pinfo.GetValue(entity2, new Object[] { });
				string name = pinfo.Name;
				CompareValue(value1, value2, errorList, name, path, pinfo.PropertyType);
			}
			foreach (FieldInfo pinfo in entity1.GetType().GetFields(BindingFlagsForReflection))
			{
				if (pinfo.Name.Contains("BackingField")) continue;
				if (IgnoreList.Contains(pinfo.Name)) continue;
				Object value1 = pinfo.GetValue(entity1);
				Object value2 = pinfo.GetValue(entity2);
				string name = pinfo.Name;
				CompareValue(value1, value2, errorList, name, path, pinfo.FieldType);
			}
		}

		private void CompareValue(object value1, object value2, ICollection<string> errorList, string name, String path, Type type)
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
			else if (TypeIsEnumerable(type))
			{
				CompareIEnumerable(value1, value2, errorList, name, path + "." + name);
			}
			else
			{
				FindDifferencies(value1, value2, path + "." + name, errorList);
			}
		}

		private void CompareIEnumerable(object value1, object value2, ICollection<string> errorList, String name, string path)
		{
			IEnumerable en1 = value1 as IEnumerable;
			IEnumerable en2 = value2 as IEnumerable;
			IEnumerator enumerator1 = en1.GetEnumerator();
			IEnumerator enumerator2 = en2.GetEnumerator();
			Boolean movenext1, movenext2;
			do
			{
				movenext1 = enumerator1.MoveNext();
				movenext2 = enumerator2.MoveNext();
				if (movenext1 != movenext2)
				{
					errorList.Add(String.Format("{0} has different number of elements", path));
				}
				else if (movenext1 && movenext2)
				{
					//Compare the element
					CompareValue(enumerator1.Current, enumerator2.Current, errorList, name, path, enumerator1.Current.GetType());
				}
			} while (movenext1 && movenext2);
		}

		private static bool TypeIsDirectlyComparable(Type type)
		{
			return type.IsPrimitive || type == typeof(String);
		}

		private static bool TypeIsEnumerable(Type type)
		{
			return typeof(IEnumerable).IsAssignableFrom(type);
		}

		public List<String> IgnoreList { get; set; }

		public ObjectComparer()
		{
			IgnoreList = new List<string>();
		}

		public void AddIgnore(string value)
		{
			IgnoreList.Add(value);
		}
	}
}
