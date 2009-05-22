using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure
{
	/// <summary>
	/// When a transaction is closing we need to tell if the transaction 
	/// is to be rollbacked or committed
	/// </summary>
	public class TransactionClosingEventArgs : EventArgs
	{
		public Boolean IsDoomed { get; private set; }

		public TransactionClosingEventArgs(bool isDoomed)
		{
			IsDoomed = isDoomed;
		}
	}

	public class TransactionClosedEventArgs : EventArgs
	{
		public Boolean IsDoomed { get; private set; }

		public TransactionClosedEventArgs(bool isDoomed)
		{
			IsDoomed = isDoomed;
		}
	}
}
