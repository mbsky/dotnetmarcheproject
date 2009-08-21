using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using DotNetMarche.Common.Test.Concrete.Castle.Classes;
using DotNetMarche.Infrastructure.Castle;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Concrete.Castle
{
	[TestFixture]
	public class LifeCycleTests
	{

		#region BaseTest

		[Test]
		public void TestStandard()
		{
			ITest tran, sing;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\config1.xml")))
			{
				tran = ioc.Resolve<ITest>("TransientITest");
				sing = ioc.Resolve<ITest>("SingletonITest");
			}
			Assert.IsTrue(tran.IsDisposed);
			Assert.IsTrue(sing.IsDisposed);
		}

		[Test]
		public void TestStandard2()
		{
			ITest tran, sing1, sing2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\config1.xml")))
			{
				tran = ioc.Resolve<ITest>("TransientITest");
				sing1 = ioc.Resolve<ITest>("SingletonITest");
				ioc.Release(tran);
				ioc.Release(sing1);
				Assert.IsTrue(tran.IsDisposed);
				Assert.IsFalse(sing1.IsDisposed);
				sing2 = ioc.Resolve<ITest>("SingletonITest");
				Assert.AreSame(sing1, sing2);
			}
		}

		/// <summary>
		/// This test is changed because of castle lifecycle management change
		/// </summary>
		[Test]
		public void TestContained3()
		{
			DisposableComponent tran;
			using (WindsorContainer ioc = new WindsorContainer(@"Concrete\Castle\Config\config1.xml"))
			{
				tran = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				Assert.IsFalse(tran.ITest.IsDisposed);
			}
			///This assert was previously IsFalse, because castle does not correctly dispose transient objects at the end of container lifecycle
			Assert.IsTrue(tran.ITest.IsDisposed);
		}

		#endregion


		/// <summary>
		/// Verify that into the context the object are really transient and that all the object
		/// gets disposed out of the context, even the inner resolved object.
		/// </summary>
		[Test]
		public void TestContained4()
		{
			DisposableComponent tran;
			DisposableComponent tran2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					Assert.That(tran, Is.Not.EqualTo(tran2));
				}
				Assert.IsTrue(tran.ITest.IsDisposed);
				Assert.IsTrue(tran2.ITest.IsDisposed);
				Assert.IsTrue(tran.IsDisposed);
				Assert.IsTrue(tran2.IsDisposed);
			}
		}

		/// <summary>
		/// Same of test 4 but with only an object
		/// </summary>
		[Test]
		public void TestContained4a()
		{
			DisposableComponent tran;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				}
				Assert.IsTrue(tran.ITest.IsDisposed);
				Assert.IsTrue(tran.IsDisposed);
			}
		}

		/// <summary>
		/// Create one object not in a context the object gets disposed when the container itself gets
		/// disposed.
		/// </summary>
		[Test]
		public void TestContained4b()
		{
			DisposableComponent tran;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				tran = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
			}
			Assert.IsTrue(tran.ITest.IsDisposed);
			Assert.IsTrue(tran.IsDisposed);
		}

		/// <summary>
		/// Create Two objects  out of a context the object gets disposed, similar to the 4 test, but with no
		/// context
		/// </summary>
		[Test]
		public void TestContained4c()
		{
			DisposableComponent tran1;
			DisposableComponent tran2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				tran1 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				Assert.That(tran1, Is.Not.EqualTo(tran2));
			}
			Assert.IsTrue(tran1.ITest.IsDisposed);
			Assert.IsTrue(tran2.ITest.IsDisposed);
			Assert.IsTrue(tran1.IsDisposed);
			Assert.IsTrue(tran2.IsDisposed);
		}

		/// <summary>
		/// Create three objects out of a context, the object gets disposed when the container gets disposed.
		/// </summary>
		[Test]
		public void TestContained4d()
		{
			DisposableComponent tran1;
			DisposableComponent tran2;
			DisposableComponent tran3;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				tran1 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				tran3 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				Assert.That(tran1, Is.Not.EqualTo(tran2));
				Assert.That(tran3, Is.Not.EqualTo(tran2));
				Assert.That(tran3, Is.Not.EqualTo(tran1));
			}
			Assert.IsTrue(tran1.ITest.IsDisposed);
			Assert.IsTrue(tran2.ITest.IsDisposed);
			Assert.IsTrue(tran3.ITest.IsDisposed);
			Assert.IsTrue(tran1.IsDisposed);
			Assert.IsTrue(tran2.IsDisposed);
			Assert.IsTrue(tran3.IsDisposed);
		}

		/// <summary>
		/// Create three objects in a context, the object gets disposed when the container gets disposed.
		/// </summary>
		[Test]
		public void TestContainedThreeObjectInAContext()
		{
			DisposableComponent tran1;
			DisposableComponent tran2;
			DisposableComponent tran3;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran1 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					tran3 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					Assert.That(tran1, Is.Not.EqualTo(tran2));
					Assert.That(tran3, Is.Not.EqualTo(tran2));
					Assert.That(tran3, Is.Not.EqualTo(tran1));
				}
				Assert.IsTrue(tran1.ITest.IsDisposed);
				Assert.IsTrue(tran2.ITest.IsDisposed);
				Assert.IsTrue(tran3.ITest.IsDisposed);
				Assert.IsTrue(tran1.IsDisposed);
				Assert.IsTrue(tran2.IsDisposed);
				Assert.IsTrue(tran3.IsDisposed);
			}
		}

		/// <summary>
		/// Create three objects, two in a context, the third out of the context, the two object created
		/// in a context gets disposed when the context end, the third object gets disposed when the
		/// container itself gets disposed.
		/// </summary>
		[Test]
		public void TestContainedTwoObjectInAContextAndOneOutside()
		{
			DisposableComponent tran1;
			DisposableComponent tran2;
			DisposableComponent tran3;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran1 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
					tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				}
				tran3 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				//Tran3 does not gets disposed because it is created out of the context.
				Assert.IsTrue(tran1.ITest.IsDisposed);
				Assert.IsTrue(tran2.ITest.IsDisposed);
				Assert.IsFalse(tran3.ITest.IsDisposed);
				Assert.IsTrue(tran1.IsDisposed);
				Assert.IsTrue(tran2.IsDisposed);
				Assert.IsFalse(tran3.IsDisposed);
			}
			Assert.IsTrue(tran3.ITest.IsDisposed);
			Assert.IsTrue(tran3.IsDisposed);
		}


		/// <summary>
		/// Create three objects out of a context, the three object gets released calling release in the
		/// container. The inner object resolved by dependencies gets no disposed until the whole container
		/// ends.
		/// </summary>
		[Test]
		public void TestContained4e()
		{
			DisposableComponent tran1;
			DisposableComponent tran2;
			DisposableComponent tran3;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				tran1 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				tran2 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				tran3 = ioc.Resolve<DisposableComponent>("TransientDisposableCon");
				ioc.Release(tran1);
				ioc.Release(tran2);
				ioc.Release(tran3);
				//Inner dependencies does not gets still disposed by release :( 
				//Edit: castle 5888 corrects this, now they are correctly disposed. all of the assert changes
				//[Old ASSERT]
				//Assert.IsFalse(tran1.ITest.IsDisposed);
				//Assert.IsFalse(tran2.ITest.IsDisposed);
				//Assert.IsFalse(tran3.ITest.IsDisposed);
				//[New ASSERT]
				Assert.IsTrue(tran1.ITest.IsDisposed);
				Assert.IsTrue(tran2.ITest.IsDisposed);
				Assert.IsTrue(tran3.ITest.IsDisposed);
				//Released object Yes
				Assert.IsTrue(tran1.IsDisposed);
				Assert.IsTrue(tran2.IsDisposed);
				Assert.IsTrue(tran3.IsDisposed);
			}
			//Container gets disposed and now all the inner object gets disposed.
			Assert.IsTrue(tran1.ITest.IsDisposed);
			Assert.IsTrue(tran2.ITest.IsDisposed);
			Assert.IsTrue(tran3.ITest.IsDisposed);
		}

		/// <summary>
		/// this is the standard behavior of the transient object, the inner object gets not disposed when release
		/// is called
		/// Edit: Changed in version 5888 now castle correctly dispose everything
		/// </summary>
		[Test]
		public void TestRelease()
		{
			DisposableComponent tran;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				tran = ioc.Resolve<DisposableComponent>("TransientDisposableCon");

				Assert.IsFalse(tran.IsDisposed);
				Assert.IsFalse(tran.ITest.IsDisposed);
				ioc.Release(tran);

				Assert.IsTrue(tran.IsDisposed);
				//Assert.IsFalse(tran.ITest.IsDisposed); //Old test, castle did not correctly dispsed chained objects
				Assert.IsTrue(tran.ITest.IsDisposed); //New test, now castle does correct works
			}
		}

		#region Test Mix custom and singleton



		#endregion

		#region TodoTest

		/// <summary>
		/// A disposable object with singleton depends on a ITest interface that is satisfied
		/// by an object with custom lifecycle. Check that the inner resolved object gets disposed
		/// when the container dispose itself.
		/// </summary>
		[Test]
		public void TestSingletonThatDependsFromCustom()
		{
			NotDisposableComponent tran;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran = ioc.Resolve<NotDisposableComponent>("NotDisposableSingleton");
				}
				//Now the singleton object tran ended is life, it does not implement dispose but the inner ITEst object should
				//gets disposed.
				Assert.IsTrue(tran.ITest.IsDisposed);
			}
		}

		/// <summary>
		/// The singleton context is really singleton in a context?
		/// </summary>
		[Test]
		public void BaseTestSingletonContext()
		{
			NotDisposableComponent tran1, tran2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran1 = ioc.Resolve<NotDisposableComponent>("NotDisposableSingleton");
					tran2 = ioc.Resolve<NotDisposableComponent>("NotDisposableSingleton");
					Assert.That(tran1, Is.EqualTo(tran2));
					Assert.That(tran1.ITest, Is.EqualTo(tran2.ITest));
				}
			}
		}

		/// <summary>
		/// Same as <see cref="BaseTestSingletonContext"/> but using singleton object that supports disposable
		/// </summary>
		[Test]
		public void BaseTestSingletonContextDisposable2()
		{
			DisposableComponent tran1, tran2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran1 = ioc.Resolve<DisposableComponent>("SingletonDisposableCon");
					tran2 = ioc.Resolve<DisposableComponent>("SingletonDisposableCon");
					Assert.That(tran1, Is.EqualTo(tran2));
					Assert.That(tran1.ITest, Is.EqualTo(tran2.ITest));
				}
			}
		}

		/// <summary>
		/// Same as <see cref="BaseTestSingletonContextDisposable2"/> but tests that all object gets disposed.
		/// </summary>
		[Test]
		public void BaseTestSingletonContextDisposable3()
		{
			DisposableComponent tran1, tran2;
			using (WindsorContainer ioc = new WindsorContainer(new XmlInterpreter(@"Concrete\Castle\Config\ConfigurationCustom.xml")))
			{
				using (ContextLifecycle.BeginContext())
				{
					tran1 = ioc.Resolve<DisposableComponent>("SingletonDisposableCon");
					tran2 = ioc.Resolve<DisposableComponent>("SingletonDisposableCon");
					Assert.That(tran1, Is.EqualTo(tran2));
					Assert.That(tran1.ITest, Is.EqualTo(tran2.ITest));
				}
				Assert.IsTrue(tran1.ITest.IsDisposed);
				Assert.IsTrue(tran2.ITest.IsDisposed);
				Assert.IsTrue(tran1.IsDisposed);
				Assert.IsTrue(tran2.IsDisposed);
			}
		}


		#endregion
	}
}
