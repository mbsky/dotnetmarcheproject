using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.Infrastructure.Helpers;

namespace DotNetMarche.Infrastructure.Concrete
{
	/// <summary>
	/// used mainly for test purpose, this repository handle all 
	/// object in memory, it provide a quick way to simulate data
	/// without the use of a database.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class InMemoryRepository<T> : IRepository<T>
	{
		#region Constructors

		static InMemoryRepository()
		{
			LastGeneratedId = 0;
		}

		#endregion

		#region Internal structures

		protected Dictionary<Object, T> context = new Dictionary<Object, T>();

		/// <summary>
		/// Used for generation of Id.
		/// </summary>
		private static Int32 LastGeneratedId;

		#endregion

		#region IRepository<T> Members

		public T GetById(object id, Boolean getProxy)
		{
			if (!context.ContainsKey(id)) return default(T);
			return context[id];
		}

		public T GetById(object id)
		{
			return GetById(id, false);
		}

		public void Save(T obj)
		{
			EntityIdFinder.SetIdValueForEntity(obj);
			context.Add(EntityIdFinder.GetIdValueFromEntity(obj), obj);
		}

		public void Update(T obj)
		{
			throw new NotImplementedException();
		}

		public void SaveOrUpdate(T obj)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> Query()
		{
			throw new NotImplementedException();
		}

		 public IEnumerable<T> Query(String queryText)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}