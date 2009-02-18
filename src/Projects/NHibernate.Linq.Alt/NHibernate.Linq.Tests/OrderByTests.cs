using System.Linq;
using MbUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class OrderByTests : BaseTest
	{
		[Test]
		public void AscendingOrderByClause()
		{
			var query = from c in nwnd.Customers
			            orderby c.CustomerID
			            select c.CustomerID;

			var ids = query.ToList();

			if (ids.Count > 1)
			{
				Assert.GreaterThan(ids[1], ids[0]);
			}
		}

		[Test]
		public void DescendingOrderByClause()
		{
			var query = from c in nwnd.Customers
			            orderby c.CustomerID descending
			            select c.CustomerID;

			var ids = query.ToList();

			if (ids.Count > 1)
			{
				Assert.GreaterThan(ids[0], ids[1]);
			}
		}

		[Test]
		public void ComplexAscendingOrderByClause()
		{
			var query = from c in nwnd.Customers
			            where c.Country == "Belgium"
			            orderby c.Country , c.City
			            select c.City;

			var ids = query.ToList();

			if (ids.Count > 1)
			{
				Assert.GreaterThan(ids[1], ids[0]);
			}
		}

		[Test]
		public void ComplexDescendingOrderByClause()
		{
			var query = from c in nwnd.Customers
			            where c.Country == "Belgium"
			            orderby c.Country descending , c.City descending
			            select c.City;

			var ids = query.ToList();

			if (ids.Count > 1)
			{
				Assert.GreaterThan(ids[0], ids[1]);
			}
		}

		[Test]
		public void ComplexAscendingDescendingOrderByClause()
		{
			var query = from c in nwnd.Customers
			            where c.Country == "Belgium"
			            orderby c.Country ascending , c.City descending
			            select c.City;

			var ids = query.ToList();

			if (ids.Count > 1)
			{
				Assert.GreaterThan(ids[0], ids[1]);
			}
		}
	}
}