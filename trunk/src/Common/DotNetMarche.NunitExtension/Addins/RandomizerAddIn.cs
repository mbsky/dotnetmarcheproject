using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Core.Extensibility;

namespace DotNetMarche.NunitExtension.Addins
{
	[NUnitAddin(Name ="Randomizer")]
	public class RandomizerAddIn : NUnitTestFixtureBuilder, IAddin 
	{
		public const string RandomizerTestAttribute = "DotNetMarche.NunitExtension.Attributes.RandomizeTestOrderFixtureAttribute";

		#region IAddin Members

		public bool Install(IExtensionHost host)
		{
			IExtensionPoint testCaseBuilders = host.GetExtensionPoint("SuiteBuilders");
			if (testCaseBuilders == null)
			{
				return false;
			}

			testCaseBuilders.Install(this);
			return true;
		}

		#endregion

		public override bool CanBuildFrom(Type type)
		{
			return Reflect.HasAttribute(type, RandomizerTestAttribute, true);
		}

		protected override TestSuite MakeSuite(Type type)
		{
			return new RandomizerTestFixture(type);
		}
	}
}
