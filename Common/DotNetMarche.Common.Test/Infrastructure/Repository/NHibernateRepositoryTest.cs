using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Infrastructure.Entities;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Concrete.Repository;
using DotNetMarche.TestHelpers.Data;
using DotNetMarche.TestHelpers.Reflection;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure.Repository
{
	[TestFixture]
	public class NHibernateRepositoryTest
	{
		#region Test Management

		private IDisposable OverrideSettings;
		private const string ConfigFileName = "files\\NhConfigFile1.cfg.xml";
		private InMemoryConfigurationRegistry repo;
		private NHibernateRepository<AnEntity> sut;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			NHibernateSessionManager.GenerateDbFor(ConfigFileName);
			sut = new NHibernateRepository<AnEntity>();
			sut.ConfigurationFileName = ConfigFileName;
			repo = new InMemoryConfigurationRegistry();
			repo.ConnStrings.Add(
				"main", new ConnectionStringSettings(
					"main", "Data Source=DbFile1.db;Version=3", "System.Data.SQLite"));
			OverrideSettings = ConfigurationRegistry.Override(repo);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			OverrideSettings.Dispose();
		}

		#endregion

		[Test]
		public void TestSaveChangeIdOfTheEntity()
		{
			AnEntity ent = AnEntity.CreateSome();
			sut.Save(ent);
			Assert.That(Invoker.GetProp<Int32>(ent, "Id"), Is.Not.EqualTo(0));
		}

		[Test]
		public void TestSaveSaveAllData()
		{
			AnEntity ent = AnEntity.CreateSome();
			sut.Save(ent);
			DbAssert.OnQuery("select Id, Name, Value from AnEntity where Id ={Id}")
				.SetInt32Param("Id", Invoker.GetProp<Int32>(ent, "Id"))
				.That("Name", Is.EqualTo("Name"))
				.That("Value", Is.EqualTo(99))
				.ExecuteAssert();
		}
	}

}
