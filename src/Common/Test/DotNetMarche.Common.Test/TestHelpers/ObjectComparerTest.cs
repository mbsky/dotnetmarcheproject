using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.TestHelpers.Comparison;
using DotNetMarche.TestHelpers.Constraints;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using NUnit.Framework;

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
			Assert.That(entity1, Is.Not.ObjectEqual(entity2));
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
			Assert.That(entity1, Is.ObjectEqual(entity2));
		}  
	}
}
