using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Caching;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.TestHelpers.BaseTests;
using DotNetMarche.TestHelpers.BaseTests.Helpers;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoIs = Rhino.Mocks.Constraints.Is;

namespace DotNetMarche.Common.Test.Infrastructure.Data
{
	[TestFixture, TestWithRhinoMock, TestWithInMemoryConfiguration]
	public class DataAccessNoTransaction : BaseUtilityTestWithHelper
	{

		protected override void OnTestFixtureSetUp()
		{
			base.OnTestFixtureSetUp();
			Configuration.ConnStrings.Add("preload1", new ConnectionStringSettings("preload1", "data source=" +
				Path.GetFullPath(@"Infrastructure\Data\Preload\preload1.db"), "System.Data.SQLite"));
		}

		[Test]
		public void TestBasicUsageOfCache()
		{
			ICache cache = MockRepository.CreateMock<ICache>();
			DisposeAtTheEndOfTest(GlobalCache.Override(cache));
			var Query = "select Id from Table1 where Name = {param}";
			Expect.Call(cache.Get<String>(Query)).Return("select Id from Table1 where Name = :param");
			MockRepository.ReplayAll();
			Int64 result = DataAccess.CreateQuery(Query).SetStringParam("param", "OtherTest").ExecuteScalar<Int64>();
			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void TestIfNotInCacheCacheTheQuery()
		{
			ICache cache = MockRepository.CreateMock<ICache>();
			DisposeAtTheEndOfTest(GlobalCache.Override(cache));
			var Query = "select Id from Table1 where Name = {param}";
			var ExpandedQuery = "select Id from Table1 where Name = :param";
			Expect.Call(cache.Insert(null, null, null, null, null))
				.Constraints(RhinoIs.Equal(Query), RhinoIs.Equal("DataAccess"), RhinoIs.Equal(ExpandedQuery),
								 RhinoIs.Anything(), RhinoIs.Anything())
				.Return(ExpandedQuery);
			Expect.Call(cache.Get<String>(Query)).Return(null);
			MockRepository.ReplayAll();
			Int64 result = DataAccess.CreateQuery(Query).SetStringParam("param", "OtherTest").ExecuteScalar<Int64>();
			Assert.That(result, Is.EqualTo(2));
		}

		
	}
}
