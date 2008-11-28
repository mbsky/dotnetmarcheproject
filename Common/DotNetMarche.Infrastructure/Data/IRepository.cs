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
		/// Get the object by Id, the id is an object. Since the concept of 
		/// Proxy or ghost object is used by NHiberate and ORM this methods
		/// permits to get a proxy and not a real object
		/// </summary>
		/// <param name="id"></param>
		/// <param name="getProxy">If true the repository does not retrieve the
		/// object from the db if it is not in the context, but creates a proxy.</param>
		/// <returns></returns>
		T GetById(Object id, Boolean getProxy);

		/// <summary>
		/// This is the base version that gets real object and not a proxy
		/// <see cref="GetById(object,bool)"/>
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(Object id);

		#region Lifecycle management

		/// <summary>
		/// Save the object into the storage medium
		/// </summary>
		/// <param name="obj"></param>
		void Save(T obj);

		/// <summary>
		/// Update the object into the storage medium
		/// </summary>
		/// <param name="obj"></param>
		void Update(T obj);

		/// <summary>
		/// Save or update the object into storage medium
		/// </summary>
		/// <param name="obj"></param>
		void SaveOrUpdate(T obj);

		#endregion

		#region Query interface

		/// <summary>
		/// provide access with the LINQ to Repository concrete interface.
		/// </summary>
		/// <returns></returns>
		IQueryable<T> Query();

		/// <summary>
		/// Uses the internal query parser.
		/// </summary>
		/// <param name="queryText"></param>
		/// <returns></returns>
		IEnumerable<T> Query(String queryText);

		#endregion
	}
}
