using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.TestHelpers.BaseTests
{
	/// <summary>
	/// A test helper is an object that can be used to interact
	/// with the test
	/// </summary>
	public interface ITestHelper
	{
		void FixtureSetUp(IBaseTestFixture fixture);
		void SetUp(IBaseTestFixture fixture);
		void TearDown(IBaseTestFixture fixture);
		void FixtureTearDown(IBaseTestFixture fixture);
	}

	public interface ITestHelperAttribute
	{
		ITestHelper Create();
	}
}
