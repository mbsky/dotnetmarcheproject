﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;

namespace DotNetMarche.Infrastructure.Concrete.Repository
{
	public class EntityFrameworkRepository<T> : IRepository<T>
	{
		#region IRepository<T> Members

		public T GetById(object id)
		{
			return GetById(id, false);
		}

		public T GetById(object id, Boolean getProxy)
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

		public IEnumerable<T> Query(String queryText)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
