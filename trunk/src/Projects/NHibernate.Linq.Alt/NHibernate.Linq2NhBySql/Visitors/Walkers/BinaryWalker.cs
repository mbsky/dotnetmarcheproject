using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    abstract class BinaryWalker : Walker
    {
        protected  BinaryWalker(ICriteria criteria) : base(criteria) { }
        internal abstract WalkedToken Walk(WalkedToken left, WalkedToken right);
    }
}
