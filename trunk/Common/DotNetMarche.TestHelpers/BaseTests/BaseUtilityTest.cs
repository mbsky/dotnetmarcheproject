using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure;
using NUnit.Framework;

namespace DotNetMarche.TestHelpers.BaseTests
{
	public abstract class BaseUtilityTest
	{
		#region Initialization and private

		private List<IDisposable> singleTestDisposableList;
		private List<Action> singleTestTearDownActions;		
		private List<IDisposable> fixtureDisposableList;
		private List<Action> fixtureTearDownActions;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			singleTestDisposableList = new List<IDisposable>();
			singleTestTearDownActions = new List<Action>();
			fixtureDisposableList = new List<IDisposable>();
			fixtureTearDownActions = new List<Action>();
			OnTestFixtureSetUp();
		}

		protected virtual void OnTestFixtureSetUp()
		{
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OnTestFixtureTearDown();
			Boolean ErrorOnDispose = false;
			fixtureDisposableList.ForEach(d =>
			{
				try
				{
					d.Dispose();
				}
				catch (Exception)
				{
					ErrorOnDispose = true;
				}
			});
			Boolean ErrorOnTearDownAction = false;
			fixtureTearDownActions.ForEach(a =>
			{
				try
				{
					a();
				}
				catch (Exception)
				{
					ErrorOnTearDownAction = true;
				}
			});
			Assert.That(ErrorOnDispose == false, "Some disposable object generates errors during Fixture Tear Down");
			Assert.That(ErrorOnTearDownAction == false, "Some tear down action generates errors during Fixture Tear Down");
		}

		protected virtual void OnTestFixtureTearDown()
		{
		}

		[SetUp]
		public void SetUp()
		{
			singleTestDisposableList.Clear();
			singleTestTearDownActions.Clear();
			OnSetUp();
		}

		protected virtual void OnSetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
			OnTearDown();
			Boolean ErrorOnDispose = false;
			singleTestDisposableList.ForEach(d =>
         	{
         		try
         		{
         			d.Dispose();
         		}
         		catch (Exception)
         		{
         			ErrorOnDispose = true;
         		}
         	});
			Boolean ErrorOnTearDownAction = false;
			singleTestTearDownActions.ForEach(a =>
			{
				try
				{
					a();
				}
				catch (Exception)
				{
					ErrorOnTearDownAction = true;
				}
			});
			Assert.That(ErrorOnDispose == false, "Some disposable object generates errors during Test Tear Down");
			Assert.That(ErrorOnTearDownAction == false, "Some tear down action generates errors during Test Tear Down");
		}

		protected virtual void OnTearDown()
		{
		}

		#endregion

		#region Cleanup management

		protected void DisposeAtTheEndOfTest(IDisposable disposableObject)
		{
			singleTestDisposableList.Add(disposableObject);
		}		

		protected void DisposeAtTheEndOfFixture(IDisposable disposableObject)
		{
			fixtureDisposableList.Add(disposableObject);
		}		
		
		protected void ExecuteAtTheEndOfTest(Action action)
		{
			singleTestTearDownActions.Add(action);
		}

		protected void ExecuteAtTheEndOfFixture(Action action)
		{
			fixtureTearDownActions.Add(action);
		}

		#endregion
	}
}
