using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Visitors;
using System.Linq.Expressions;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// represent a node of MemberAccess Type, this node has the responsibility
    /// to check for a single condition like
    /// 
    /// where ent.PBool
    /// 
    /// all contition expressed on simple boolean properties are expressed with a 
    /// single invocation of a member access so it is duty of this node to create 
    /// a valid criteria
    /// </summary>
    internal class MemberAccessWalker : WalkedToken
    {

        #region Constructors

        internal MemberAccessWalker(String methodName, System.Type type, ICriteria criteria)
            : base(TokenType.PropertyPath, null, criteria)
        {
            this.methodName = methodName;
            this.realPath = methodName;
            this.finalType = type;
        }

        #endregion

        /// <summary>
        /// This overriding access has the duty to create a valid
        /// criterion for boolean property used as a single criterion.
        /// </summary>
        public override NHibernate.Expressions.ICriterion Criterion
        {
            get
            {
                return base.Criterion ?? CreateCriterionForSingleMemberAccess();
            }
        }

        internal override T GetValue<T>()
        {
            if (!typeof(T).IsAssignableFrom(typeof(String)))
                throw new ArgumentException("The MemberAccessWalker contains only string value not " + typeof(T).FullName);
            return (T)(Object)MethodName;
        }

        private String realPath;
        private String methodName = String.Empty;
        public String MethodName
        {
            get { return methodName; }
        }
        private System.Type finalType;
        public System.Type FinalType
        {
            get { return finalType; }
        }

        #region Single use of a boolean property

        private NHibernate.Expressions.ICriterion CreateCriterionForSingleMemberAccess()
        {
            if (!(FinalType == typeof(Boolean))) 
                throw new ApplicationException("This node cannot create a criterion");
            base.criterion = NHibernate.Expressions.Expression.Eq(this.MethodName, true);
            return base.criterion;
        }

        #endregion

        #region Composition methods

        /// <summary>
        /// We should do composition, when you make condition like
        /// Entest.Encontained.PStr you will ends up with a chain of member expression
        /// that are to be composed.
        /// This method is responsible to create the alias.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        internal MemberAccessWalker Compose(String name, System.Type newType)
        {
            realPath += "." + name;
            methodName = CreateAlias(realPath);
            finalType = newType;
            return this;
        }

        private String CreateAlias(String path)
        {
            String[] names = path.Split('.');
            String partial = String.Empty;
            for (Int32 I = 0; I < names.Length - 1; I++)
            {
                partial = names[I];
                if (rootCriteria.GetCriteriaByAlias(partial) == null)
                {
                    rootCriteria.CreateAlias(partial, partial);
                }

            }
            return partial + "." + names[names.Length - 1];
        }

        #endregion
    }
}
