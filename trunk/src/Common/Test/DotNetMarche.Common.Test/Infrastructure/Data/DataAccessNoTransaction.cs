using System;
using System.Configuration;
using System.IO;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Caching;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.TestHelpers.BaseTests;
using DotNetMarche.TestHelpers.BaseTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoIs = Rhino.Mocks.Constraints.Is;

namespace DotNetMarche.Common.Test.Infrastructure.Data
{
	[TestFixture, CustomConfiguration]
	public class DataAccessNoTransaction : BaseTestFixtureWithHelper
	{

		protected override void OnTestFixtureSetUp()
		{
			base.OnTestFixtureSetUp();
			InMemoryConfigurationRegistry registry = base.GetFromTestContext<InMemoryConfigurationRegistry>(
				CustomConfiguration.RegistryTestContextKey);
			registry.ConnStrings.Add("preload1", new ConnectionStringSettings("preload1", "data source=" +
				Path.GetFullPath(@"Infrastructure\Data\Preload\preload1.db"), "System.Data.SQLite"));
		}

		[Test]
		public void TestBasicUsageOfCache()
		{
			ICache cache = MockRepository.GenerateStub<ICache>();
			DisposeAtTheEndOfTest(GlobalCache.Override(cache));
			var Query = "select Id from Table1 where Name = {param}";
			cache.Expect(c => c.Get<String>(Query))
				.Return("select Id from Table1 where Name = :param");
			Int64 result = DataAccess.CreateQuery(Query).SetStringParam("param", "OtherTest").ExecuteScalar<Int64>();
			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void TestIfNotInCacheCacheTheQuery()
		{
			ICache cache = MockRepository.GenerateStub<ICache>();
			DisposeAtTheEndOfTest(GlobalCache.Override(cache));
			var Query = "select Id from Table1 where Name = {param}";
			var ExpandedQuery = "select Id from Table1 where Name = :param";
			cache.Expect(c => c.Insert(null, null, null, null, null))
				.Constraints(RhinoIs.Equal(Query), RhinoIs.Equal("DataAccess"), RhinoIs.Equal(ExpandedQuery),
								 RhinoIs.Anything(), RhinoIs.Anything())
				.Return(ExpandedQuery);
			cache.Expect(c => c.Get<String>(Query)).Return(null);
			Int64 result = DataAccess.CreateQuery(Query).SetStringParam("param", "OtherTest").ExecuteScalar<Int64>();
			Assert.That(result, Is.EqualTo(2));
		}

		
	}
}
