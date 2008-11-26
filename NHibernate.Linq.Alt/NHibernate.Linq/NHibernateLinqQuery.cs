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

namespace NHibernate.Linq
{



    public class NHibernateLinqQuery<TResult> : Query.QueryProvider {
        #region Field Declarations

        private readonly ICriteria rootCriteria;
        private ICriteriaQuery criteriaQuery;
        private readonly ISession session;

        #endregion Field Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NHibernate.Linq.NHibernateLinqQuery`1"/> class.
        /// </summary>
        /// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
        public NHibernateLinqQuery(ISession session)
        {
            this.session = session;
            rootCriteria = session.CreateCriteria(typeof(TResult));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NHibernate.Linq.NHibernateLinqQuery`1"/> class.
        /// </summary>
        /// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
        /// <param name="rootCriteria">An ICritiera object to use as the root criteria query.</param>
        protected NHibernateLinqQuery(ISession session, ICriteria rootCriteria)
        {
            this.session = session;
            this.rootCriteria = rootCriteria;
        }

        #endregion Constructors

        internal ICriteria RootCriteria
        {
            get { return rootCriteria; }
        }

        internal ISession Session
        {
            get { return session; }
        }

        internal ICriteriaQuery CriteriaQuery
        {
            get
            {
                if (criteriaQuery == null)
                {
                    criteriaQuery = new CriteriaQueryTranslator(
                        (ISessionFactoryImplementor)session.SessionFactory,
                        (CriteriaImpl)rootCriteria,
                        typeof(TResult), "this");
                }
                return criteriaQuery;
            }
        }

        #region execution

        public override object Execute<T>(System.Linq.Expressions.Expression expression)
        {
            Visitors.QueryTranslator translator = new Visitors.QueryTranslator(rootCriteria);
            translator.Translate(expression);
            Object result = translator.Translation.GetResult<T>();
            return result;
        }

        public override object Execute(System.Linq.Expressions.Expression expression)
        {
            Visitors.QueryTranslator translator = new Visitors.QueryTranslator(rootCriteria);
            translator.Translate(expression);
            Object result = translator.Translation.GetResult<Object>();
            return result;
        }

        private class TranslationResult {
            public ICriterion Criterion;
        }

        public override string GetQueryText(System.Linq.Expressions.Expression expression)
        {
            return rootCriteria.ToString();
        }
        
        #endregion


    }


	}