using System.Linq;
using NHibernate.Linq;
using NHibernate.Linq2NhBySql;
using NHibernate.Linq2NhBySql.Query;

namespace NHibernate.Linq2NhBySql
{
	/// <summary>
	/// Provides a static method that enables LINQ syntax for NHibernate Criteria Queries.
	/// </summary>
	public static class NHibernateExtensions
	{
		/// <summary>
		/// Creates a new <see cref="NHibernateQueryProvider{TResult}"/> object used to evaluate an expression tree.
		/// </summary>
		/// <typeparam name="T">An NHibernate entity type.</typeparam>
		/// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
		/// <returns>An <see cref="Query"/> used to evaluate an expression tree.</returns>
		public static IOrderedQueryable<T> Linq<T>(this ISession session)
		{
			return new Query<T>(new NHibernateQueryProvider<T>(session));
		}

		/// <summary>
		/// Provide access to dbfunction.
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static IDbMethods DbMethods(this ISession session)
		{
			return null;
		}
	}
}