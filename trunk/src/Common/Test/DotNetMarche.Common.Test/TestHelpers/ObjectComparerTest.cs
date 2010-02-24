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
			var res = ObjectComparer.FindDifferencies(entity1, entity2);
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
		public void CompositeProperty()
		{
			AnotherEntity e1 = new AnotherEntity() {Name = "2" , Entity = new AnEntity() {Name = "Test1"}};
			AnotherEntity e2 = new AnotherEntity() {Name = "2", Entity = new AnEntity() { Name = "Test2" } };
			var res = ObjectComparer.FindDifferencies(e1, e2);
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
	}
}
