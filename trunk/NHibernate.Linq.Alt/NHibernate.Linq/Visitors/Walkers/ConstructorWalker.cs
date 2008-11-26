using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Represent a node that should construct something.
    /// </summary>
    class ConstructorWalker : StackWalker
    {
        private ConstructorInfo constructorInfo;
        internal ConstructorWalker(ConstructorInfo constructorInfo, ICriteria criteria) : base(criteria) {
            this.constructorInfo = constructorInfo;
        }

        /// <summary>
        /// This first version works only with constant expression
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(Stack<WalkedToken> stack)
        {
            Object[] invokeArguments = new Object[constructorInfo.GetParameters().Length];
            for (Int32 pi = invokeArguments.Length - 1; pi >= 0; --pi)
            {
                invokeArguments[pi] = stack.Pop().GetValue<Object>();
            }
            return WalkerFactory.FromConstant(constructorInfo.Invoke(invokeArguments), rootCriteria);
        }
    }
}
