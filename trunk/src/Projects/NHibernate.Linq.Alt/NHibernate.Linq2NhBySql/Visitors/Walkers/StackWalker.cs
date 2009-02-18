using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// This class needs the full operand stack to work, some of the operators
    /// needs to access directly the stack of walked token to perform something.
    /// </summary>
    abstract class StackWalker : Walker
    {
        protected StackWalker(ICriteria criteria) : base(criteria) { }
        internal abstract WalkedToken Walk(Stack<WalkedToken> stack);
    }
}
