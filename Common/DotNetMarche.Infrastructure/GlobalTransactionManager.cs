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

		private static Transaction CurrentTransaction
		{
			get { return (Transaction)CurrentContext.GetData(TransactionScopeKey); }
		}

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
			Transaction currentTransaction = (Transaction)CurrentContext.GetData(TransactionScopeKey);
			CurrentContext.ReleaseData(TransactionScopeKey);
			currentTransaction.Complete();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="completeTransactionCallback"></param>
		/// <returns>A disposable action, if we are in a transaction the action does nothing, but
		/// if we are not in a transaction the action will committ or dispose the transaction.</returns>
		public static TransactionToken Enlist(Action<Boolean> completeTransactionCallback)
		{
			if (IsInTransaction)
			{
				CurrentTransaction.Enlist(completeTransactionCallback);
				return new TransactionToken();
			}
			else
			{
				//We are not in a transaction
				return new TransactionTokenForNoGlobalTransaction(completeTransactionCallback);
			}
		}

		#region Inner Classes

		/// <summary>
		/// This is the class that the caller will use to communicate with the transactionmanager.
		/// </summary>
		public class TransactionToken : IDisposable
		{

			public virtual void Doom()
			{
				GlobalTransactionManager.DoomCurrentTransaction(); 
			}

			#region IDisposable Members

			public void Dispose()
			{
				InnerDispose();
			}

			protected virtual void InnerDispose() {}

			#endregion
		}

		/// <summary>
		/// When we are not in a global transaction this object manages all the transaction.
		/// </summary>
		public class TransactionTokenForNoGlobalTransaction : TransactionToken
		{
			private readonly Action<Boolean> disposeAction;
			private Boolean IsDoomed;

			public TransactionTokenForNoGlobalTransaction(Action<Boolean> disposeAction)
			{
				this.disposeAction = disposeAction;
			}

			public override void Doom()
			{
				IsDoomed = true;
			}

			protected override void InnerDispose()
			{
				if (null != disposeAction) disposeAction(!IsDoomed);
			}
		}

		#endregion
	}
}