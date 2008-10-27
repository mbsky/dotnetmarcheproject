using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
			foreach (PropertyInfo pinfo in entity1.GetType().GetProperties(BindingFlagsForReflection))
			{
				Object value1 = pinfo.GetValue(entity1, new Object[] {});
				Object value2 = pinfo.GetValue(entity2, new Object[] {});
				if (!value1.Equals(value2))
				{
					ErrorList.Add(String.Format("Property {0} is different {1}!={2}", pinfo.Name, value1, value2));
				}
			}
			return ErrorList;
		}
	}
}
