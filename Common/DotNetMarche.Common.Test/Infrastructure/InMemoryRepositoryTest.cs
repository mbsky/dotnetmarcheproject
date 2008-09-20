﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Helpers;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class InMemoryRepositoryTest
	{
		private InMemoryRepository<AnEntity> AnEntitySut;

		[SetUp]
		public void SetUp()
		{
			AnEntitySut = new InMemoryRepository<AnEntity>();
		}

		[Test]
		public void BasicEmptyRepoGet()
		{
			Assert.That(AnEntitySut.GetById(10), Is.Null);
		}

		/// <summary>
		/// When you save an entity, the repository should give to that entity a unique id.
		/// </summary>
		[Test]
		public void BasicAddEntity()
		{
			AnEntity GianMaria = AnEntity.Create(10, "Gian Maria", 200);
			AnEntitySut.Save(GianMaria);
			Assert.That(EntityIdFinder.GetIdValueFromEntity(GianMaria), Is.Not.EqualTo(10));
		}
	}
}
