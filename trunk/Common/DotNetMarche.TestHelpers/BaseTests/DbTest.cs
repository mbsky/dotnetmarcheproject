using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NUnit.Framework;

namespace DotNetMarche.TestHelpers.BaseTests
{
	public class DbTest
	{
		private TransactionScope scope = new TransactionScope();

		[SetUp]
		public void SetUp()
		{
			OnTestSetUp();
		}

		protected virtual void OnTestSetUp()
		{
			scope = new TransactionScope();
		}

		[TearDown]
		public void TearDown()
		{
			OnTestTearDown();
		}

		protected virtual void OnTestTearDown()
		{
			scope.Dispose();
		}
	}
}
