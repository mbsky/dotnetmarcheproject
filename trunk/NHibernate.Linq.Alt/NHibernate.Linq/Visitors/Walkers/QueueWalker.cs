using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    abstract class QueueWalker: Walker
    {
        protected QueueWalker(ICriteria criteria) : base(criteria) { }
        internal abstract WalkedToken Walk(Queue<Walker> queue);
    }
}
