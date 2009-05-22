using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.Common.Test.Infrastructure.Entities.ForEntityIdFinder;
using DotNetMarche.Infrastructure.Helpers;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class GenericHelpers
	{

		#region EntityIdFinder

	/// <summary>
		/// verify that the idFinder works well with a priva id
		/// </summary>
		[Test]
		public void TestIdFinder()
		{
			AnEntity entity = AnEntity.CreateWithIdOnly(10);
			Object id = EntityIdFinder.GetIdValueFromEntity(entity);
			Assert.That(id, Is.TypeOf(typeof (Int32)));
			Assert.That(id, Is.EqualTo(10));
		}

		/// <summary>
		/// The property does not have a property called id
		/// </summary>
		[Test, ExpectedException(typeof(ArgumentException))]
		public void TestIdFinderForNoIdEntity()
		{
			IdNotNamedId entity = IdNotNamedId.Create();
			Object id = EntityIdFinder.GetIdValueFromEntity(entity);
		}

		[Test]
		public void TestIdFinderWithField()
		{
			IdWithField entity = IdWithField.Create(10);
			Object id = EntityIdFinder.GetIdValueFromEntity(entity);
			Assert.That(id, Is.TypeOf(typeof(Int32)));
			Assert.That(id, Is.EqualTo(10));
		}

		
		[Test]
		public void TestIdFinderWithFieldPublic()
		{
			PublicIdEntity entity = PublicIdEntity.Create(10);
			Object id = EntityIdFinder.GetIdValueFromEntity(entity);
			Assert.That(id, Is.TypeOf(typeof(Int32)));
			Assert.That(id, Is.EqualTo(10));
		}

		/// <summary>
		/// verify that the idFinder can set private id for entities.
		/// </summary>
		[Test]
		public void TestIdFinderSet()
		{
			AnEntity entity = AnEntity.CreateWithIdOnly(10);
			EntityIdFinder.SetIdValueForEntity(entity, 99);
			Object id = EntityIdFinder.GetIdValueFromEntity(entity);
			Assert.That(id, Is.TypeOf(typeof(Int32)));
			Assert.That(id, Is.EqualTo(99));
		}
		
		#endregion
	}
}
