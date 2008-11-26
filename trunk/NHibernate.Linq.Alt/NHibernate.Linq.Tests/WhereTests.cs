using System;
using MbUnit.Framework;
using NHibernate.Linq.SqlClient;
using NHibernate.Linq.Tests.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using NEX = NHibernate.Criterion;

namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class WhereTests : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        #region Old and various tests


        [Test]
        public void NoWhereClause()
        {
            var query = (
                            from user in session.Linq<User>()
                            select user).ToList();
            Assert.AreEqual(3, query.Count);
        }



        [Test]
        public void WhereWithConstantExpression()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "ayende"
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void WhereWithConstantExpressionNotEq()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name != "ayende"
                         select user).ToList();
            Assert.AreEqual(2, query.Count);
        }


        [Test]
        public void WhereWithConstantExpression2()
        {
            var query = (from user in session.Linq<User>()
                         where "ayende" == user.Name
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void WhereWithStartsWith()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name.StartsWith("a")
                         select user).FirstOrDefault();
            Assert.AreEqual(query.Name, "ayende");
        }


        [Test]
        public void WhereWithEndsWith()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name.EndsWith("e")
                         select user).ToList();
            Assert.AreEqual(2, query.Count);

        }

        [Test]
        public void WhereWithContains()
        {
            IEnumerable<User> userlist = (from user in session.Linq<User>()
                           where user.Name.Contains("ye")
                           select user);
            User ayende = userlist.Single<User>();
            Assert.AreEqual(ayende.Name, "ayende");

        }

        [Test]
        public void WhereWithConditionOnBoolean()
        {
            IEnumerable<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                        where ent.PBool
                                        select ent);
            Assert.AreEqual(result.Count<EntityTest>(), 3);
        }

        [Test]
        public void WhereWithConditionOnBooleanExplicit()
        {
            IEnumerable<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                              where ent.PBool == true
                                              select ent);
            Assert.AreEqual(result.Count<EntityTest>(), 3);
        }

        [Test]
        public void WhereWithConditionOnBooleanNot()
        {
            IEnumerable<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                              where !ent.PBool
                                              select ent);
            Assert.AreEqual(result.Single<EntityTest>().PString, "Four");
        }

        #endregion


        #region Walk the properties

        [Test]
        public void WhereWithWalkTheGraph()
        {
            IList<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                        where ent.Container.PStr == "Alkampfer"
                                        select ent).ToList<EntityTest>();
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void WhereWithWalkTheGraphAndStartsWith()
        {
            IList<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                        where ent.Container.PStr.StartsWith("A")
                                        select ent).ToList<EntityTest>();
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void WhereWithPropertyEqualsToAnother()
        {
            IEnumerable<EntityTest> result = (from ent in session.Linq<EntityTest>()
                                              where ent.PInt32 == ent.PInt32
                                              select ent);
            Assert.AreEqual(result.Count<EntityTest>(), 4);
        }

        #endregion

        #region Different type of integer

        [Test]
        public void WhereWithInt16()
        {
            IEnumerable<EntityTest> resultlist = (from o in session.Linq<EntityTest>()
                                 where o.PInt16 == 1
                                 select o);
            EntityTest result = resultlist.Single<EntityTest>();
            Assert.AreEqual(result.PInt16, 1);
        }

        [Test]
        public void WhereWithInt64()
        {
            IEnumerable<EntityTest> resultlist = (from o in session.Linq<EntityTest>()
                                 where o.PInt64 == 1
                                 select o);
            EntityTest result = resultlist.Single<EntityTest>();
            Assert.AreEqual(result.PInt64, 1);
        }

        #endregion

        #region DbFunction

        [Test]
        public void WhereDbFunctSubstring()
        {
            IEnumerable<EntityTest> Three = (
                            from en in session.Linq<EntityTest>()
                            where session.DbMethods().Substring(en.PString, 2, 3) == "hre"
                            select en);
            //Using Single we assure that we have only a result
            Assert.AreEqual(Three.Single<EntityTest>().PString, "Three");

        }

        [Test]
        public void WhereDbFunctCharIndex()
        {
            IEnumerable<EntityTest> Three = (
                            from en in session.Linq<EntityTest>()
                            where session.DbMethods().CharIndex('h', en.PString) == 2
                            select en);
            //Using Single we assure that we have only a result
            Assert.AreEqual(Three.Single<EntityTest>().PString, "Three");

        }


        [Test]
        public void WhereDbFunctMonth()
        {
            IEnumerable<EntityContainer> Gm = (
                            from en in session.Linq<EntityContainer>()
                            where session.DbMethods().Month(en.RegDate) == 7
                            select en);
            //Using Single we assure that we have only a result
            Assert.AreEqual(Gm.Single<EntityContainer>().PStr, "Gian Maria Ricci");

        }


        #endregion

        #region Some projection

        [Test]
        public void CountResultWithProjection()
        {
            Int32 count = (from user in session.Linq<User>()
                           select user).Count();
            Assert.AreEqual(count, 3);
        }

        [Test]
        public void CountResultWithProjectionAnCondition()
        {
            Int32 count = (from user in session.Linq<User>()
                           where user.Name == "ayende"
                           select user).Count();
            Assert.AreEqual(count, 1);
        }

        #endregion


        [Test]
        public void WhereWithCountOnCollection()
        {
            EntityContainer result = (from entc in session.Linq<EntityContainer>()
                                      where entc.Tests.Count > 1
                                      select entc).Single<EntityContainer>();
            Assert.AreEqual(result.PStr, "Alkampfer");
        }



        [Test]
        public void UsersByRegistrationDate()
        {
            DateTime d = DateTime.Today;
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt == d
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void FirstElementWithWhere()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "ayende"
                         select user).First();
            Assert.AreEqual("ayende", query.Name);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FirstElementWithQueryThatReturnsNoResults()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "xxx"
                         select user).First();
        }

        [Test]
        public void FirstOrDefaultElementWithQueryThatReturnsNoResults()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "xxx"
                         select user).FirstOrDefault();

            Assert.IsNull(query);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleElementWithQueryThatReturnsNoResults()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "xxx"
                         select user).Single();
        }

        /// <summary>
        /// This test should trhow an exception because we retrieve
        /// more than one elment and the single forces the result
        /// to have only one element.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleElementWithQueryThatReturnsMoreThanOneResult()
        {
            var query = (from user in session.Linq<User>()
                         select user).Single();

        }

        [Test]
        public void SingleOrDefaultElementWithQueryThatReturnsNoResults()
        {
            var query = (from user in session.Linq<User>()
                         where user.Name == "xxx"
                         select user).SingleOrDefault();

            Assert.IsNull(query);
        }

        [Test]
        public void UsersRegisteredAtOrAfterY2K()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt >= new DateTime(2000, 1, 1)
                         select user).ToList();
            Assert.AreEqual(2, query.Count);
        }


        [Test]
        public void UsersRegisteredAtOrAfterY2K_And_Before2001()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt >= new DateTime(2000, 1, 1) && user.RegisteredAt <= new DateTime(2001, 1, 1)
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }



        [Test]
        public void UsersByNameAndRegistrationDateNow()
        {

            var query = (from user in session.Linq<User>()
                         where user.Name == "ayende" && user.RegisteredAt == DateTime.Today
                         select user).FirstOrDefault();
            Assert.AreEqual("ayende", query.Name);
            Assert.AreEqual(DateTime.Today, query.RegisteredAt);
        }



        [Test]
        public void UsersByNameOrRegistrationDateNow()
        {

            var query = (from user in session.Linq<User>()
                         where user.Name == "ayende" || user.RegisteredAt == new DateTime(1998, 12, 31)
                         select user).ToList();
            Assert.AreEqual(query.Count, 2);

        }

        [Test]
        public void UsersRegisteredAfterY2K()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt > new DateTime(2000, 1, 1)
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void UsersRegisteredAtOrBeforeY2K()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt <= new DateTime(2000, 1, 1)
                         select user).ToList();
            Assert.AreEqual(2, query.Count);
        }

        [Test]
        public void UsersRegisteredBeforeY2K()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt < new DateTime(2000, 1, 1)
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void UsersRegisteredAtOrBeforeY2KAndNamedNHibernate()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt <= new DateTime(2000, 1, 1) && user.Name == "nhibernate"
                         select user).ToList();
            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void UsersRegisteredAtOrBeforeY2KOrNamedNHibernate()
        {
            var query = (from user in session.Linq<User>()
                         where user.RegisteredAt <= new DateTime(2000, 1, 1) || user.Name == "nhibernate"
                         select user).ToList();
            ObjectDumper.Write(query);
            Assert.AreEqual(2, query.Count);
        }

        /// <summary>
        /// do not forget to cast the query to an IENumerable if
        /// you want to test the count property.
        /// </summary>
        [Test]
        public void TestDataContext()
        {
            IEnumerable<User> query = from u in nhib.Users
                        where u.Name == "ayende"
                        select u;
            Assert.AreEqual(1, query.Count());
            Assert.AreEqual(query.ToArray<User>()[0].Name, "ayende");
        }

        [Test]
        public void UsersWithNullLoginDate()
        {
            var query = (from user in session.Linq<User>()
                         where user.LastLoginDate == null
                         select user).ToList();

            CollectionAssert.AreCountEqual(2, query);
        }

        [Test]
        public void UsersWithNonNullLoginDate()
        {
            var query = (from user in session.Linq<User>()
                         where user.LastLoginDate != null
                         select user).ToList();

            CollectionAssert.AreCountEqual(1, query);
        }

        [Test]
        public void UsersWithDynamicInvokedExpression()
        {
            //simulate dynamically created where clause
            Expression<Func<User, bool>> expr1 = u => u.Name == "ayende";
            Expression<Func<User, bool>> expr2 = u => u.Name == "rahien";

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            var dynamicWhereClause = Expression.Lambda<Func<User, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);

            var query = session.Linq<User>().Where(dynamicWhereClause).ToList();

            CollectionAssert.AreCountEqual(2, query);
        }
    }
}
