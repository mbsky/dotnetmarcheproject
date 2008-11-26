using System;
using System.Linq;
using MbUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Collections.Generic;


namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class SelectTest : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        [Test]
        public void TestOneFieldProjection() {
            var query = from user in session.Linq<User>()
                        select user.Name;
            IList<String> result = query.ToList<String>();
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void TestAnonymousProjection()
        {
            var query = from user in session.Linq<User>()
                        select new { Name = user.Name, RegDate = user.RegisteredAt };
            Int32 count = 0;
            foreach (var el in query) {
                count++;
            }
            Assert.AreEqual(count, 3);
        }

        [Test]
        public void TestAnonymousProjectionWhere()
        {
            var query = from user in session.Linq<User>()
                        where user.Name == "ayende"
                        select new { Name = user.Name, RegDate = user.RegisteredAt };
            Int32 count = 0;
            foreach (var el in query)
            {
                Assert.AreEqual(el.Name, "ayende");
                count++;
            }
            Assert.AreEqual(count, 1);
        }

        #region Non anonymous projection

        public class Test {
            public String Name { get; set; }
            public DateTime RegDate { get; set; }
        }

        [Test]
        public void TestKnownProjectionWhere()
        {
            IEnumerable<Test> test = from user in session.Linq<User>()
                        where user.Name == "ayende"
                        select new Test() { Name = user.Name, RegDate = user.RegisteredAt };

            Int32 count = 0;
            foreach (Test el in test)
            {
                Assert.AreEqual(el.Name, "ayende");
                count++;
            }
            Assert.AreEqual(count, 1);
        }
        #endregion

        #region Aggregate

        [Test]
        public void TestSum() {
            Int32 result = (from e in session.Linq<EntityTest>()
                            select e.PInt32).Sum();
            Assert.AreEqual(result, 10);
        }

        [Test]
        public void TestSum2()
        {
            Int32 result = (from e in session.Linq<EntityTest>()
                                select e).Sum(e => e.PInt32);
            Assert.AreEqual(result, 10);
        }

        [Test]
        public void TestGroupBySum()
        {
            var result = (
                from e in session.Linq<EntityTest>()
                group e by e.PBool
                    into res
                    select res.Max(e => e.PInt32));

            Int32 count = 0;
            foreach (Int32 el in result)
            {
                count++;
            }
            Assert.AreEqual(count, 2);
        }

        [Test]
        public void TestGroupBySumAnonymousProjection()
        {
            var result = (
                from e in session.Linq<EntityTest>()
                group e by e.PBool
                    into res
                    select new { 
                        Max = res.Max(e => e.PInt32), 
                        BoolValue = res.Key
                    });

            Int32 count = 0;
            foreach (var el in result)
            {
                count++;
                if (el.BoolValue)
                    Assert.IsTrue(el.Max == 4);
                else
                    Assert.IsTrue(el.Max == 5);
            }
            Assert.AreEqual(count, 2);
        }
        #endregion
    }
}
