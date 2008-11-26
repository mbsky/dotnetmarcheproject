using System;
using System.Data;
using MbUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Linq.Tests;
using NHibernate.Linq.Tests.Entities;
using NHibernate.Tool.hbm2ddl;

[assembly : AssemblyCleanup(typeof (GlobalSetup))]

namespace NHibernate.Linq.Tests
{
	public static class GlobalSetup
	{
		private static ISessionFactory factory;

		[SetUp]
		public static void SetupNHibernate()
		{
			Configuration cfg = new Configuration().Configure();
            factory = cfg.BuildSessionFactory();
#if SkipInit
            return;
#endif
			new SchemaExport(cfg).Execute(false, true, false, true);
			

			CreateTestData();
            CreateTestDataForEntest();
		}

		private static void CreateTestData()
		{
			var users = new[]
			            	{
			            		new User("ayende", DateTime.Today),
			            		new User("rahien", new DateTime(1998, 12, 31)),
			            		new User("nhibernate", new DateTime(2000, 1, 1)) { LastLoginDate = DateTime.Now.AddDays(-1) },
			            	};
			using (ISession session = CreateSession())
			{
				session.Delete("from User");
				session.Flush();
				foreach (User user in users)
				{
					session.Save(user);
				}
				session.Flush();
			}
		}

        private static void CreateTestDataForEntest() {
            EntityTest[] ents = new EntityTest[] {
                new EntityTest() {PByte = 1, PInt16=1, PInt32 = 1, PInt64 = 1, PString= "One", PBool = true}, 
                new EntityTest() {PByte = 2, PInt16=2, PInt32 = 2, PInt64 = 2, PString= "Two", PBool = true}, 
                new EntityTest() {PByte = 3, PInt16=3, PInt32 = 3, PInt64 = 3, PString= "Three", PBool = true}, 
                new EntityTest() {PByte = 4, PInt16=4, PInt32 = 4, PInt64 = 4, PString= "Four", PBool = false}
            };
            EntityContainer[] encns = new EntityContainer[] {
                new EntityContainer() {PStr = "Alkampfer", RegDate = new DateTime(2008,1,2)},
                new EntityContainer() {PStr = "Gian Maria Ricci", RegDate = new DateTime(2008,7,20)}
            };
            ents[0].Container = encns[0];
            ents[1].Container = encns[0];
            ents[2].Container = encns[0];
            ents[3].Container = encns[1];
            using (ISession session = CreateSession())
			{
				session.Delete("from EntityTest");
                session.Delete("from EntityContainer");
				session.Flush();
                foreach (EntityContainer entc in encns)
                {
                    session.Save(entc);
                }
                foreach (EntityTest ent in ents)
				{
                    session.Save(ent);
				}
				session.Flush();
			}
        }

		public static ISession CreateSession()
		{
			return factory.OpenSession();
		}

		public static ISession CreateSession(IDbConnection con)
		{
			return factory.OpenSession(con);
		}
	}
}
