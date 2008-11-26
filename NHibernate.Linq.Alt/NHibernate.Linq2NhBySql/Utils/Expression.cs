using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace NHibernate.Linq.Utils
{
    static class Expression
    {
        internal static System.Linq.Expressions.Expression StripQuotes(System.Linq.Expressions.Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }
    }
}
