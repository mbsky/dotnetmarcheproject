using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace NHibernate.Linq.Visitors.CustomCriterion
{
	public class SqlFunctionCriterion : NHibernate.Criterion.AbstractCriterion
    {

        private Object mValueToCompareTo;
        public Object ValueToCompareTo
        {
            get { return mValueToCompareTo; }
            set { mValueToCompareTo = value; }
        }
        private String mStrOperator;
        public String StrOperator
        {
            get { return mStrOperator; }
            set { mStrOperator = value; }
        }

        private Int32 mPropertyNamePosition;
        private String mFunction;

        /// <summary>
        /// Parameters needed by the sql function.
        /// </summary>
        private Object[] mSqlFunctionParameter;

        public SqlFunctionCriterion(
            Int32 propertyNamePosition,
            object valueToCompareTo,
            String stringOperator,
            String function,
            params Object[] functionParameters)
        {
            mValueToCompareTo = valueToCompareTo;
            mPropertyNamePosition = propertyNamePosition;
            mStrOperator = stringOperator;
            mFunction = function.ToLower();
            mSqlFunctionParameter = functionParameters;
        }

        /// <summary>
        /// Returns to nhibernate the type of the parameters.
        /// I need only one operator.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <returns></returns>
        public override NHibernate.Engine.TypedValue[] GetTypedValues(
            NHibernate.ICriteria criteria,
				NHibernate.Criterion.ICriteriaQuery criteriaQuery)
        {

            return new TypedValue[] {
				new TypedValue(GetITypeFromCLRType(mValueToCompareTo.GetType()), mValueToCompareTo, EntityMode.Poco)};
        }

        /// <summary>
        /// This is the real core function that build the fragment of sql needed
        /// to build the criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override NHibernate.SqlCommand.SqlString ToSqlString(
            NHibernate.ICriteria criteria,
				NHibernate.Criterion.ICriteriaQuery criteriaQuery,
            IDictionary<string, IFilter> enabledFilters)
        {

            //retrieve with projection the real name of the property.
            String[] PropertyColumn = criteriaQuery.GetColumnsUsingProjection(
                criteria, (String) mSqlFunctionParameter[mPropertyNamePosition]);
            NHibernate.Dialect.Dialect dialect = criteriaQuery.Factory.Dialect;
            mSqlFunctionParameter[mPropertyNamePosition] = PropertyColumn[0]; 
            if (!dialect.Functions.ContainsKey(mFunction))
            {
                //throw new ApplicationException("Current dialect does not support " + mFunction + " function");
                //Todo for now try to set the function but without the dialect.
                return CreateQueryString(
                    BuildUnknownExpression(mFunction, mSqlFunctionParameter));
            }
            ISQLFunction func = (ISQLFunction)dialect.Functions[mFunction];
            SqlString functionResolved = func.Render(mSqlFunctionParameter, criteriaQuery.Factory);
            //Now we have the cast operation required.
            return CreateQueryString(functionResolved.ToString());
        }

        private String BuildUnknownExpression(String function, Object[] parameters)
        {
            StringBuilder b = new StringBuilder();
            b.Append(function);
            b.Append("(");
            for (Int32 ix = 0; ix < parameters.Length; ++ix) {
                if (ix != mPropertyNamePosition && (parameters[ix] is string || parameters[ix] is char))
                    b.AppendFormat("'{0}'", parameters[ix]);
                else
                    b.Append(parameters[ix]);
                b.Append(",");
            }
            b.Length -= 1;
            b.Append(")");
            return b.ToString();
        }

        private SqlString CreateQueryString(String functionResolved)
        {
            SqlStringBuilder sb = new SqlStringBuilder();
            sb.Add(functionResolved)
                .Add(mStrOperator)
                .AddParameter();
            return sb.ToSqlString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(mSqlFunctionParameter[mPropertyNamePosition])
                .Append(mStrOperator)
                .Append(mValueToCompareTo);
            return sb.ToString();
        }

        private static String GetSqlTypeFromCLRType(System.Type type)
        {
            if (type == typeof(String))
                return "String(4000)";
            else if (type == typeof(Int32))
                return "Int32";
            else
                throw new ArgumentException("Cannot handle type " + type.FullName);
        }

        private static IType GetITypeFromCLRType(System.Type type)
        {
            if (type == typeof(String))
                return NHibernate.NHibernateUtil.String;
            else if (type == typeof(Int32))
                return NHibernate.NHibernateUtil.Int32;
            else if (type == typeof(DateTime))
                return NHibernate.NHibernateUtil.DateTime;
            else
                throw new ArgumentException("Cannot handle type " + type.FullName);
        }
    }
}
