using DotNetMarche.PhotoAlbum.MsUnitTest.MVCUtils;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data;

namespace DotNetMarche.PhotoAlbum.MsUnitTest
{


	/// <summary>
	///This is a test class for MasterLogicTest and is intended
	///to contain all MasterLogicTest Unit Tests
	///</summary>
	[TestClass()]
	public class MasterLogicTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for CreateMenu
		///</summary>
[TestMethod()]
[DeploymentItem(@".\SampleFiles\BaseMenu1.xml")]
public void CreateMenuTest()
{
	PrivateObject param0 = new PrivateObject(new MasterLogic(new MyTestUrlHelper()));
	MasterLogic_Accessor target = new MasterLogic_Accessor(param0);
	string menuFileName = @"BaseMenu1.xml";
	MenuItem expected = new MenuItem("TEST");
	target.CreateMenu(menuFileName);
	Assert.AreEqual(expected.Text, "TEST");
}
	}
}
