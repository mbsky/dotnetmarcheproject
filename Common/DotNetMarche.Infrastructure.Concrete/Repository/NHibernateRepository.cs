using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;

namespace DotNetMarche.Infrastructure.Concrete.Repository
{
	public class NHibernateRepository<T> : IRepository<T>
	{

		#region Inner management

		/// <summary>
		/// Each repository can have a different configuration file name
		/// to support more than one database.
		/// </summary>
		public String ConfigurationFileName { get; set; }

		#endregion

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
