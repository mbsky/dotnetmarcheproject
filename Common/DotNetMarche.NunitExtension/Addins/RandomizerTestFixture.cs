using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Core;

namespace DotNetMarche.NunitExtension.Addins
{
	class RandomizerTestFixture : TestFixture
	{
		public RandomizerTestFixture(Type fixtureType)
			: base(fixtureType)
		{
			this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod(fixtureType);
			this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod(fixtureType);
		}

		protected override void DoOneTimeSetUp(TestResult suiteResult)
		{
			base.DoOneTimeSetUp(suiteResult);
			suiteResult.AssertCount = NUnitFramework.GetAssertCount(); 
		}

		protected override void DoOneTimeTearDown(TestResult suiteResult)
		{
			base.DoOneTimeTearDown(suiteResult);
			suiteResult.AssertCount += NUnitFramework.GetAssertCount();
		}

		public override System.Collections.IList Tests
		{
			get
			{
				Random rnd = new Random();
				return base.Tests.Cast<Test>().OrderBy(n => rnd.Next()).ToList();
			}
		}
	}
}
