using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMarche.TestHelpers.BaseTests
{
	//#region Attributes

	//[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	//public class TestWithRhinoMockAttribute : Attribute { }

	//[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	//public class TestWithInMemoryConfiguration : Attribute { }

	//#endregion

	public abstract class BaseTestFixtureWithHelper : BaseTestFixture
	{

		protected List<ITestHelper> Helpers = new List<ITestHelper>();

		#region Rhino mocks helper

		private readonly Attribute[] customAttributes;


		#endregion

		protected BaseTestFixtureWithHelper()
		{
			Type type = this.GetType();
			customAttributes = Attribute.GetCustomAttributes(type);
			foreach (ITestHelperAttribute attribute in
				customAttributes.OfType<ITestHelperAttribute>())
			{
				Helpers.Add(attribute.Create());
			}
		}

		protected override void OnTestFixtureSetUp()
		{
			foreach (var helper in Helpers) helper.FixtureSetUp(this);
			base.OnTestFixtureSetUp();
		}

		protected override void OnSetUp()
		{
			foreach (var helper in Helpers) helper.SetUp(this);
			base.OnSetUp();
		}

		protected override void OnTearDown()
		{
			foreach (var helper in Helpers) helper.TearDown(this);
			base.OnTearDown();
		}

		protected override void OnTestFixtureTearDown()
		{
			foreach (var helper in Helpers) helper.FixtureTearDown(this);
			base.OnTestFixtureTearDown();
		}
	}
}
