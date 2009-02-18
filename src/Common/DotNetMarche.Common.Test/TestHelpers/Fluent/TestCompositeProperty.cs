using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Nablasoft.Test.UnitTest;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent
{
	[TestFixture]
	public class TestCompositeProperty {

		/// <summary>
		/// I wanna assert that in the collection there is an element with PropA equal to "A" and PropB equal to "B"
		/// </summary>
		[Test]
		public void TestCompositeOne() {
			List<SimpleTwoProps> list = CreateTestList();
			Assert.That(list, MyHas.One['('].Property("PropA", "A").And.Property("PropB", 2)[')']);
		}

		private static List<SimpleTwoProps> CreateTestList() {
			List<SimpleTwoProps> al = new List<SimpleTwoProps>();
			al.Add(new SimpleTwoProps("A", 2));
			al.Add(new SimpleTwoProps("B", 3));
			al.Add(new SimpleTwoProps("C", 4));
			al.Add(new SimpleTwoProps("C", 2));
			return al;
		}
	}
}