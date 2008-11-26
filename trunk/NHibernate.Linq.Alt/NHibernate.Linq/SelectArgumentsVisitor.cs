using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Dialect.Function;
using NHibernate.Expressions;
using NHibernate.Type;
using Expression=System.Linq.Expressions.Expression;

namespace NHibernate.Linq
{
	public class SelectArgumentsVisitor<TResult> : LinqExpressionVisitor
	{
		private static readonly ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
		private static readonly ISQLFunction arithmaticDivide = new VarArgsSQLFunction("(", "/", ")");
		private static readonly ISQLFunction arithmaticMultiply = new VarArgsSQLFunction("(", "*", ")");
		private static readonly ISQLFunction arithmaticSubstract = new VarArgsSQLFunction("(", "-", ")");


		private readonly NHibernateLinqQuery<TResult> parent;
		private readonly List<IProjection> projections = new List<IProjection>();

		public SelectArgumentsVisitor(NHibernateLinqQuery<TResult> parent)
		{
			this.parent = parent;
		}

		public IProjection[] Projections
		{
			get { return projections.ToArray(); }
		}


		public ProjectionList ProjectionList
		{
			get
			{
				NHibernate.Expressions.ProjectionList list = NHibernate.Expressions.Projections.ProjectionList();
				foreach (var projection in projections)
				{
					list.Add(projection);
				}
				return list;
				 
			}
		}

		public override void VisitCallExpression(MethodCallExpression expr)
		{
			projections.Add(parent.GetProjection(expr));
		}

		public override void VisitConstantExpression(ConstantExpression expr)
		{
			projections.Add(new ConstantProjection(expr.Value));
		}

		public override void VisitConditionalExpression(ConditionalExpression expr)
		{
			var visitorTrue = new SelectArgumentsVisitor<TResult>(parent);
			visitorTrue.VisitExpression(expr.IfTrue);

			var visitorFalse = new SelectArgumentsVisitor<TResult>(parent);
			visitorFalse.VisitExpression(expr.IfFalse);

			var visitorCondition = new WhereArgumentsVisitor(parent.RootCriteria);
			visitorCondition.VisitExpression(expr.Test);
			Conjunction conjunction = NHibernate.Expressions.Expression.Conjunction();
			foreach (var criterion in visitorCondition.CurrentCriterions)
			{
				conjunction.Add(criterion);
			}

			projections.Add(
				NHibernate.Expressions.Projections
					.Conditional(conjunction, 
						visitorTrue.ProjectionList, 
						visitorFalse.ProjectionList)
						);
		}

		public override void VisitAddExpression(BinaryExpression expr)
		{
			var leftVisitor = new SelectArgumentsVisitor<TResult>(parent);
			var rightVisitor = new SelectArgumentsVisitor<TResult>(parent);
			leftVisitor.VisitExpression(expr.Left);
			rightVisitor.VisitExpression(expr.Right);

			var joinedProjections = new List<IProjection>();
			joinedProjections.AddRange(leftVisitor.projections);
			joinedProjections.AddRange(rightVisitor.projections);

			IType[] types = joinedProjections[0].GetTypes(parent.RootCriteria, parent.CriteriaQuery);
			var useConcat = types[0] is AbstractStringType;
			SqlFunctionProjection projection;
			if (useConcat)
			{
				projection = new SqlFunctionProjection("concat", types[0], joinedProjections.ToArray());
			}
			else
			{
				projection = new SqlFunctionProjection(arithmaticAddition, types[0], joinedProjections.ToArray());
			}
			projections.Add(projection);
		}

		public override void VisitMultiplyExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticMultiply);
		}

		public override void VisitSubtractExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticSubstract);
		}

		public override void VisitDivideExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticDivide);
		}

		private void VisitAritmaticOperation(BinaryExpression expr, ISQLFunction arithmaticOperation)
		{
			var leftVisitor = new SelectArgumentsVisitor<TResult>(parent);
			var rightVisitor = new SelectArgumentsVisitor<TResult>(parent);
			leftVisitor.VisitExpression(expr.Left);
			rightVisitor.VisitExpression(expr.Right);

			var joinedProjections = new List<IProjection>();
			joinedProjections.AddRange(leftVisitor.projections);
			joinedProjections.AddRange(rightVisitor.projections);
			var types = joinedProjections[0].GetTypes(parent.RootCriteria, parent.CriteriaQuery);
			var projection = new SqlFunctionProjection(arithmaticOperation, types[0], joinedProjections.ToArray());
			projections.Add(projection);
		}

		public override void VisitConvertExpression(UnaryExpression expr)
		{
			var visitor = new SelectArgumentsVisitor<TResult>(parent);
			visitor.VisitExpression(expr.Operand);
			var list = NHibernate.Expressions.Projections.ProjectionList();
			foreach (var proj in visitor.Projections)
			{
				list.Add(proj);
			}
			var projection = new CastProjection(NHibernateUtil.GuessType(expr.Type), list);
			projections.Add(projection);
		}

		public override void VisitMemberAccessExpression(MemberExpression expr)
		{
			projections.Add(parent.GetProjection(expr));
		}
	}
}