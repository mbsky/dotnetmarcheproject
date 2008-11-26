using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;
namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// This class represent a function invoked in a string property of a member.
    /// </summary>
    class StringFunctionsWalker : BinaryWalker
    {
        internal enum StringFunction {
            StartsWith,
            EndsWith,
            Contains,
        }
        
        private StringFunction func;

        
        internal StringFunctionsWalker(StringFunction func,  ICriteria criteria) : base(criteria) {
            this.func = func;
        }

        /// <summary>
        /// We can invoke a StartsWith only in a member of a domain class.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(WalkedToken left, WalkedToken right)
        {
            System.Diagnostics.Debug.Assert(left.GetType() == typeof(MemberAccessWalker), "A string function can be invoked only on a member of a domain object");
            String searchString; 
            switch (func) {
                   case StringFunction.StartsWith:
                    searchString = right.GetValue<String>() + "%";
                    break;
                   case StringFunction.EndsWith:
                       searchString = "%" + right.GetValue<String>();
                       break;
                   case StringFunction.Contains:
                       searchString = "%" + right.GetValue<String>() + "%";
                       break;
                   default:
                       throw new NotImplementedException(String.Format("String function {0} is not implemented", func));
               }
            return WalkedToken.FromCriterion(Expression.Like(left.GetValue<String>(), searchString));
        }
    }
}
