extern alias OrigCastle;

using System;
using System.Collections.Generic;
using System.Text;
using Castle.Windsor;
using DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses;
using DotNetMarche.Infrastructure.Castle.AOP;
using Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

using CastleIInvocation = OrigCastle::Castle.Core.Interceptor.IInvocation;

namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1
{
    [TestFixture]
    public class TestBasic {

        /// <summary>
        /// Smoke test, everything ok? Objects can be resolved?
        /// </summary>
        [Test]  
        public void TestBasicInterception() {
            using (IWindsorContainer container = new WindsorContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample1.config")) {
                ISomething someth = container.Resolve<ISomething>();
                someth.AMethod("Test");      
                someth.OtherMethod("Test", "otherTEst");
            }
        }

        /// <summary>
        /// Check if the interceptor will be really called
        /// </summary>
        [Test]
        public void TestBasicInterceptionTest() {
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.CreateMock<IAspect>();
            hmock.PostCall(null, null);
            LastCall.IgnoreArguments();
            hmock.PreCall(null);
            LastCall.Return(MethodVoteOptions.Continue)
                .IgnoreArguments();
            mock.ReplayAll();

            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample1.config")) {
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                container.Resolve<ISomething>().OtherMethod("test1", "test2");
                //This is not intercepted, so the interceptor will be not called.
                container.Resolve<ISomething>().AMethod("test2");
            }

            mock.VerifyAll();
        }

        /// <summary>
        /// Check if the interceptor will be really called
        /// </summary>
        [Test]
        public void TestBasicInterceptionShouldNotProceed() {
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.CreateMock<IAspect>();
            hmock.PreCall(null);
            LastCall.Return(MethodVoteOptions.Halt)
                .IgnoreArguments();
            mock.ReplayAll();

            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample1.config")) {
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                container.Resolve<ISomething>().OtherMethod("test1", "test2");
            }

            mock.VerifyAll();
        }

        /// <summary>
        /// Check if the interceptor will be really called
        /// </summary>
        [Test]
        public void TestBasicInterceptionExceptionThrow() {
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.CreateMock<IAspect>();
            hmock.PreCall(null);
            LastCall.Return(MethodVoteOptions.Continue)
                .IgnoreArguments();

            hmock.OnException(null, null, null);
            LastCall.Constraints(
                new Rhino.Mocks.Constraints.Anything(),
                Rhino.Mocks.Constraints.Is.TypeOf(typeof(NullReferenceException)), 
                Rhino.Mocks.Constraints.Is.Anything());

            mock.ReplayAll();
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample1.config")) {
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                try {
                    container.Resolve<ISomething>().OtherMethod(null, null);
                    Assert.Fail("If reach here exception was not thrown");
                }
                catch (Exception) {
                }

            }

            mock.VerifyAll();
        }

        /// <summary>
        /// If an interface should not match any pointcut the object should not be proxied
        /// </summary>
        [Test]
        public void TestProxyObjectOnlyIfNeeded() {
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample1.config")) {
                //Some class has a method that match the pointcut, it should be proxyed
                object obj = container.Resolve<ISomething>();
                Assert.IsNotInstanceOfType(typeof(SomeClass), obj);

                obj = container.Resolve<IAnotherSomething>();
                Assert.IsInstanceOfType(typeof(OtherClass), obj, "OtherClass must not be proxied");
            }
        }

        /// <summary>
        /// TEst if a class is intercepted even if it is declared before pointcut.
        /// </summary>
        [Test]
        public void TestProxyObjectIfDeclaredBeforePointcut() {
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample2.config"))
            {
                //Some class has a method that match the pointcut, it should be proxyed
                object obj = container.Resolve<ISomething>();
                Assert.IsNotInstanceOfType(typeof(SomeClass), obj);

                obj = container.Resolve<IAnotherSomething>();
                Assert.IsNotInstanceOfType(typeof(OtherClass), obj, "OtherClass must not be proxied");
            }
        }

        /// <summary>
        /// Can I declare two pointcut?
        /// </summary>
        public void TestSmokeMultiplePointCut()
        {
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample3.config")) {}
        }
         
        /// <summary>
        /// I have two pointcut, both of them must work.
        /// </summary>
        [Test] 
        public void TestMultiplePointCut()
        {
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.CreateMock<IAspect>();
            Expect.Call(hmock.PreCall(null))
                .Repeat.Twice()
                .Return(MethodVoteOptions.Continue)
                .IgnoreArguments();
            Expect.Call(() => hmock.PostCall(null, null))
                .Repeat.Twice()
                .IgnoreArguments();

            mock.ReplayAll();
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample3.config"))
            {
                //Set the mock into the kernel.
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                ISomething something = container.Resolve<ISomething>();
                something.AMethod("TEST");
                something.OtherMethod("1", "2");
            }

            mock.VerifyAll();
        }

        /// <summary>
        /// Verify that I'm able to override the real return value of the method.
        /// </summary>
        [Test]
        public void TestOverrideReturnValue()
        {
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.StrictMock<IAspect>();
            Expect.Call(hmock.PreCall(null))
                .Return(MethodVoteOptions.Continue)
                .IgnoreArguments();
            hmock.PostCall(null, null);
            LastCall.Callback(delegate(CastleIInvocation ii, AspectReturnValue retval)
                                  {
                                      retval.WrappedReturnValue = "1234567890";
                                      return retval.Original.Equals("12");
                                  });
             
            mock.ReplayAll();
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample2.config"))
            {
                //Set the mock into the kernel.
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                ISomething something = container.Resolve<ISomething>();
                String retvalue = something.OtherMethod("1", "2");
                Assert.That(retvalue, Is.EqualTo("1234567890"));
            }

            mock.VerifyAll();
        }

        /// <summary>
        /// Verify that I'm able to stop propagation of exception and return a default value
        /// </summary>
        [Test]
        public void TestIgnoreExeptionReturnValue()
        { 
            //Create and mock the interceptor.
            MockRepository mock = new MockRepository();
            IAspect hmock = mock.StrictMock<IAspect>();
            Expect.Call(hmock.PreCall(null))
                .Return(MethodVoteOptions.Continue)
                .IgnoreArguments();
            Expect.Call(() => hmock.PostCall(null, null))
                .IgnoreArguments();
            Expect.Call(() => hmock.OnException(null, null, null))
                .Callback(delegate(CastleIInvocation ii, Exception e, AspectExceptionAction action)
                              {
                                  action.IgnoreException = true;
                                  return true;
                              });
            mock.ReplayAll();
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\ConfigSample2.config"))
            {
                //Set the mock into the kernel.
                container.Kernel.RemoveComponent("interceptor");
                container.Kernel.AddComponentInstance("interceptor", hmock);
                ISomething something = container.Resolve<ISomething>("throwclass");
                String retvalue = something.OtherMethod("1", "2");
                Assert.That(retvalue, Is.Null); //Default value is returned
            }

            mock.VerifyAll();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestWrongAopConfiguration() {
            using (TestContainer container = new TestContainer(@"Concrete\Castle\AOP\TestSuite1\Configs\WrongConfig.config")) { }
        }
    }
}