using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using NHEX = NHibernate.Expressions;
using System.Reflection;

namespace NHibernate.Linq.Visitors {
	class QueryTranslator : ExpressionVisitor {

		/// <summary>
		/// this is the reference to the current projection row.
		/// </summary>
		ParameterExpression row;

		internal LinkedList<String> projectedProperties = new LinkedList<String>();

		public ICriteria rootCriteria;
		public QueryTranslator(ICriteria criteria) {
			rootCriteria = criteria;
			Translation = new QueryTranslated(this) { Criteria = rootCriteria };
			this.row = Expression.Parameter(typeof(Select.ProjectionResult), "row");
		}
		public QueryTranslated Translation { get; set; }

		private Int32 CurrentColumnIndex = 0;

		public void Translate(Expression exp) {
			//System.Linq.Expressions.Expression evaled = Evaluator.PartialEval(exp);
			Select.GroupVisitor gvisitor = new Select.GroupVisitor(row, CurrentColumnIndex);
			exp = gvisitor.Visit(exp);
			CurrentColumnIndex = gvisitor.currentColumnIndex;
			foreach (NHEX.IProjection p in gvisitor.ListOfProjections)
				Linq.Utils.NHibernate.AddProjectionToCriteria(p, rootCriteria);
			Visit(exp);
		}

		protected override Expression VisitMethodCall(MethodCallExpression m) {
			if (m.Method.DeclaringType == typeof(Queryable) ||
			 m.Method.DeclaringType == typeof(Enumerable)) {
				switch (m.Method.Name) {
					case "Where":
						WhereArgumentsVisitor vst = new WhereArgumentsVisitor(rootCriteria);
						System.Linq.Expressions.Expression exp = vst.Visit(m.Arguments[1]);
						rootCriteria.Add(vst.CurrentCriterions[0]);

						break;
					case "Select":
						HandleSelect(m);
						break;
					case "First":
						TranslateFirst(m, false);
						break;
					case "FirstOrDefault":
						TranslateFirst(m, true);
						break;
					case "Single":
						TranslateSingle(m, false);
						break;
					case "SingleOrDefault":
						TranslateSingle(m, true);
						break;
					case "Count":
						Translation = new CountQueryTranslated(this) { Criteria = rootCriteria };
						break;
					case "Sum":
						HandleProjectionOfSingleElement(m, s => NHEX.Projections.Sum(s));
						break;
					case "Min":
						HandleProjectionOfSingleElement(m, s => NHEX.Projections.Min(s));
						break;
					case "Max":
						HandleProjectionOfSingleElement(m, s => NHEX.Projections.Max(s));
						break;
					case "Average":
						HandleProjectionOfSingleElement(m, s => NHEX.Projections.Avg(s));
						break;
					case "GroupBy":
						HandleGroupBy(m);
						break;
					default:
						Console.WriteLine("Unknown method " + m.Method.Name);
						break;
				}
			}
			return base.VisitMethodCall(m);
		}

		/// <summary>
		/// Handle projection of aggregate, an aggregate is a SUm, Max or other stuff, it is valid only
		/// if is a single projection
		/// </summary>
		/// <param name="m"></param>
		private void HandleProjectionOfSingleElement(
			 System.Linq.Expressions.MethodCallExpression m,
			 Func<String, NHEX.IProjection> createProjection) {
			if (m.Arguments.Count == 2) {
				throw new ApplicationException("Cannot handle composite aggregation");
				//Select.PostfixSelectVisitor svst = new Select.PostfixSelectVisitor(row, s => NHEX.Projections.Property(s));
				//svst.Visit(Linq.Utils.Expression.StripQuotes(m.Arguments[1]));
				//projectedProperties.AddLast(svst.ProjectionsPropertyNames[0]);
				//Linq.Utils.NHibernate.AddProjectionToCriteria(
				//    createProjection(svst.ProjectionsPropertyNames[0]), rootCriteria);
			}
			Translation = new AggregateQueryTranslated(this, createProjection) { Criteria = rootCriteria };
		}

