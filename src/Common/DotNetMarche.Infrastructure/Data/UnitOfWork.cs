using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Data
{
	/// <summary>
	/// This is the class that manages the concept of UnitOfWork for
	/// the repositories. the concept of UnitOfWork is different from that
	/// of a transaction, a unit of work can be longer than a transaction and
	/// it is strictly related to the repository pattern.
	/// </summary>
	public static class UnitOfWork
	{
		private const String UnitOfWorkScopeKey = "482B5186-B41B-4f84-91F4-7C8E5835758F";

		public static Boolean IsInUnitOfWork
		{
			get { return null != CurrentContext.GetData(UnitOfWorkScopeKey); }
		}

		/// <summary>
		/// You can start nested disposable action.
		/// </summary>
		/// <returns></returns>
		public static UnitOfWorkToken Start()
		{
			throw new NotImplementedException();
			//if (IsInUnitOfWork)
		}
	}
}
