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
		private const string MsgUnsupportedIdType = "The entity has an id of type 0}, that have no generator associated.";

		private static Hashtable IdList = new Hashtable();

		private static RetrieveValue GetRetrieveValueForType(Type type)
		{
			return ((TypeInformation)IdList[type]).retriever;
		}

		private static IdGenerator GetIdGeneratorForType(Type type)
		{
			return ((TypeInformation)IdList[type]).generator;
		}

		public static Object GetIdValueFromEntity<T>(T entity)
		{
			if (!IdList.ContainsKey(typeof(T))) FindIdByReflection<T>();
			return (GetRetrieveValueForType(typeof(T)).GetValue(entity));
		}

		/// <summary>
		/// Set the new value based on the default generator for the type
		/// of id.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		public static void SetIdValueForEntity<T>(T entity)
		{
			if (!IdList.ContainsKey(typeof(T))) FindIdByReflection<T>();
			GetRetrieveValueForType(typeof(T)).SetValue(entity, GetIdGeneratorForType(typeof(T)).Generate());
		}	
		
		public static void SetIdValueForEntity<T>(T entity, Object value)
		{
			if (!IdList.ContainsKey(typeof(T))) FindIdByReflection<T>();
			GetRetrieveValueForType(typeof(T)).SetValue(entity, value);
		}

		private static void FindIdByReflection<T>()
		{
			//This type was never investigated.
			TypeInformation Id = FindRetrieveValueForType<T>();
			if (Id == null)
			{
				//The entity does not have any property called Id, throw exception.
				throw new ArgumentException(String.Format(MsgNotHaveIdPropertyException, typeof(T)));
			}
			lock (IdList)
			{
				IdList.Add(typeof(T), Id);
			}
		}

		private static TypeInformation FindRetrieveValueForType<T>()
		{
			PropertyInfo pinfo = typeof(T).GetProperty("Id", BindingFlagsToReflectEntities);
			if (pinfo != null) return new TypeInformation(
				new PropertyInfoRetrieveValue(pinfo), FindGeneratorForType(pinfo.PropertyType));
			FieldInfo finfo = typeof(T).GetField("Id", BindingFlagsToReflectEntities);
			if (finfo != null) return new TypeInformation(
				new FieldInfoRetrieveValue(finfo), FindGeneratorForType(finfo.FieldType));
			return null;
		}

		private static IdGenerator FindGeneratorForType(Type type)
		{
			if (type == typeof(Int32))
				return new SequenceIdGenerator();
			else
				throw new NotSupportedException(String.Format(MsgUnsupportedIdType, type));
		}

		#region InternalClasses

		private abstract class RetrieveValue
		{
			public abstract Object GetValue(Object entity);

			public abstract void SetValue(object entity, object value);
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

			public override void SetValue(object entity, object value)
			{
				info.SetValue(entity, value, Constants.EmptyObjectArray);
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

			public override void SetValue(object entity, object value)
			{
				info.SetValue(entity, value);
			}
		}
		#endregion

		#region Internal structure

		private class TypeInformation
		{
			internal RetrieveValue	retriever;
			internal IdGenerator generator;

			public TypeInformation(RetrieveValue retriever, IdGenerator generator)
			{
				this.retriever = retriever;
				this.generator = generator;
			}
		}

		#endregion
	}
}