		/// <summary>
		/// Handle the select part of an expression.
		/// </summary>
		/// <param name="m"></param>
		private void HandleSelect(System.Linq.Expressions.MethodCallExpression m) {
			LambdaExpression lambda = (LambdaExpression)Linq.Utils.Expression.StripQuotes(m.Arguments[1]);
			Select.PostfixSelectVisitor svst = new Select.PostfixSelectVisitor(row, s => NHEX.Projections.Property(s));
			Expression exp = svst.Visit(lambda.Body);

			//If there are no projection there is no more work to do.
			if (svst.ListOfProjections.Count == 0) return;

			Translation.projector = Expression.Lambda(exp, row);

			for (Int32 I = 0; I < svst.ListOfProjections.Count; ++I) {
				projectedProperties.AddLast(svst.ProjectionsPropertyNames[I]);
				Linq.Utils.NHibernate.AddProjectionToCriteria(
					 svst.ListOfProjections[I], rootCriteria);
			}

		}

		private void HandleGroupBy(System.Linq.Expressions.MethodCallExpression m) {
			Select.PostfixSelectVisitor svst = new Select.PostfixSelectVisitor(row, s => NHEX.Projections.GroupProperty(s));
			LambdaExpression lambda = (LambdaExpression)Linq.Utils.Expression.StripQuotes(m.Arguments[1]);
			Expression exp = svst.Visit(lambda.Body);

			//If there are no projection there is no more work to do.
			if (svst.ListOfProjections.Count == 0) return;

			Translation.projector = Expression.Lambda(exp, row);
			for (Int32 I = 0; I < svst.ListOfProjections.Count; ++I) {
				String projectedName = svst.ProjectionsPropertyNames[I];
				projectedProperties.AddLast(projectedName);
				if (projectedName.Contains(".")) {
					rootCriteria.SetFetchMode(projectedName.Substring(0, projectedName.LastIndexOf('.')), FetchMode.Join);
				}
				Linq.Utils.NHibernate.AddProjectionToCriteria(
					 NHEX.Projections.GroupProperty(projectedName), rootCriteria);
			}

			//The m.type is the return value, it is an IEnumerable<IGrouping<T, S>>
			//Linq.Utils.LinqGroupingResultTransformer transformer =
			//   new NHibernate.Linq.Utils.LinqGroupingResultTransformer(
			//       m.Type.GetGenericArguments()[0], svst.ProjectionsPropertyNames[0]);
			//rootCriteria.SetResultTransformer(transformer);
		}

		/// <summary>
		/// Translate the  expression "first", there must be an expression on the 
		/// translation reference.
		/// </summary>
		/// <param name="m"></param>
		private void TranslateFirst(MethodCallExpression m, Boolean defaultIfNull) {
			Debug.Assert(Translation != null, "Cannot apply first without a query");
			Translation = new FirstQueryTranslated(this) { Criteria = rootCriteria, DefaultIfNull = defaultIfNull };
		}

		/// <summary>
		/// Translate the  expression "first", there must be an expression on the 
		/// translation reference.
		/// </summary>
		/// <param name="m"></param>
		private void TranslateSingle(MethodCallExpression m, Boolean defaultIfNull) {
			Debug.Assert(Translation != null, "Cannot apply first without a query");
			if (defaultIfNull)
				Translation = new SingleOrDefaultQueryTranslated(this) { Criteria = rootCriteria };
			else
				Translation = new SingleQueryTranslated(this) { Criteria = rootCriteria };
		}
	}

	/// <summary>
	/// represent the query Translated. It is an object that contains
	/// all the information about the query and knows how to deal with the
	/// result
	/// </summary>
	class QueryTranslated {
		protected QueryTranslator containerTranslator;
		internal QueryTranslated(QueryTranslator containerTranslator) {
			this.containerTranslator = containerTranslator;
		}
		/// <summary>
		/// This has not to be accessed from external code, because this
		/// class knows how to deal with the result.
		/// </summary>
		public ICriteria Criteria { get; set; }

		/// <summary>
		/// This is the projector lambda.
		/// </summary>
		public LambdaExpression projector;


