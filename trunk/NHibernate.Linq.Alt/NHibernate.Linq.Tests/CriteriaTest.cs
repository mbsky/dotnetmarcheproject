using System;
using MbUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using NEX = NHibernate.Expressions;

namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class CriteriaTest : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        /// <summary>
        /// show how to use a detatched criteria to create a condition
        /// on count.
        /// This query select all Encontained objects that contains more than 1
        /// test object inside the Tests collection.
        /// </summary>
        [Test]
        public void WhereWithCountOnCollection_Criteria()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityContainer), "RootClass");

                NEX.DetachedCriteria d = NEX.DetachedCriteria.For(typeof(EntityTest))
                    .SetProjection(NEX.Projections.RowCount())
                    .Add(NEX.Property.ForName("Container").EqProperty("RootClass.Id"));

                //You can use also
                //.Add(NEX.Expression.EqProperty("Encontained", "RootClass.Id"));

                c.Add(NEX.Subqueries.Lt(1, d));
                IList<EntityContainer> result = c.List<EntityContainer>();
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0].PStr, "Alkampfer");
            }
        }

        [Test]
        public void WhereWithCountOnCollection_Criteria2()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityContainer), "RootClass");

                NEX.DetachedCriteria d = NEX.DetachedCriteria.For(typeof(EntityContainer))
                    .CreateAlias("Tests", "Tests")
                    .SetProjection(NEX.Projections.ProjectionList()
                        .Add(NEX.Projections.RowCount()))
                    .Add(NEX.Property.ForName("Id").EqProperty("RootClass.Id"));

                c.Add(NEX.Subqueries.Lt(1, d));
                IList<EntityContainer> result = c.List<EntityContainer>();
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0].PStr, "Alkampfer");
                ICollection<String> k = session.SessionFactory.Dialect.Functions.Keys;

            }
        }

        [Test]
        public void WhereWithDateTimeFunctions_Criteria()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(User));
                c.Add(new NHibernate.Linq.Visitors.CustomCriterion.SqlFunctionCriterion(
                     0, 1, "=", "month", "RegisteredAt"));

                ICollection<String> k = session.SessionFactory.Dialect.Functions.Keys;
                IList<User> res = c.List<User>();
                foreach (User u in res)
                    Assert.AreEqual(u.RegisteredAt.Month, 1);
            }
        }

        [Test]
        public void WhereWithDateTimeFunctions_Criteria2()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(User));
                c.Add(new NHibernate.Linq.Visitors.CustomCriterion.SqlFunctionCriterion(
                    0, 6, ">", "month", "RegisteredAt"));

                ICollection<String> k = session.SessionFactory.Dialect.Functions.Keys;
                IList<User> res = c.List<User>();
                foreach (User u in res)
                    Assert.Greater(u.RegisteredAt.Month, 6);
            }
        }

        [Test]
        public void WhereWithDateTimeFunctions_CriteriaTraversal()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityTest))
                    .CreateAlias("Container", "Container")
                    .Add(new NHibernate.Linq.Visitors.CustomCriterion.SqlFunctionCriterion(
                             0, 1, "=", "month", "Container.RegDate"));

                ICollection<String> k = session.SessionFactory.Dialect.Functions.Keys;
                IList<EntityTest> res = c.List<EntityTest>();
                foreach (EntityTest e in res)
                    Assert.AreEqual(e.Container.RegDate.Month, 1);
            }
        }

        /// <summary>
        /// USe a function of the db that accepts parameters.
        /// </summary>
        [Test]
        public void WhereWithSubString_Criteria()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityTest))
                    .Add(new NHibernate.Linq.Visitors.CustomCriterion.SqlFunctionCriterion(
                    0, "hr", "=", "substring", "PString", 1, 2));

                ICollection<String> k = session.SessionFactory.Dialect.Functions.Keys;
                IList<EntityTest> res = c.List<EntityTest>();
                foreach (EntityTest e in res)
                    Assert.AreEqual(e.PString, "Three");
            }
        }

        /// <summary>
        /// USe a function of the db that accepts parameters.
        /// </summary>
        [Test]
        public void WhereWithCompareAPropretyWithAnother_Criteria()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityTest))
                    .Add(NEX.Expression.EqProperty("PInt32", "PInt32"));

           
                IList<EntityTest> res = c.List<EntityTest>();
                Assert.AreEqual(res.Count, 4);
            }
        }

        [Test]
        public void Group_Criteria()
        {
            using (ISession session = CreateSession())
            {
                ICriteria c = session.CreateCriteria(typeof(EntityTest))
                    .SetProjection(
                        NEX.Projections.ProjectionList()
                        .Add(NEX.Projections.GroupProperty("PBool"))
                        .Add(NEX.Projections.Max("PInt32")));

                System.Collections.IList result = c.List();
                Assert.AreEqual(result.Count, 4);
            }
        }
    }
}
