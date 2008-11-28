using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.TestHelpers.BaseTests.Helpers;
using Rhino.Mocks;

namespace DotNetMarche.TestHelpers.BaseTests
{
	#region Attributes

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class TestWithRhinoMockAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class TestWithInMemoryConfiguration : Attribute { }

	#endregion

	public abstract class BaseUtilityTestWithHelper : BaseUtilityTest
	{

		private List<ITestHelper> helpers = new List<ITestHelper>();

		#region Override configuration

		protected InMemoryConfigurationRegistry configurationRegistry;
		protected InMemoryConfigurationRegistry Configuration
		{
			get { return configurationRegistry; }
		}

		#endregion

		#region Rhino mocks helper

		private readonly RhinoMockTestHelper mockHelper;
		private readonly Attribute[] customAttributes;

		protected MockRepository MockRepository
		{
			get { return mockHelper.MockRepository; }
		}

		#endregion

		protected BaseUtilityTestWithHelper()
		{
			Type type = this.GetType();
			customAttributes = Attribute.GetCustomAttributes(type);

			if (customAttributes.Any(a => a is TestWithRhinoMockAttribute))
			{
				mockHelper = new RhinoMockTestHelper();
				helpers.Add(mockHelper);
			}

		}

		protected override void OnTestFixtureSetUp()
		{
			if (customAttributes.Any(a => a is TestWithInMemoryConfiguration))
			{
				//Set a global in memory configuration to easy configuration
				configurationRegistry = new InMemoryConfigurationRegistry();
				DisposeAtTheEndOfFixture(ConfigurationRegistry.Override(configurationRegistry));
			}


			foreach (var helper in helpers) helper.FixtureSetUp();
			base.OnTestFixtureSetUp();
		}

		protected override void OnSetUp()
		{
			foreach (var helper in helpers) helper.SetUp();
			base.OnSetUp();
		}

		protected override void OnTearDown()
		{
			foreach (var helper in helpers) helper.TearDown();
			base.OnTearDown();
		}

		protected override void OnTestFixtureTearDown()
		{
			foreach (var helper in helpers) helper.FixtureSetUp();
			base.OnTestFixtureTearDown();
		}
	}
}
