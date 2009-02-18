using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    abstract class UnaryWalker : Walker
    {
        protected UnaryWalker(ICriteria criteria) : base(criteria) {}
        internal abstract WalkedToken Walk(WalkedToken token);
    }
}
