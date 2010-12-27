using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Validator.Utils
{
	/// <summary>
	/// 
	/// </summary>
	internal static class CollectionHelper
	{
		/// <summary>
		/// Determines whether a type is a subclass of a generic class or a generic interface
		/// </summary>
		/// <param name="typeToCheck">The generic.</param>
		/// <param name="typeLookedFor">To check.</param>
		/// <returns>
		/// 	<c>true</c> if [is subclass of raw generic] [the specified generic]; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsGenericSubclassOf(Type typeToCheck, Type typeLookedFor)
		{
			if (typeToCheck == typeLookedFor) return true;

			if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == typeLookedFor)
				return true;

			if (typeToCheck.ImplementsInterface(typeLookedFor))
				return true;

			if (typeToCheck.BaseType != null)
				return IsGenericSubclassOf(typeToCheck.BaseType, typeLookedFor);

			return false;
		}

		/// <summary>
		/// VErify if a type implements a generic interface
		/// </summary>
		/// <param name="typeToCheck">The type to check.</param>
		/// <param name="interfaceToLookFor">The interface to look for.</param>
		/// <returns></returns>
		public static Boolean ImplementsInterface(this Type typeToCheck, Type interfaceToLookFor)
		{
			return typeToCheck.GetInterface(interfaceToLookFor.Name) != null;
		}

		/// <summary>
		///Gets from an IEnumerable{T} the type T.
		///  </summary>
		/// <param name="typeToCheck">The type to check.</param>
		/// <returns></returns>
		public static Type GetGenericTypeOfIEnumerable(this Type typeToCheck)
		{
			if (!typeToCheck.ImplementsInterface(typeof(IEnumerable<>)))
				return null;
			var genTypes = typeToCheck.GetGenericArguments();
			return genTypes.FirstOrDefault();
		}
	}
}
