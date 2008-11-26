using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MbUnit.Framework;
using NHibernate.Linq.Tests.Entities;

namespace NHibernate.Linq.Tests
{
	public class BaseTest
	{
		protected NorthwindContext db;
		protected TestContext nhib;
		protected NorthwindContext nwnd;
		protected ISession session;

		protected virtual string ConnectionStringName
		{
			get { return "Northwind"; }
		}

		[SetUp]
		public void Setup()
		{
			session = CreateSession();
			nwnd = db = new NorthwindContext(session);
			nhib = new TestContext(session);
		}

		protected virtual ISession CreateSession()
		{
			IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
			con.Open();
			return GlobalSetup.CreateSession(con);
		}

		[TearDown]
		public void TearDown()
		{
			session.Connection.Dispose();
			session.Dispose();
			session = null;
		}
	}
}