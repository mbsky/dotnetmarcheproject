using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Data;
using NHibernate;
using NHibernate.Linq;

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

		#endregion
	}
}
