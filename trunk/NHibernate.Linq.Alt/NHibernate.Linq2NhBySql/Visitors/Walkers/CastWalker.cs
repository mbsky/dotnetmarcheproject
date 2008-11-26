using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    class CastWalker : UnaryWalker
    {
        private System.Type mCastTo;
        public System.Type CastTo {
            get { return mCastTo; }
        }    
        internal CastWalker(System.Type castTo, ICriteria criteria) : base(criteria) {
            mCastTo = castTo;
        }

        internal override WalkedToken Walk(WalkedToken token)
        {

            return token;
        }
    }
}
