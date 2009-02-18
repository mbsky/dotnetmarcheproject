using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using System.Reflection;
using LE = System.Linq.Expressions;

namespace NHibernate.Linq.Visitors.Select
{
    /// <summary>
    /// Visit a select subtree using postfix traversal, it returns
    /// the list of projections to insert in the query
    /// </summary>
    class PostfixSelectVisitor : ExpressionVisitor
    {

        LE.ParameterExpression row;
        static MethodInfo miGetValue;
        Int32 currentColumnIndex = 0;
        Func<String, IProjection> projectionCreator;
        /// <summary>
        /// Grab a method info that represent a call to the GetValue
        /// method of a ProjectionResult.
        /// </summary>
        static PostfixSelectVisitor()
        {
            miGetValue = typeof(ProjectionResult).GetMethod("GetValue");
        }

        public PostfixSelectVisitor(LE.ParameterExpression row, Func<String, IProjection> projectionCreator)
        {
            projections = new List<IProjection>();
            ProjectionsPropertyNames = new List<String>();
            this.projectionCreator = projectionCreator;
            this.row = row;
        }

        /// <summary>
        /// Todo, create projection from external code
        /// </summary>
        public IList<IProjection> ListOfProjections
        {
            get { return projections; }
        }
        private IList<IProjection> projections;

        public List<String> ProjectionsPropertyNames { get; set; }



        private String currentMemberAccess;

        /// <summary>
        /// Access of member, should create a projection and convert the expression.
        /// When we found a memberAccess we should substitute to a call to ourt
        /// object that implements projectionResult, so we should convert
        /// </summary>
        /// <param name="m"></param>
        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (m.Expression.NodeType == System.Linq.Expressions.ExpressionType.Parameter)
            {
                String memberName = ComposeMemberAccess(m);
                projections.Add(projectionCreator(memberName));
                ProjectionsPropertyNames.Add(memberName);
                currentMemberAccess = String.Empty;
                return LE.Expression.Convert( //Call the GetValue on row passing currentcolumnindex as argument.
                    LE.Expression.Call(
                        row, 
                        miGetValue, 
                        LE.Expression.Constant(currentColumnIndex++)),
                    m.Type);
            }
            else if (m.Expression.NodeType == System.Linq.Expressions.ExpressionType.MemberAccess) {
                ComposeMemberAccess(m);
            }
            return base.VisitMemberAccess(m);
        }

        private String ComposeMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            currentMemberAccess = m.Member.Name + 
                (String.IsNullOrEmpty(currentMemberAccess) ? "" : "." + currentMemberAccess);
            return currentMemberAccess;
        }

    }
}
