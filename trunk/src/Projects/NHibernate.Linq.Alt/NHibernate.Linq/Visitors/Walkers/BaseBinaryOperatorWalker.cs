using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Criterion;

namespace NHibernate.Linq.Visitors
{
    class BaseBinaryOperatorWalker : BinaryWalker
    {

        private Boolean isNegated;

        /// <summary>
        /// This is the signature of a function that takes a string and an object
        /// and create a nhibernate expression.
        /// </summary>
        Func<String, Object, ICriterion> expCreator;
        /// <summary>
        /// This create a comparison between two properties.
        /// </summary>
        Func<String, String, ICriterion> expPropertyComparisonCreator;

        internal BaseBinaryOperatorWalker(
            Func<String, Object, ICriterion> expCreator,
            Func<String, String, ICriterion> expPropertyComparisonCreator,
            ICriteria criterion)
            : base(criterion)
        {
            this.expCreator = expCreator;
            this.expPropertyComparisonCreator = expPropertyComparisonCreator;
        }

        /// <summary>
        /// This is the core function that compose two token.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal override WalkedToken Walk(WalkedToken left, WalkedToken right)
        {
            //First of all check if left is a sql expression
            if (left.TokenType == TokenType.SqlFunction)
                return WalkSqlFunction(left, right);
            //Then check if is a criteria on two properties
            if (left.TokenType == TokenType.PropertyPath && right.TokenType == TokenType.PropertyPath)
                return WalkTwoProperties(left, right);

            MemberAccessWalker w = (MemberAccessWalker) (left.TokenType == TokenType.PropertyPath ? left : right);
            Object value = left.TokenType == TokenType.PropertyPath ? right.GetValue<Object>() : left.GetValue<Object>();
            String propertyName = w.MethodName;
            ///Handle null value because it should be treated differently.
            if (value == null)
                return WalkedToken.FromCriterion(new NullExpression(propertyName));

            if (!w.FinalType.IsAssignableFrom(value.GetType())) 
                if (w.FinalType == typeof(Int32))
                    value = ((IConvertible) value).ToInt32(null);
                else if (w.FinalType == typeof(Int16))
                    value = ((IConvertible) value).ToInt16(null);
                 else if (w.FinalType == typeof(Int64))
                    value = ((IConvertible)value).ToInt64(null);
                else
                    throw new ApplicationException("Incompatible types"); 
            return WalkedToken.FromCriterion(expCreator(propertyName, value));
        }

        ///// <summary>
        ///// If a type is nullable of we must
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //private System.Type GetTypeCheckForNullable(System.Type type) { 
        //if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {…}
        //w.FinalType.GetGenericArguments()
        //}
        /// <summary>
        /// Ok we have a sql function, the inner token already created a criterion we
        /// only needs to configureIt
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private WalkedToken WalkSqlFunction(WalkedToken left, WalkedToken right)
        {
            CustomCriterion.SqlFunctionCriterion crit = (CustomCriterion.SqlFunctionCriterion)left.Criterion;
            //configureSqlCustomCriterion(crit);
				NHibernate.Criterion.ICriterion outCriterion = expCreator("", null);
            crit.StrOperator =  outCriterion.GetType().GetProperty(
                "Op", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(outCriterion, null) as String;
            crit.ValueToCompareTo = right.GetValue<Object>();
            return left;
        }

        /// <summary>
        /// Execute a comparison between two properties and not between a properties
        /// and a costant.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private WalkedToken WalkTwoProperties(WalkedToken left, WalkedToken right)
        {
            return WalkedToken.FromCriterion(
                expPropertyComparisonCreator(left.GetValue<String>(), right.GetValue<String>()));
        }
        
    }
}
