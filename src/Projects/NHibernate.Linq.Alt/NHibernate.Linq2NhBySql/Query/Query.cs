using System;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

using NHibernate.Linq.Query;

namespace NHibernate.Linq2NhBySql.Query
{
	/// <summary>
	/// A default implementation of IQueryable for use with QueryProvider
	/// </summary>
	public class Query<T> : IOrderedQueryable<T>
	{
		QueryProvider provider;
		Expression expression;

		public Query(QueryProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.provider = provider;
			this.expression = Expression.Constant(this);
		}

		public Query(QueryProvider provider, Expression expression)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
			{
				throw new ArgumentOutOfRangeException("expression");
			}
			this.provider = provider;
			this.expression = expression;
		}

		Expression IQueryable.Expression
		{
			get { return this.expression; }
		}

		System.Type IQueryable.ElementType
		{
			get { return typeof(T); }
		}

		IQueryProvider IQueryable.Provider
		{
			get { return this.provider; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)this.provider.Execute<T>(this.expression)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
		}

		public override string ToString()
		{
			return this.provider.GetQueryText(this.expression);
		}
	}
}