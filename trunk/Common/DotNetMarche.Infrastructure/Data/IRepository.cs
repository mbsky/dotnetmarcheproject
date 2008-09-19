using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Data
{
	/// <summary>
	/// This is the main interface of a repository pattern.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepository<T>
	{
		/// <summary>
		/// Get the object by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(Object id);
		void Save(T obj);
		void Update(T obj);
		void SaveOrUpdate(T obj);

		IQueryable<T> Query();
	}
}
