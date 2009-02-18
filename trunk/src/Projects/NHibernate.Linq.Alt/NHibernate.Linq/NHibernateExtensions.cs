using System.Linq;

namespace NHibernate.Linq
{
	/// <summary>
	/// Provides a static method that enables LINQ syntax for NHibernate Criteria Queries.
	/// </summary>
	public static class NHibernateExtensions
	{
		/// <summary>
		/// Creates a new <see cref="T:NHibernate.Linq.NHibernateLinqQuery`1"/> object used to evaluate an expression tree.
		/// </summary>
		/// <typeparam name="T">An NHibernate entity type.</typeparam>
		/// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
		/// <returns>An <see cref="T:NHibernate.Linq.NHibernateLinqQuery`1"/> used to evaluate an expression tree.</returns>
		public static IOrderedQueryable<T> Linq<T>(this ISession session)
		{
			return new Query.Query<T>(
                new NHibernateLinqQuery<T>(session));
		}

        /// <summary>
        /// Alk PRovide access to dbfunction.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static IDbMethods DbMethods(this ISession session)
        {
            return null;
        }
	}
}