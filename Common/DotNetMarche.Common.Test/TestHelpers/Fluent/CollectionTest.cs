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

namespace DotNetMarche.Common.Test.TestHelpers.Fluent
{
	[TestFixture]
	public class CollectionTest {

		[Test]
		public void TestMyAll() {
			ArrayList al = new ArrayList();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("C", "B"));
			al.Add(new AnEntity("A", "B"));
			Assert.That(al, Has.All.Property("PropertyB", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B"));
		}

		[Test]
		public void TestAllWithLinq() {
			IList<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("C", "B"));
			al.Add(new AnEntity("A", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B"));
			Assert.That(al.Where(e => e.PropertyB == "B").Count(), Is.EqualTo(3));
		}

		[Test]
		public void TestMyAllWithAndLinq() {
			IList<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B")
			                	.And.All.Property("PropertyA", "A"));
			Assert.That(al.Where(e => e.PropertyB == "B" && e.PropertyA == "A").Count(), Is.EqualTo(3));
		}

		[Test]
		public void TestMyAllWithAndLinq2() {
			IList<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("U", "B"));
			al.Add(new AnEntity("A", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B")
			                	.And.Twice.Property("PropertyA", "A"));

			Assert.That(al.Count(e => e.PropertyB == "B"), Is.EqualTo(3));
			Assert.That(al.Count(e => e.PropertyA == "A"), Is.EqualTo(2));
		}

		[Test]
		public void TestMyAllWithAnd() {
			ArrayList al = new ArrayList();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			//Assert.That(al, Has.All.Property("PropertyB", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B")
			                	.And.All.Property("PropertyA", "A"));
		}

		/// <summary>
		/// This test shows that it is possible to chain constraint on 
		/// object count and base property. This test assert that all
		/// element has propertyB equal to B and the main object has property "count" 
		/// equal to 3
		/// </summary>
		[Test]
		public void TestMyAllWithAnd2() {
			ArrayList al = new ArrayList();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("A", "B"));
			//Assert.That(al, Has.All.Property("PropertyB", "B"));
			Assert.That(al, MyHas.All.Property("PropertyB", "B")
			                	.And.Property("Count", 3));
		}

		[Test]
		public void ACollectionTest() {
			ArrayList al = new ArrayList();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("C", "B"));
			al.Add(new AnEntity("A", "B"));
			Assert.That(al, Has.All.Property("PropertyB", "B"));
			
			Assert.That(al, Has.Some.Property("PropertyA", "C"));
		}

		[Test]
		public void AnotherCollectionTest() {
			List<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("C", "B"));
			al.Add(new AnEntity("A", "B"));

			Assert.That(al, Has.All.Property("PropertyB", "B"));
			Assert.That(al, Has.Some.Property("PropertyA", "C"));
			//MyHas.nunitOne.
			//Assert.That(al, new NunitOneItemConstraint(new PropertyConstraint("PropertyA", Is.EqualTo("C"))));
			Assert.That(al, MyHas.One.Property("PropertyA", "C"));
			Assert.That(al, MyHas.Twice.Property("PropertyA", "A"));
			Assert.That(al, MyHas.Count(3).Property("PropertyB", "B"));
		}

		[Test]
		public void TestChainWithAnd() {
			List<AnEntity> al = CreateTestListOne();
			Assert.That(al, MyHas.Twice.Property("PropertyA", "q").And.One.Property("PropertyB", "w"));
		}

		[Test]
		public void TestChainWithAndAndComposite() {
			List<AnEntity> al = CreateTestListOne();
			Assert.That(al, MyHas.Twice.Property("PropertyA", "q").Count(3).Property("PropertyB", "r"));
		}

		[Test]
		public void TestChainWithAndNone() {
			List<AnEntity> al = CreateTestListOne();
			Assert.That(al, MyHas.Twice.Property("PropertyA", "q").None.Property("PropertyB", "oo"));
		}
		[Test]
		public void TestChainWithAndNoneExplicit() {
			List<AnEntity> al = CreateTestListOne();
			Assert.That(al, MyHas.Twice.Property("PropertyA", "q")
			                	.And.None.Property("PropertyB", "oo"));
		}

		/// <summary>
		/// Verify that the property adaptor is used
		/// </summary>
		[Test]
		public void TestOperatorOnProperty() {
			List<SimpleTwoProps> al = CreateTestListTwo();
			Assert.That(al, MyHas.One.Property("PropB").GreaterThan(4));
		}

		[Test]
		public void TestChainWithOrNoneExplicit() {
			List<AnEntity> al = CreateTestListOne();
			Assert.That(al, 
			            MyHas.Twice.Property("PropertyA", "q")
			            	.Or.One.Property("PropertyB", "w"));
		}

		#region Constructors

		private static List<SimpleTwoProps> CreateTestListTwo() {
			List<SimpleTwoProps> al = new List<SimpleTwoProps>();
			al.Add(new SimpleTwoProps("A", 2));
			al.Add(new SimpleTwoProps("B", 3));
			al.Add(new SimpleTwoProps("C", 4));
			al.Add(new SimpleTwoProps("C", 5));
			return al;
		}

		private List<AnEntity> CreateTestListOne() {
			List<AnEntity> al = new List<AnEntity>();
			al.Add(new AnEntity("A", "B"));
			al.Add(new AnEntity("C", "r"));
			al.Add(new AnEntity("E", "r"));
			al.Add(new AnEntity("q", "r"));
			al.Add(new AnEntity("q", "w"));
			return al;
		}

		#endregion


	}
}