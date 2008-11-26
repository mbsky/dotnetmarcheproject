using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace NHibernate.Linq.Visitors
{
    class AndWalker : BinaryWalker
    {
        internal AndWalker(ICriteria criterion) : base(criterion) {        }

        internal override WalkedToken Walk(WalkedToken left, WalkedToken right)
        {
            Conjunction crit = new Conjunction();

            crit.Add(left.Criterion);
            crit.Add(right.Criterion);
            return WalkedToken.FromCriterion(crit);
        }
    }
}
