using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;
using System.Reflection;
using LE = System.Linq.Expressions;
using NHEX = NHibernate.Expressions;
namespace NHibernate.Linq.Visitors.Select {
	/// <summary>
	/// Find aggregator.
	/// </summary>
	class GroupVisitor : ExpressionVisitor {
		LE.ParameterExpression row;
		static MethodInfo miGetValue;
		public Int32 currentColumnIndex = 0;

		/// <summary>
		/// Todo, create projection from external code
		/// </summary>
		public IList<IProjection> ListOfProjections {
			get { return projections; }
		}
		private IList<IProjection> projections;

		public List<String> ProjectionsPropertyNames { get; set; }

		/// <summary>
		/// Grab a method info that represent a call to the GetValue
		/// method of a ProjectionResult.
		/// </summary>
		static GroupVisitor() {
			miGetValue = typeof(ProjectionResult).GetMethod("GetValue");
		}

		public GroupVisitor(
			 LE.ParameterExpression row,
			 Int32 currentColumnIndex) {
			projections = new List<IProjection>();
			ProjectionsPropertyNames = new List<String>();
			this.row = row;
		}

		protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m) {
			switch (m.Method.Name) {
				case "Sum":
					return HandleProjectionOfSingleElement(m, s => NHEX.Projections.Sum(s));
				case "Min":
					return HandleProjectionOfSingleElement(m, s => NHEX.Projections.Min(s));
				case "Max":
					return HandleProjectionOfSingleElement(m, s => NHEX.Projections.Max(s));
				case "Average":
					return HandleProjectionOfSingleElement(m, s => NHEX.Projections.Avg(s));
			}
			return base.VisitMethodCall(m);
		}

		/// <summary>
		/// Handle a project
		/// </summary>
		/// <param name="m"></param>
		/// <param name="createProjection"></param>
		private LE.Expression HandleProjectionOfSingleElement(
		  System.Linq.Expressions.MethodCallExpression m,
		  Func<String, NHEX.IProjection> createProjection) {
			if (m.Arguments.Count == 2) {
				LE.LambdaExpression l = (LE.LambdaExpression)Linq.Utils.Expression.StripQuotes(m.Arguments[1]);
				LE.MemberExpression me = (LE.MemberExpression)l.Body;

				String memberName = me.Member.Name;
				ProjectionsPropertyNames.Add(memberName);
				projections.Add(createProjection(memberName));

				return LE.Expression.Convert( //Call the GetValue on row passing currentcolumnindex as argument.
					 LE.Expression.Call(
						  row,
						  miGetValue,
						  LE.Expression.Constant(currentColumnIndex++)),
					 me.Type);

			}
			return m;

		}
		//if (m.Arguments.Count == 2)
		//  {
		//      Select.PostfixSelectVisitor svst = new Select.PostfixSelectVisitor(row, s => NHEX.Projections.Property(s));
		//      svst.Visit(Linq.Utils.Expression.StripQuotes(m.Arguments[1]));
		//      projectedProperties.AddLast(svst.ProjectionsPropertyNames[0]);
		//      Linq.Utils.NHibernate.AddProjectionToCriteria(
		//          createProjection(svst.ProjectionsPropertyNames[0]), rootCriteria);
		//  }
		//  Translation = new AggregateQueryTranslated(this, createProjection) { Criteria = rootCriteria };
	}
}
