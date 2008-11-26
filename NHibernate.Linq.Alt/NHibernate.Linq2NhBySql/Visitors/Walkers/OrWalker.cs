using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;
namespace NHibernate.Linq.Visitors
{
    class OrWalker: BinaryWalker
    {
        internal OrWalker(ICriteria criterion) : base(criterion) { }

        internal override WalkedToken Walk(WalkedToken left, WalkedToken right)
        {
            Disjunction crit = new Disjunction();

            crit.Add(left.Criterion);
            crit.Add(right.Criterion);
            return WalkedToken.FromCriterion(crit);
        }
    }
}
