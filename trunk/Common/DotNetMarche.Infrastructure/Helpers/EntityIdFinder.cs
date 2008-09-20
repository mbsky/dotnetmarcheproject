using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Infrastructure.Helpers
{
	/// <summary>
	/// The InMemory repository or other form of repository needs to 
	/// know witch field of an entity is the ID, or the primary key
	/// of the repository. This class helps to find the id of the entity
	/// through reflection and some "convention over configuration".
	/// </summary>
	public static class EntityIdFinder
	{
		private const BindingFlags BindingFlagsToReflectEntities =
			BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

		private const string MsgNotHaveIdPropertyException = "Entity of type {0} does not have Id property and cannot be used with inmemoryrepository";

		private static Hashtable IdList = new Hashtable();

		private static RetrieveValue GetRetrieveValueForType(Type type)
		{
			return (RetrieveValue)IdList[type];
		}

		public static Object GetIdValueFromEntity<T>(T entity)
		{
			if (!IdList.ContainsKey(typeof(T))) FindIdByReflection<T>();
			return (GetRetrieveValueForType(typeof(T)).GetValue(entity));
		}

		private static void FindIdByReflection<T>()
		{
			//This type was never investigated.
			RetrieveValue Id = FindRetrieveValueForType<T>();
			if (Id == null)
			{
				//The entity does not have any property called Id, throw exception.
				throw new ArgumentException(String.Format(MsgNotHaveIdPropertyException, typeof(T)), "entity");
			}
			lock (IdList)
			{
				IdList.Add(typeof(T), Id);
			}
		}

		private static RetrieveValue FindRetrieveValueForType<T>()
		{
			PropertyInfo pinfo = typeof(T).GetProperty("Id", BindingFlagsToReflectEntities);
			if (pinfo != null) return new PropertyInfoRetrieveValue(pinfo);
			FieldInfo finfo = typeof(T).GetField("Id", BindingFlagsToReflectEntities);
			if (finfo != null) return new FieldInfoRetrieveValue(finfo);
			return null;
		}

		#region InternalClasses

		private abstract class RetrieveValue
		{
			public abstract Object GetValue(Object entity);
		}

		private class PropertyInfoRetrieveValue : RetrieveValue
		{
			public PropertyInfo info;

			public PropertyInfoRetrieveValue(PropertyInfo info)
			{
				this.info = info;
			}

			public override Object GetValue(Object entity)
			{
				return info.GetValue(entity, Constants.EmptyObjectArray);
			}
		}

		private class FieldInfoRetrieveValue : RetrieveValue
		{
			public FieldInfo info;

			public FieldInfoRetrieveValue(FieldInfo info)
			{
				this.info = info;
			}

			public override Object GetValue(Object entity)
			{
				return info.GetValue(entity);
			}
		}
		#endregion
	}
}
