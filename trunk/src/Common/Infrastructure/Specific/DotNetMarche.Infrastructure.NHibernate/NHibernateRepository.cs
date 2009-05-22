using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;
using NHibernate;
using NHibernate.Linq;
using DotNetMarche.Utils.Linq;

namespace DotNetMarche.Infrastructure.NHibernate
{
	public class NHibernateRepository<T> : IRepository<T>
	{

		#region Inner management

		/// <summary>
		/// Each repository can have a different configuration file name
		/// to support more than one database.
		/// </summary>
		public String ConfigurationFileName { get; set; }

		private ISession CurrentSession
		{
			get { return NHibernateSessionManager.GetSessionFor(ConfigurationFileName); }
		}
		#endregion

		#region IRepository<T> Members

		public T GetById(object id)
		{
			return CurrentSession.Get<T>(id);
		}

		public T GetById(object id, Boolean getProxy)
		{
			if (!getProxy)
				return GetById(id);
			else
				return CurrentSession.Load<T>(id);
		}

		public void Save(T obj)
		{
			CurrentSession.Save(obj);
		}

		public void Update(T obj)
		{
			CurrentSession.Update(obj);
		}

		public void SaveOrUpdate(T obj)
		{
			CurrentSession.SaveOrUpdate(obj);
		}

		public IQueryable<T> Query()
		{
			return CurrentSession.Linq<T>();
		}

		public IEnumerable<T> Query(String queryText)
		{
			IOrderedQueryable<T> queryable = CurrentSession.Linq<T>();
			return queryable.Where(queryText);
		}

		#endregion
	}
}