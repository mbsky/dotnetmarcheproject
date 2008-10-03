using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Helpers;
using DotNetMarche.Utils;

namespace DotNetMarche.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public static class GlobalTransactionManager
	{
		#region Basic Transaction Function

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
		/// Enlist in the transaction, enlisting consists only in passing a <see cref="Action<Boolean>"/> that 
		/// will be called when the transaction will committ or rollback witha  parameter of true and false respectively.
		/// </summary>
		/// <param name="completeTransactionCallback">The callback that will be called when the transaction will end, the 
		/// value of the Boolean parameter will be true if the transaction should committ and false if the transaction
		/// should rollback.</param>
		/// <returns>A disposable action, if we are in a transaction the action does nothing, but
		/// if we are not in a transaction the action will committ or dispose the transaction.</returns>
		public static TransactionToken Enlist(Action<Boolean> completeTransactionCallback)
		{
			if (IsInTransaction)
			{
				//Let the current transaction handle this.
				CurrentTransaction.Enlist(completeTransactionCallback);
				return new TransactionToken();
			}
			else
			{
				//We are not in a transaction
				return new TransactionTokenForNoGlobalTransaction(completeTransactionCallback);
			}
		}

		#endregion

		#region TransactionContext

		public static class TransactionContext
		{
			public static void Set(String key, Object obj)
			{
				Verify.That(IsInTransaction, "Cannot doom the transaction because there is not an active transaction");
				CurrentTransaction.TransactionContext[key] = obj;
			}

			public static Object Get(String key)
			{
				Verify.That(IsInTransaction, "Cannot doom the transaction because there is not an active transaction");
				return CurrentTransaction.TransactionContext.SafeGet(key);
			}
		}
		

		#endregion

		#region Inner Classes

		/// <summary>
		/// This is the class that the caller will use to communicate with the transactionmanager.
		/// This class is created when we are in global transaction so we are sure that there is 
		/// a global connection and that the real committ action will be handled by the main transaction.
		/// </summary>
		public class TransactionToken : IDisposable
		{
			public virtual void SetInTransactionContext(String key, Object obj)
			{
				CurrentTransaction.TransactionContext[key] = obj;
			}

			public virtual Object GetFromTransactionContext(String key)
			{
				return CurrentTransaction.TransactionContext.SafeGet(key);
			}

			public virtual void Doom()
			{
				DoomCurrentTransaction(); 
			}

			#region IDisposable Members

			public void Dispose()
			{
				InnerDispose();
			}

			/// <summary>
			/// We should not to nothing inside this dispose, because the real committ action was
			/// set in the global transaction manager.
			/// </summary>
			protected virtual void InnerDispose() {}

			#endregion
		}

		/// <summary>
		/// When we are not in a global transaction this object manages a transaction
		/// that lasts only for the current call.
		/// </summary>
		public class TransactionTokenForNoGlobalTransaction : TransactionToken
		{
			private readonly Action<Boolean> disposeAction;
			private Boolean IsDoomed;
			private readonly Dictionary<String, Object> localContext = new Dictionary<String, Object>();

			public override void SetInTransactionContext(String key, Object obj)
			{
				localContext[key] = obj;
			}

			public override Object GetFromTransactionContext(String key)
			{
				return localContext.SafeGet(key);
			}


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