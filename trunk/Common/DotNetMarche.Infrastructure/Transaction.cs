using System;
using System.Collections.Generic;
using DotNetMarche.Infrastructure.Logging;

namespace DotNetMarche.Infrastructure
{
	/// <summary>
	/// This is the class that represents a transaction, it contains the 
	/// date of transaction start as well as other function needed to 
	/// enlist component in the transaction.
	/// </summary>
	public class Transaction
	{
		public DateTime TransactionStart { get; set; }

		/// <summary>
		/// Tells if the transaction is to be committed or rollbacked
		/// </summary>
		private Boolean IsDoomed { get; set; }

		public Transaction(DateTime transactionStart)
		{
			TransactionStart = transactionStart;
		}

		private readonly List<Action<Boolean>> completeTransactionCallbacks = new List<Action<Boolean>>();

		/// <summary>
		/// Enlist a delegate into the transaction, the transaction will call the
		/// list of action passing true if the transaction is committed or false
		/// if the transaction is rollback.
		/// </summary>
		/// <param name="transactionCompleted"></param>
		public void Enlist(Action<Boolean> transactionCompleted)
		{
			completeTransactionCallbacks.Add(transactionCompleted);
		}

		/// <summary>
		/// You can only Doom a transaction, you cannot take a doomed transaction
		/// and make it to committ.
		/// </summary>
		internal void Doom()
		{
			IsDoomed = true;
		}

		internal void Complete()
		{
			InnerCommitt(!IsDoomed);
		}

		/// <summary>
		/// Committ or rollback the transaction.
		/// </summary>
		/// <param name="transactionOk"></param>
		private void InnerCommitt(bool transactionOk)
		{
			foreach(Action<Boolean> action in completeTransactionCallbacks)
			{
				try
				{
					action(transactionOk);
				}
				catch (Exception ex)
				{
					Logger.ErrorFormat("Exception during transaction {0}", ex, transactionOk ? "committ" : "rollback");
				}
			}
		}
	}
}