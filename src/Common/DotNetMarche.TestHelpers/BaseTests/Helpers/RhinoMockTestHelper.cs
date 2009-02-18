using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;

namespace DotNetMarche.TestHelpers.BaseTests.Helpers
{
	public class RhinoMockTestHelper : ITestHelper 
	{
		public MockRepository MockRepository { get; set; }

		#region ITestHelper Members

		public void FixtureSetUp()
		{
		}

		public void SetUp()
		{
			MockRepository = new MockRepository();
		}

		public void TearDown()
		{
			MockRepository.VerifyAll();
			MockRepository = null;
		}

		public void FixtureTearDown()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
