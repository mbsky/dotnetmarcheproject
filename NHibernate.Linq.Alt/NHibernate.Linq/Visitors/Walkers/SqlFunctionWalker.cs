using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Represent a node that is a function of the database, To create a sql function
    /// we need to know the function, the opeartor, the value to compare tu and the properties
    /// to pass to the function. The function itself needs the name of the property as first value
    /// of the espression
    /// 
    /// Ex. Substring(e.FirstName, 1, 2) = "ic"
    /// 
    /// must be translated to database.with the real name of the column and so we preparea 
    /// new sqlfunctionCriteria("FirstName", "ic", "=", 1,2)
    /// Since the "=" part is not known this single node keeps care of the function to call and the constant
    /// to use
    /// </summary>
    class SqlFunctionWalker : StackWalker
    {
        private Int32  mParameterNum;
        private String mSqlFunction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameterNum">The numbers of parameters that this function accept</param>
        /// <param name="criteria"></param>
        internal SqlFunctionWalker(String sqlFunction, Int32 parameterNum, ICriteria criteria)
            : base(criteria) {
                mParameterNum = parameterNum;
                mSqlFunction = sqlFunction;
        }

        /// <summary>
        /// We need to pop from the stack all the parameters of the function to call.
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(Stack<WalkedToken> stack)
        {
          
            //The parameter to pass to sql function except the first that is the method name
            Object[] invokeArguments = new Object[mParameterNum - 1];
            Int32 propertyParameterIndex = 0;
            for (Int32 pi = invokeArguments.Length - 1; pi >= 0; --pi)
            {
                WalkedToken wt = stack.Pop();
                invokeArguments[pi] = wt.GetValue<Object>();
                if (wt.TokenType == TokenType.PropertyPath)
                    propertyParameterIndex = pi;
                
            }
            stack.Pop(); //Remove the last element that does not means nothing.
            CustomCriterion.SqlFunctionCriterion crit =
                new CustomCriterion.SqlFunctionCriterion(
                    propertyParameterIndex, null, null, mSqlFunction, invokeArguments);
            return WalkedToken.FromSqlCriterion(crit);
        }
    }
}
