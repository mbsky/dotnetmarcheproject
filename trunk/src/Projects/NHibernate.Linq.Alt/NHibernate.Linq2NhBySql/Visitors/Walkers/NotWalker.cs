using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    class NotWalker : UnaryWalker
    {

        internal NotWalker(ICriteria criteria) : base(criteria) { }

        internal override WalkedToken Walk(WalkedToken token)
        {

            return WalkedToken.FromCriterion( NHibernate.Expressions.Expression.Not(token.Criterion));
        }
    }
}
