using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.Data;
using NUnit.Framework;

namespace DotNetMarche.TestHelpers.BaseTests
{
	public class DbTest
	{
		private IDisposable scope;

		[SetUp]
		public void SetUp()
		{
			OnTestSetUp();
		}

		protected virtual void OnTestSetUp()
		{
			scope = GlobalTransactionManager.BeginTransaction(); 
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
