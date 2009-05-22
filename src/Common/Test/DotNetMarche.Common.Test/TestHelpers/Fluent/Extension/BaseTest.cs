using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest;
using Nablasoft.Test.UnitTest;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;
using System.Linq;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.Extension
{
	[TestFixture]
	public class BaseTest {

		internal List<AnEntity> CreateList1() {
			List<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("B", "B"));
			al.Add(new AnEntity("C", "B"));
			return al;
		}

		[Test]
		public void TestFirstCountA() {
			List<AnEntity> al = CreateList1();
			Assert.That(al, MyHas
			                	.All.Property("PropertyB", "B")
			                	.And.One.Property("PropertyA", "C"));
		}

		[Test]
		public void TestFirstCount() {
			List<AnEntity> al = CreateList1();
			al.Has().Count(3, e => e.PropertyB == "B").Assert();
		}

		[Test]
		public void TestFirstAll() {
			List<AnEntity> al = CreateList1();
			al.Has().All(e => e.PropertyB == "B").Assert();
		}

		[Test]
		public void TestCombineWithAnd() {
			List<AnEntity> al = CreateList1();
			al.Has().All(e => e.PropertyB == "B")
				.And.One(e => e.PropertyA == "A").Assert();
		}
	}
}