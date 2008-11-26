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
	public class NHibernateRepositoryTest : DotNetMarche.TestHelpers.BaseTests.BaseUtilityTest
	{
		#region Test Management

		private IDisposable OverrideSettings;
		private const string ConfigFileName = "files\\NhConfigFile1.cfg.xml";
		private InMemoryConfigurationRegistry repo;
		private NHibernateRepository<AnEntity> sut;

		/// <summary>
		/// In the test fixture setup I simply ovveride the configuration registry
		/// using an in memory configuration with a fixed connection string.
		/// </summary>
		protected override void OnTestFixtureSetUp()
		{
			sut = new NHibernateRepository<AnEntity>();
			sut.ConfigurationFileName = ConfigFileName;
			repo = new InMemoryConfigurationRegistry();
			repo.ConnStrings.Add(
				"main", new ConnectionStringSettings(
					"main", "Data Source=DbFile1.db;Version=3", "System.Data.SQLite"));
			DisposeAtTheEndOfFixture(ConfigurationRegistry.Override(repo));
			NHibernateSessionManager.GenerateDbFor(ConfigFileName);
			
			base.OnTestFixtureSetUp();
		}

		#endregion

		[Test]
		public void TestSaveChangeIdOfTheEntity()
		{
			AnEntity ent = AnEntity.CreateSome();
			sut.Save(ent);
			Assert.That(Invoker.GetProp<Int32>(ent, "Id"), Is.Not.EqualTo(0));
		}

		/// <summary>
		/// Test saving data into database
		/// </summary>
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

		[Test]
		public void TestBasicQueryableForSession()
		{
			var result = from AnEntity en in sut.Query()
			             where en.Name == "Alkampfer"
			             select en;
			Assert.That(result.Count(), Is.EqualTo(0));
		}
	}

}
