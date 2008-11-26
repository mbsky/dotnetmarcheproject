using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;


namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// This class represent a token returned from a walk, this can be a simple
    /// </summary>
    class WalkedToken : Walker
    {

        //protected List<ICriterion> criterionList;
        //public List<ICriterion> CriterionList
        //{
        //    get { return criterionList ?? (criterionList = new List<ICriterion>()); }
        //}
        //public Int32 CriterionCount
        //{
        //    get { return criterionList == null ? 0 : criterionList.Count; }
        //}
        protected ICriterion criterion;
        public virtual ICriterion Criterion
        {
            get { return criterion; }
        }
       
        private TokenType tokenType;
        public TokenType TokenType
        {
            get { return tokenType; }
        }

        protected WalkedToken(TokenType type, ICriterion criterion, ICriteria criteria)
            : base(criteria)
        {
            this.tokenType = type;
            this.criterion = criterion;
        }

        /// <summary>
        /// This is the function that return the value expressed by the token, it is needed
        /// because a token can be a constant, or a result of expression so the caller does
        /// not know what he really contains.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal virtual T GetValue<T>() 
        {
            throw new NotImplementedException("The base class WalkedToken cannot contain a value, only the criterionlist");
        }

        #region Factory methods

        public static WalkedToken FromCriterion(ICriterion criterion) {
            WalkedToken token = new WalkedToken(TokenType.CriterionContainer, criterion, null);
            return token;
        }


        public static WalkedToken FromSqlCriterion(CustomCriterion.SqlFunctionCriterion criterion)
        {
            WalkedToken token = new WalkedToken(TokenType.SqlFunction , criterion, null);
            return token;
        }
        #endregion
    }
}
