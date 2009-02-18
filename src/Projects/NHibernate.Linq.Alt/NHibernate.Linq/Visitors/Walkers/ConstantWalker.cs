using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    class ConstantWalker : WalkedToken
    {

        internal ConstantWalker(Object value, ICriteria criteria) : base(TokenType.ConstantObject, null, criteria)
        {
            this.value = value;
        }

        internal override T GetValue<T>()
        {
            return (T) Value;
        }

        private Object value;
        public Object Value {
            get { return value; }
        }
    }
}
