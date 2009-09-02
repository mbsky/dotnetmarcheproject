using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;

namespace DotNetMarche.TestHelpers.BaseTests.Helpers
{
	public class CustomConfiguration : BaseTestHelper
	{
		public const String RegistryTestContextKey = "registyry";

		#region ITestHelper Members

		public override void FixtureSetUp(IBaseTestFixture fixture)
		{
			InMemoryConfigurationRegistry configurationRegistry = new InMemoryConfigurationRegistry();
			fixture.DisposeAtTheEndOfFixture(ConfigurationRegistry.Override(configurationRegistry));
			fixture.SetIntoTestContext(RegistryTestContextKey, configurationRegistry);
			
			base.FixtureSetUp(fixture);
		}

		#endregion
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class CustomConfigurationAttribute : Attribute, ITestHelperAttribute
	{
		
		#region ITestHelperAttribute Members

		public ITestHelper Create()
		{
			return new CustomConfiguration();
		}

		#endregion
	}
}
