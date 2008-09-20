using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;

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
		#region IRepository<T> Members

		public T GetById(object id)
		{
			throw new NotImplementedException();
		}

		public void Save(T obj)
		{
			throw new NotImplementedException();
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

		#endregion
	}
}