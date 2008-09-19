using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Data
{
	public static class Repository<T>
	{

		#region Static constructors and variables

		private static IRepository<T> baseRepo;
		static Repository()
		{
			baseRepo = IoC.Resolve<IRepository<T>>();
		}

		#endregion

		#region Testing Routines

		internal static DisposableAction Override(IRepository<T> overrideGlobal)
		{
			IRepository<T> actualRepository = baseRepo;
			actualRepository = overrideGlobal;
			return new DisposableAction(() => baseRepo = actualRepository);
		}

		#endregion

		public static T GetById(object id)
		{
			return baseRepo.GetById(id);
		}

		public static void Save(T obj)
		{
			baseRepo.Save(obj);
		}

		public static void Update(T obj)
		{
			baseRepo.Update(obj);
		}

		public static void SaveOrUpdate(T obj)
		{
			baseRepo.SaveOrUpdate(obj);
		}

		public static IQueryable<T> Query()
		{
			return baseRepo.Query();
		}

	}
}
