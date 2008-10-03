using System;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Helpers;

namespace DotNetMarche.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public static class GlobalTransactionManager
	{
		private const String TransactionScopeKey = "FF2DB3E7-1D02-4f60-9F78-CCD6DF7D2841";

		/// <summary>
		/// Begin a transaction.
		/// </summary>
		/// <returns></returns>
		public static DisposableAction BeginTransaction()
		{
			CurrentContext.SetData(TransactionScopeKey, new Transaction(DateTime.Now));
			return new DisposableAction(CloseCurrentTransaction);
		}

		/// <summary>
		/// Tells if a transaction is active.
		/// </summary>
		public static Boolean IsInTransaction
		{
			get { return null != CurrentContext.GetData(TransactionScopeKey); }
		}

		private static Transaction CurrentTransaction
		{
			get { return (Transaction) CurrentContext.GetData(TransactionScopeKey); }
		}

		public static void DoomCurrentTransaction()
		{
			Verify.That(IsInTransaction, "Cannot doom the transaction because there is not an active transaction");
			CurrentTransaction.Doom();
		}

		/// <summary>
		/// Close current transaction, this function is private because is used inside a DisposableAction.
		/// We need to check if are inside an exception handler to doom the transaction, then we need to 
		/// remove the global transaction from the context and committ it.
		/// </summary>
		private static void CloseCurrentTransaction()
		{
			Verify.That(IsInTransaction, "Cannot doom the transaction because there is not an active transaction");
			if (Utils.ExceptionUtils.IsInExceptionHandler())
				CurrentTransaction.Doom();
			Transaction currentTransaction = (Transaction) CurrentContext.GetData(TransactionScopeKey);
			CurrentContext.ReleaseData(TransactionScopeKey);
			currentTransaction.Complete();
		}

		public static void Enlist(Action<Boolean> completeTransactionCallback)
		{
			Verify.That(IsInTransaction, "Cannot doom the transaction because there is not an active transaction");
			CurrentTransaction.Enlist(completeTransactionCallback);
		}
	}
}