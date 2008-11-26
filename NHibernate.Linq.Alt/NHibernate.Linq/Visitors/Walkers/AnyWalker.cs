using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Used to create a any criteria. Any criteria is used to express
    /// condition in a subexpression.
    /// </summary>
    class AnyWalker : BinaryWalker
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootCriteria"></param>
        public AnyWalker(ICriteria rootCriteria) : base(rootCriteria) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(WalkedToken left, WalkedToken right)
        {
            if (left.TokenType != TokenType.PropertyPath)
                throw new ApplicationException("Cannot apply any to token of type " + left.TokenType);
            //now create the criterion
            DetachedCriteria d = DetachedCriteria.For(rootCriteria.CriteriaClass)
                .SetProjection(Projections.Id())
                .Add(Expression.IsNotEmpty(left.GetValue<String>()))
                .Add(right.Criterion); 
            return WalkedToken.FromCriterion(Subqueries.Exists(d));
            //return WalkedToken.FromCriterion(Expression.IsNotEmpty(token.GetValue<String>()));
        }
    }
}
