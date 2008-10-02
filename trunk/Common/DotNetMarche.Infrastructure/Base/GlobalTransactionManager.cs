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
		private class TransactionToken
		{
			public DateTime TransactionStart { get; set; }

			public TransactionToken(DateTime transactionStart)
			{
				TransactionStart = transactionStart;
			}
		}

		/// <summary>
		/// Begin a transactionScope.
		/// </summary>
		/// <returns></returns>
		public static DisposableAction BeginTransactionScope()
		{
			throw new NotImplementedException();
		}
	}
}
