using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace ExpressionTreeDumper{
    public abstract class PostfixExpressionVisitor : ExpressionVisitor 
    {
        public Queue<Expression> postfixExp = new Queue<Expression>();
        public override Expression Visit(Expression exp)
        {
           Expression retexp = base.Visit(exp);
           PVisit(exp);
           return retexp;
        }

        public  void InnerPostfixVisit(Expression exp)
        {
                switch (exp.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                         this.PVisitUnary((UnaryExpression)exp);
                         break;
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Coalesce:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                    case ExpressionType.ExclusiveOr:
                         this.PVisitBinary((BinaryExpression)exp);
                         break;
                    case ExpressionType.TypeIs:
                         this.PVisitTypeIs((TypeBinaryExpression)exp);
                         break;
                    case ExpressionType.Conditional:
                         this.PVisitConditional((ConditionalExpression)exp);
                         break;
                    case ExpressionType.Constant:
                         this.PVisitConstant((ConstantExpression)exp);
                         break;
                    case ExpressionType.Parameter:
                         this.PVisitParameter((ParameterExpression)exp);
                         break;
                    case ExpressionType.MemberAccess:
                         this.PVisitMemberAccess((MemberExpression)exp);
                         break;
                    case ExpressionType.Call:
                         this.PVisitMethodCall((MethodCallExpression)exp);
                         break;
                    case ExpressionType.Lambda:
                         this.PVisitLambda((LambdaExpression)exp);
                         break;
                    case ExpressionType.New:
                         this.PVisitNew((NewExpression)exp);
                         break;
                    case ExpressionType.NewArrayInit:
                    case ExpressionType.NewArrayBounds:
                         this.PVisitNewArray((NewArrayExpression)exp);
                         break;
                    case ExpressionType.Invoke:
                         this.PVisitInvocation((InvocationExpression)exp);
                         break;
                    case ExpressionType.MemberInit:
                         this.PVisitMemberInit((MemberInitExpression)exp);
                         break;
                    case ExpressionType.ListInit:
                         this.PVisitListInit((ListInitExpression)exp);
                         break;
                    default:
                        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
                }
             }

        protected virtual void PVisit(Expression exp)  {
            if (exp != null) InnerPostfixVisit(exp);
        }
        protected virtual void PVisitBinding(MemberBinding binding) { }



        protected virtual void PVisitElementInitializer(ElementInit initializer) { }


        protected virtual void PVisitUnary(UnaryExpression u) { }

        protected virtual void PVisitBinary(BinaryExpression b) { }


        protected virtual void PVisitTypeIs(TypeBinaryExpression b) { }


        protected virtual void PVisitConstant(ConstantExpression c) { }


        protected virtual void PVisitConditional(ConditionalExpression c) { }


        protected virtual void PVisitParameter(ParameterExpression p) { }


        protected virtual void PVisitMemberAccess(MemberExpression m) { }

        protected virtual void PVisitMethodCall(MethodCallExpression m) { }

        protected virtual void PVisitExpressionList(ReadOnlyCollection<Expression> original) { }


        protected virtual void PVisitMemberAssignment(MemberAssignment assignment) { }

        protected virtual void PVisitMemberMemberBinding(MemberMemberBinding binding) { }


        protected virtual void PVisitMemberListBinding(MemberListBinding binding) { }


        protected virtual void PVisitBindingList(ReadOnlyCollection<MemberBinding> original) { }


        protected virtual void PVisitElementInitializerList(ReadOnlyCollection<ElementInit> original) { }

        protected virtual void PVisitLambda(LambdaExpression lambda) { }
       

        protected virtual void PVisitNew(NewExpression nex) {}


        protected virtual void PVisitMemberInit(MemberInitExpression init) { }


        protected virtual void PVisitListInit(ListInitExpression init) { }


        protected virtual void PVisitNewArray(NewArrayExpression na) { }


        protected virtual void PVisitInvocation(InvocationExpression iv) { }
       
 
    }
}