		/// <summary>
		/// Get the result 
		/// Base version return a list of element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual Object GetResult<TResult>() {
			if (projector == null) {
				//There is no projector, but we can still have a single projection
				if (Criteria.Projection != null) {
					return Criteria.UniqueResult();
				}
				else { 
				return Criteria.List<TResult>();
				}
				
			}
			else {
				return GetProjectedResult<TResult>();

				//return new Select.ProjectionReader<TResult>(
				//    Criteria.List(), (Func<Select.ProjectionResult, TResult>)realProjector);
				//return Activator.CreateInstance(
				//    typeof(Select.ProjectionReader<TResult>),
				//    BindingFlags.Instance
			}
		}

		/// <summary>
		/// Returns a projected result.
		/// </summary>
		/// <returns></returns>
		private object GetProjectedResult<TResult>() {
			if (Criteria.Projection.IsGrouped) {
				Object result = Criteria.List();
				return result;
			}
			else {
				Delegate realProjector = projector.Compile();
				System.Collections.IList result = Criteria.List();
				System.Type typeToReturn = projector.Body.Type;
				return Activator.CreateInstance(
					  typeof(Select.ProjectionReader<>).MakeGenericType(typeToReturn),
					  BindingFlags.Instance | BindingFlags.NonPublic, null,
					  new object[] { result, realProjector },
					  null
					  );
			}

		}
	}

	class FirstQueryTranslated : QueryTranslated {

		internal FirstQueryTranslated(
			 QueryTranslator containerTranslator)
			: base(containerTranslator) { }

		public Boolean DefaultIfNull { get; set; }

		public override object GetResult<TResult>() {
			Criteria.SetMaxResults(1);
			TResult result = Criteria.UniqueResult<TResult>();
			if (result == null)
				if (DefaultIfNull)
					return default(TResult);
				else
					throw new InvalidOperationException("There are no element to be retrieved");

			return (TResult)result;
		}
	}

	/// <summary>
	/// The single operator is a special ones, it throw an exception
	/// if the original sequence has no element or has more than one element
	/// 
	/// </summary>
	class SingleQueryTranslated : QueryTranslated {
		internal SingleQueryTranslated(
			 QueryTranslator containerTranslator)
			: base(containerTranslator) { }

		public override object GetResult<TResult>() {
			return Criteria.SetMaxResults(2)
				 .List<TResult>()
				 .Single<TResult>();
		}
	}

	class SingleOrDefaultQueryTranslated : QueryTranslated {
		internal SingleOrDefaultQueryTranslated(
			 QueryTranslator containerTranslator)
			: base(containerTranslator) { }

		public override object GetResult<TResult>() {
			return Criteria.SetMaxResults(2)
				 .List<TResult>()
				 .SingleOrDefault<TResult>();
		}
	}

	/// <summary>
	/// Count is called on the query
	/// </summary>
	class CountQueryTranslated : QueryTranslated {

		internal CountQueryTranslated(
			 QueryTranslator containerTranslator)
			: base(containerTranslator) { }
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		public override object GetResult<TResult>() {
			Criteria.SetProjection(NHEX.Projections.RowCount());
			Object retvalue = Criteria.UniqueResult();
			return retvalue;
		}
	}

	class AggregateQueryTranslated : QueryTranslated {
		Func<String, NHEX.IProjection> createProjection;
		internal AggregateQueryTranslated(
			 QueryTranslator containerTranslator,
			 Func<String, NHEX.IProjection> createProjection)
			: base(containerTranslator) {
			this.createProjection = createProjection;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		public override object GetResult<TResult>() {
			if (containerTranslator.projectedProperties.Count == 1 &&
				 (Criteria.Projection == null || Criteria.Projection.IsGrouped == false)) {
				//This works only for a projection of singole result with not group 
				//TODO: Move in a unique handler.
				Criteria.SetProjection(createProjection(containerTranslator.projectedProperties.Single()));
				Object result = Criteria.UniqueResult();
				//Now change type since the caller wants a nullable
				if (!typeof(TResult).IsAssignableFrom(result.GetType())) {
					System.Type nullableType = typeof(TResult).GetGenericArguments()[0];
					return Convert.ChangeType(result, nullableType);
				}
				return result;
			}
			return base.GetResult<TResult>();
			throw new NotImplementedException("This kind of aggregation is not supported");

		}
	}
}
