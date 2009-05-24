using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.NunitExtension.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class RandomizeTestOrderFixtureAttribute : Attribute
	{
	}
}