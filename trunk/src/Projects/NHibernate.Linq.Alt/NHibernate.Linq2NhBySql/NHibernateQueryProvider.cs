using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NHibernate.Engine;
using NHibernate.Expressions;
using NHibernate.Impl;
//using NHibernate.Linq.Expressions;
using NHibernate.Loader.Criteria;
using NHibernate.Transform;
using NHibernate.Type;
using LinqExpression = System.Linq.Expressions.Expression;
using NHExpression = NHibernate.Expressions.Expression;

namespace NHibernate.Linq2NhBySql
{
	public class NHibernateQueryProvider<TResult> : Linq.Query.QueryProvider
	{

		private ISession session;

		public NHibernateQueryProvider(ISession session)
		{
			this.session = session;
		}

		public override string GetQueryText(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}

		public override object Execute<T>(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}

		public override object Execute(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}