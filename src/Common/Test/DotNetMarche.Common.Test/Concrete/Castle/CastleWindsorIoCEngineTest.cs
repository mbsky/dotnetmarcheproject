using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Concrete.Classes;
using DotNetMarche.Infrastructure.Castle;
using NUnit.Framework;


namespace DotNetMarche.Common.Test.Concrete.Castle
{
	[TestFixture]
	public class CastleWindsorIoCEngineTest
	{

		private CastleWindsorIoCEngine CastleTest1;
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{   
			CastleTest1 = new CastleWindsorIoCEngine("Concrete\\files\\CastleTest1.xml");
		} 

		#region Tests with configuration file 1
		 
		[Test]
		public void TestResolveBase()
		{
			IIoCBase obj = CastleTest1.Resolve<IIoCBase>();
			Assert.That(obj, Is.Not.Null);
			Assert.That(obj.Value, Is.EqualTo(10));
		}

		[Test]
		public void TestResolveWithName()
		{
			IIoCBase obj = CastleTest1.Resolve<IIoCBase>("BaseNamed");
			Assert.That(obj, Is.Not.Null);
			Assert.That(obj.Value, Is.EqualTo(12));
		}		
		
		[Test, ExpectedException()]
		public void TestResolveComponentWithUncompleteDependance()
		{
			IIoCChain obj = CastleTest1.Resolve<IIoCChain>();
		}

		[Test]
		public void TestResolveBaseWithExplicitDependance()
		{
			IIoCBase obj = CastleTest1.ResolveWithName<IIoCBase>("BaseNoParam", "needValue", 99);
			Assert.That(obj.Value, Is.EqualTo(99));
		}

		[Test]
		public void TestResolveDefaultWithExplicitDependance()
		{
			IIoCBase obj = CastleTest1.Resolve<IIoCBase>("needValue", 99);
			Assert.That(obj.Value, Is.EqualTo(99));
		}


		[Test, Explicit("Broken by castle in version 5888?")]
		public void TestResolveChainWithExplicitDependance()
		{
			IIoCChain obj = CastleTest1.Resolve<IIoCChain>("needValue", 98);
			Assert.That(obj.TheBase.Value, Is.EqualTo(98));
		}

		[Test, Explicit("Broken by castle in version 5888?")]
		public void TestResolveChainWithExplicitDependance2()
		{
			IIoCChain obj = CastleTest1.ResolveWithName<IIoCChain>("ChainDependant2", "needValue", 97);
			Assert.That(obj.TheBase.Value, Is.EqualTo(97));
		}

		#endregion
	}
}