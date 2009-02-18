using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DotNetMarche.TestHelpers.Fluent.Comparers
{
	public class EntityComparer {

		internal String CompareEntities(Object obj1, Object obj2) {
			StringBuilder sb = new StringBuilder();
			Boolean retvalue = CompareEntities(obj1, obj2, sb);
			return sb.ToString();
		}

		private List<String> mPropertyToCompare = null;
		public List<String> PropertiesToCompare {
			get { return mPropertyToCompare ?? (mPropertyToCompare = new List<string>()); }
		}

		internal Boolean CompareEntities(Object obj1, Object obj2, StringBuilder sb) {

			Type ty = obj1.GetType();
			FieldInfo[] fi = ty.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			Boolean retValue = true;
			for (Int32 I = 0; I < fi.Length; ++I) {

				if (mPropertyToCompare != null && !PropertiesToCompare.Contains(fi[I].Name))
					break;

				object value1 = fi[I].GetValue(obj1);
				object value2 = fi[I].GetValue(obj2);
				//When we have fixed length field in database we have padding with spaces
				if (fi[I].FieldType == typeof(string)) {
					if (!(value1 == null || value2 == null)) {
						value1 = ((string)value1).TrimEnd();
						value2 = ((string)value2).TrimEnd();
					}
				}
				//Compare IENumerable, but exclude the string that is enumerable but has own equals
				if (!(value1 is string) && value1 is IEnumerable) {
					IEnumerable list1 = value1 as IEnumerable;
					IEnumerable list2 = value2 as IEnumerable;
					IEnumerator enum1 = list1.GetEnumerator();
					IEnumerator enum2 = list2.GetEnumerator();
					while (MoveEnum(enum1, enum2, ref retValue)) {
						if (!CompareEntities(enum1.Current, enum2.Current, sb)) {
							retValue = false;
						}
					}
				}
				else {
					if (!CompareGenericObject(fi[I].Name, value1, value2, sb)) {
						retValue = false;
					}
				}

			}
			return retValue;
		}

		private static Boolean MoveEnum(IEnumerator enum1, IEnumerator enum2, ref Boolean retvalue) {
			Boolean move1 = enum1.MoveNext();
			Boolean move2 = enum2.MoveNext();
			if (move1 != move2) {
				retvalue = false;
			}
			return move1 && move2;
		}


		/// <summary>
		/// compare due oggetti generici e genera il messaggio di errore.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		private bool CompareGenericObject(String fieldName, object value1, object value2, StringBuilder sb) {
			String stringValue1 = value1 as string;
			if (stringValue1 != null) {
				if (String.Compare(value1 as string, value2 as string) != 0) {
					CreateGenericErrorMessage(value1, value2, sb, fieldName);
					return false;
				}
			}
			if (value1.GetType().IsClass) {
				return CompareEntities(value1, value2, sb);
			}
			else {
				if (!object.Equals(value1, value2)) {
					CreateGenericErrorMessage(value1, value2, sb, fieldName);
					return false;
				}
			}


			return true;
		}

		private static void CreateGenericErrorMessage(object value1, object value2, StringBuilder sb, string fieldName) {
			sb.AppendFormat("{0} is different expected {1} but was {2}",
			                fieldName, value1 ?? "<null>", value2 ?? "<null>");
		}
	}
}