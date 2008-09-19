using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;

namespace DotNetMarche.NHibernate
{
	public class NHibernateRepository<T> : IRepository<T>
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
