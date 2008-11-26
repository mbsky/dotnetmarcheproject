using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;

namespace NHibernate.Linq.Visitors
{
    class UnaryAnyWalker : UnaryWalker
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootCriteria"></param>
        public UnaryAnyWalker(ICriteria rootCriteria) : base(rootCriteria) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(WalkedToken token)
        {
            if (token.TokenType != TokenType.PropertyPath)
                throw new ApplicationException("Cannot apply any to token of type " + token.TokenType);
            return WalkedToken.FromCriterion(Expression.IsNotEmpty(token.GetValue<String>()));
        }

    }
}
