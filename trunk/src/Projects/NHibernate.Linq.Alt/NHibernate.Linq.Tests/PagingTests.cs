using MbUnit.Framework;
using System.Linq;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class PagingTests : BaseTest
	{
		[Test]
		public void Customers1to5()
		{
			var q = (from c in nwnd.Customers select c.CustomerID).Take(5);
			var query = q.ToList();

			Assert.AreEqual(5, query.Count);
		}

		[Test]
		public void Customers11to20()
		{
			var query = (from c in nwnd.Customers select c.CustomerID).Skip(10).Take(10).ToList();

			Assert.AreEqual(10, query.Count);
		}
	}
}