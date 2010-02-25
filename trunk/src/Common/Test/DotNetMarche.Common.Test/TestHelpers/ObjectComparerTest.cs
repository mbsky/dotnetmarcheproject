using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.TestHelpers.Comparison;
using DotNetMarche.TestHelpers.Constraints;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.Common.Test.TestHelpers
{
	[TestFixture]
	public class ObjectComparerTest
	{
		[Test]
		public void TestBasicCompare()
		{
			AnEntity entity1 = AnEntity.Create(10, "test", 100);
			AnEntity entity2 = AnEntity.Create(10, "test", 100);
			Assert.That(ObjectComparer.AreEqual(entity1, entity2));
		}

		[Test]
		public void TestBasicCompareFalse()
		{
			AnEntity entity1 = AnEntity.Create(10, "test", 100);
			AnEntity entity2 = AnEntity.Create(10, "test", 110);
			Assert.That(ObjectComparer.AreEqual(entity1, entity2), Is.False);
		}


		[Test]
		public void TestBasicCompareFalseSyntaxHelpers()
		{
			AnEntity entity1 = AnEntity.Create(10, "test", 100);
			AnEntity entity2 = AnEntity.Create(10, "test", 110);
            Assert.That(entity1, DotNetMarche.TestHelpers.SyntaxHelpers.Is.Not.ObjectEqual(entity2));
		}

		[Test]
		public void TestBasicCompareDoesNotCompareBackingField()
		{
			AnEntity entity1 = AnEntity.Create(10, "test1", 110);
			AnEntity entity2 = AnEntity.Create(10, "test2", 110);
			ObjectComparer comparer = new ObjectComparer();
			var res = comparer.FindDifferencies(entity1, entity2);
			Assert.That(res.Count, Is.EqualTo(1));
		}

		[Test]
		public void TestBasicCompareWithField()
		{
			AnEntity entity1 = AnEntity.CreateSome();
			AnEntity entity2 = AnEntity.Create(entity1);
			entity2.fieldValue = entity2.fieldValue + 1;
			Assert.That(entity1, DotNetMarche.TestHelpers.SyntaxHelpers.Is.Not.ObjectEqual(entity2));
		}

		[Test]
		public void VerifyDoubleNull()
		{
			AnEntity entity1 = AnEntity.Create(10, null, 100);
			AnEntity entity2 = AnEntity.Create(10, null, 100);
			Assert.That(ObjectComparer.AreEqual(entity1, entity2));
		}

		[Test]
		public void VerifyIgnoreList()
		{
			AnEntity entity1 = AnEntity.Create(10, null, 100);
			AnEntity entity2 = AnEntity.Create(10, null, 100);
			entity2.Value = 0;
			ObjectComparer comparer = new ObjectComparer();
			comparer.AddIgnore("Value");
			Assert.That(comparer.Compare(entity1, entity2));
		}

		[Test]
		public void CompositeProperty()
		{
			AnotherEntity e1 = new AnotherEntity() {Name = "2" , Entity = new AnEntity() {Name = "Test1"}};
			AnotherEntity e2 = new AnotherEntity() {Name = "2", Entity = new AnEntity() { Name = "Test2" } };
			ObjectComparer comparer = new ObjectComparer();
			var res = comparer.FindDifferencies(e1, e2);
			Assert.That(res[0], Text.Contains("Entity.Name"));
		}


		/// <summary>
		/// this is not a real test, is used only to see the log that 
		/// a failing assertion does.
		/// </summary>
		public void TestBasicCompareFalseVerifyLog()
		{
			AnEntity entity1 = AnEntity.Create(10, "test", 100);
			AnEntity entity2 = AnEntity.Create(10, "test", 110);
			ObjectEqualConstraint c = new ObjectEqualConstraint(entity2);
			c.Matches(entity1);
			MessageWriter mw = new TextMessageWriter();
			c.WriteDescriptionTo(mw);
            Assert.That(entity1, DotNetMarche.TestHelpers.SyntaxHelpers.Is.ObjectEqual(entity2));
		}

		[Test]
		public void CollectionCompare()
		{
			AnEntityWithCollection e1 = new AnEntityWithCollection() {Id = 1, Collection = {"a", "b"}};
			AnEntityWithCollection e2 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b" } };
			Assert.That(ObjectComparer.AreEqual(e1, e2));
		}

		[Test]
		public void CollectionCompareDifferentNumberOfElement()
		{
			AnEntityWithCollection e1 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b" } };
			AnEntityWithCollection e2 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b", "c" } };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void CollectionCompareDifferentNumberOfElement2()
		{
			AnEntityWithCollection e1 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b", "c" } };
			AnEntityWithCollection e2 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b"} };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void CollectionCompareFalseComparison()
		{
			AnEntityWithCollection e1 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b" } };
			AnEntityWithCollection e2 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "c" } };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void VerifyMessageOfCollectionCompareFalseComparison()
		{
			AnEntityWithCollection e1 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "b" } };
			AnEntityWithCollection e2 = new AnEntityWithCollection() { Id = 1, Collection = { "a", "c" } };
			ObjectComparer comparer = new ObjectComparer();
			var res = comparer.FindDifferencies(e1, e2);
			Assert.That(res[0], Text.Contains("b!=c"));
			Assert.That(res[0], Text.Contains("root.Collection"));
		}

		[Test]
		public void DictionaryCompare()
		{
			AnEntityWithDictionary e1 = new AnEntityWithDictionary() { Id = 1, Dictionary = { {"a", 1}, {"b", 2} }};
			AnEntityWithDictionary e2 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 }, { "b", 2 } } };
			Assert.That(ObjectComparer.AreEqual(e1, e2));
		}

		[Test]
		public void DictionaryCompareFalseComparison()
		{
			AnEntityWithDictionary e1 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 }, { "b", 2 } } };
			AnEntityWithDictionary e2 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 }, { "b", 3 } } };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void DictionaryCompareFalseComparisonDifferentElement()
		{
			AnEntityWithDictionary e1 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 }, { "b", 2 } } };
			AnEntityWithDictionary e2 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 } } };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void DictionaryCompareFalseComparisonDifferentElement2()
		{
			AnEntityWithDictionary e1 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 } } };
			AnEntityWithDictionary e2 = new AnEntityWithDictionary() { Id = 1, Dictionary = { { "a", 1 }, { "b", 2 } } };
			Assert.That(ObjectComparer.AreEqual(e1, e2), Is.False);
		}

		[Test]
		public void CollectionComplexCompare()
		{
			AnEntityWithComplexCollection e1 = new AnEntityWithComplexCollection() 
			{ Id = 1, Collection = {new AnEntity() {Name = "Test" }} };
			AnEntityWithComplexCollection e2 = new AnEntityWithComplexCollection() 
			{ Id = 1, Collection = {new AnEntity() {Name = "Test" }} };
			ObjectComparer comparer = new ObjectComparer();
			var res = comparer.FindDifferencies(e1, e2);
			Assert.That(ObjectComparer.AreEqual(e1, e2));
		}
	}
}
