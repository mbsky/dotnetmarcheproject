using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// this token represent an invocation of a Sql function 
    /// </summary>
    //class SqlFunctionWalkedToken : WalkedToken
    //{
    //    public SqlFunctionWalkedToken(String propertyName, String sqlFunction,  ICriteria baseCriteria, params Object[] parametersValues)
    //        : base(TokenType.SqlFunction,
            
    //        baseCriteria)
    //    {
    //        mPropertyName = propertyName;
    //        mParametersValues = parametersValues;
    //        mSqlFunction = sqlFunction;
    //    }

    //    internal override T GetValue<T>()
    //    {
    //        CustomCriterion.SqlFunctionCriterion criterion =
    //            new CustomCriterion.SqlFunctionCriterion(mPropertyName, null, null, mSqlFunction, mParametersValues);
    //        return (T)(Object)criterion;
    //    }
        
    //}
}
