using System;
using System.Collections.Generic;
using System.Text;
using Castle.Windsor;
using DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses;
using DotNetMarche.Infrastructure.Castle.AOP;
using NUnit.Framework;
using Rhino.Mocks;

namespace Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses {
	
	[TestFixture]
	public class TestHelpers {

		/// <summary>
		/// Test if the container is able to override an object.
		/// </summary>
		[Test]
		public void TestTestContainerBasic() {
            TestContainer container = new TestContainer(@"Concrete\Castle\AOP\HelperClasses\ConfigSample1.config");
			SomeClass sc = new SomeClass();
			ISomething smth = container.Resolve<ISomething>();
			Assert.AreNotEqual(sc, smth);
			container.AddServiceOverride(typeof(ISomething), sc);
			Assert.AreEqual(sc, container.Resolve<ISomething>());
		}

		[Test]
		public void TestTestContainerBasicNamedService() {
            TestContainer container = new TestContainer(@"Concrete\Castle\AOP\HelperClasses\ConfigSample1.config");
			SomeClass sc = new SomeClass();
			Assert.AreNotEqual(sc, container.Resolve("someclass"));
			container.AddNamedServiceOverride("someclass", sc);
			Assert.AreEqual(sc, container.Resolve("someclass"));
             
		}
         
		/// <summary>
		/// lets see if the test container is able to override the interceptor, this test is 
		/// useful because we need a way to test if the container really is able to 
		/// do aop interception.
		/// </summary>
		[Test]
		public void TestTestContainerAop() {
			MockRepository mock = new MockRepository();
			IAspect hmock = mock.CreateMock<IAspect>();
            hmock.PostCall(null, null);
			LastCall.IgnoreArguments();
			hmock.PreCall(null);
			LastCall.Return(MethodVoteOptions.Continue)
				.IgnoreArguments();
			mock.ReplayAll();

            TestContainer container = new TestContainer(@"Concrete\Castle\AOP\HelperClasses\ConfigSample1.config");
			container.Kernel.RemoveComponent("interceptor");
			container.Kernel.AddComponentInstance("interceptor", hmock);
			container.Resolve<ISomething>().AMethod("test");
			mock.VerifyAll(); 
		}
	}
}
