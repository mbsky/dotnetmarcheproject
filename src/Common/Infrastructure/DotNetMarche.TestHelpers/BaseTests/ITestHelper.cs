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
		void FixtureSetUp();
		void SetUp();
		void TearDown();
		void FixtureTearDown();
	}
}
