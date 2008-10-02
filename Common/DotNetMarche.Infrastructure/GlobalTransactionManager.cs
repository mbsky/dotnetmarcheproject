using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Base
{
	/// <summary>
	/// 
	/// </summary>
	public static class GlobalTransactionManager
	{
		private const String TransactionScopeKey = "FF2DB3E7-1D02-4f60-9F78-CCD6DF7D2841";
		private class Transaction
		{
			public DateTime TransactionStart { get; set; }

			public Transaction(DateTime transactionStart)
			{
				TransactionStart = transactionStart;
			}

			private readonly List<Action<Boolean>> disposeList = new List<Action<Boolean>>();

			/// <summary>
			/// Enlist a delegate into the transaction, the transaction will call the
			/// list of action passing true if the transaction is committed or false
			/// if the transaction is rollback.
			/// </summary>
			/// <param name="disposeAction"></param>
			public void Enlist(Action<Boolean> disposeAction)
			{
				disposeList.Add(disposeAction);
			}

		}

		/// <summary>
		/// Begin a transactionScope.
		/// </summary>
		/// <returns></returns>
		public static DisposableAction BeginTransactionScope()
		{
			CurrentContext.SetData(TransactionScopeKey, new Transaction(DateTime.Now));
			return new DisposableAction(() => CurrentContext.ReleaseData(TransactionScopeKey));
		}
	}
}
