using MbUnit.Framework;
using System.Linq;
using NHibernate.Linq.SqlClient;
//using NHibernate.Linq2NhBySql.SqlClient;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class FunctionTests : BaseTest
	{
		[Test]
		public void LeftFunction()
		{
			var query = from e in db.Employees
			            where db.Methods.Substring(e.FirstName, 1, 2) == "An"
			            select db.Methods.Left(e.FirstName, 3);

			ObjectDumper.Write(query);
		}

		[Test]
		public void ReplaceFunction()
		{
			var query = from e in db.Employees
			            where e.FirstName.StartsWith("An")
			            select new
			                   	{
			                   		Before = e.FirstName,
                                    AfterMethod = e.FirstName.Replace("An", "Zan"),
                                    AfterExtension = db.Methods.Replace(e.FirstName, "An", "Zan")
			                   	};

			ObjectDumper.Write(query);
		}

		[Test]
		public void CharIndexFunction()
		{
			var query = from e in db.Employees
                        where db.Methods.CharIndex('A', e.FirstName) == 1
			            select e.FirstName;

			ObjectDumper.Write(query);
		}

		[Test]
		public void IndexOfFunctionExpression()
		{
			var query = from e in db.Employees
			            where e.FirstName.IndexOf("An") == 1
			            select e.FirstName;

			ObjectDumper.Write(query);
		}

		[Test]
		public void IndexOfFunctionProjection()
		{
			var query = from e in db.Employees
			            where e.FirstName.Contains("a")
			            select e.FirstName.IndexOf('A', 3);

			ObjectDumper.Write(query);
		}

		[Test]
		public void TwoFunctionExpression()
		{
			var query = from e in db.Employees
			            where e.FirstName.IndexOf("A") == db.Methods.Month(e.BirthDate)
			            select e.FirstName;

			ObjectDumper.Write(query);
		}

        [Test]
        public void MonthFunction()
        {
            var query = from e in db.Employees
                        where db.Methods.Month(e.BirthDate) == 1
                        select e.FirstName;
            ObjectDumper.Write(query);   
        }


        [Test]
        public void TestStringLength() {
            var query = from e in db.Employees
                        where e.FirstName.Length  < 20
                        select e.FirstName;
            ObjectDumper.Write(query);
        }

        [Test]
        public void TestStringAddWithStartsWith()
        {
            var query = from e in db.Employees
                        where (e.FirstName + e.LastName).StartsWith("A")
                        select e.FirstName;
            ObjectDumper.Write(query);
        }

        /// <summary>
        /// This test verify if there is the possibility to walk the
        /// graph of the properties
        /// </summary>
        [Test]
        public void TestWalkTheGraph()
        {
            var query = from o in db.Orders
                        where o.Customer.ContactName.StartsWith("AL")
                        select o.OrderID;
            ObjectDumper.Write(query);
        }
	}
}