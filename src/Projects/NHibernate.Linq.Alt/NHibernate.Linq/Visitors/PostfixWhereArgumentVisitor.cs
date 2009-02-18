using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Linq.Visitors;


namespace NHibernate.Linq
{
#if Pvisitor
    public class WhereArgumentsVisitor : PostfixExpressionVisitor, IDisposable
    {

        private IList<ICriterion> clist;
        private WalkerFactory walkerFactory;
        ICriteria rootCriteria;
        public WhereArgumentsVisitor(ICriteria rootCriteria)
        {
            this.rootCriteria = rootCriteria;
            walkerFactory = new WalkerFactory(rootCriteria);
        }

        /// <summary>
        /// This does a postfix visit and immediatly after evaluate
        /// the postfix expression.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public new System.Linq.Expressions.Expression Visit(System.Linq.Expressions.Expression exp)
        {
            System.Linq.Expressions.Expression evaled = Evaluator.PartialEval(exp);
            System.Linq.Expressions.Expression evalexp = base.Visit(evaled);
            clist = Walk();
            return evalexp;
        }

        public IList<ICriterion> CurrentCriterions
        {
            get { return clist; }
        }

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        #region executing conversion from the posfix form

        #region properties

        /// <summary>
        /// the list of walkers are keep into a list but are accessed
        /// with a stack interface.
        /// </summary>
        internal List<Walker> walkers = new List<Walker>();
        private void Push(Walker walker)
        {
            walkers.Add(walker);
        }

        private Walker Peek() { 
            return walkers.Last<Walker>();
        }

        private Walker Pop() {
            Walker walker = Peek();
            walkers.Remove(walker);
            return walker;
        }
        #endregion

        /// <summary>
        /// A parameter is ignored, since it is only a thing used by the lambda 
        /// </summary>
        /// <param name="p"></param>
        protected override void PVisitParameter(System.Linq.Expressions.ParameterExpression p)
        {
            base.PVisitParameter(p);
        }

        protected override void PVisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            WalkedToken KnownMember = null;
            KnownMember = GetKnownMemberAccess(m, KnownMember);
            if (KnownMember != null)
            {
                Push(KnownMember);
            }
            else if (IsInvocationOnDomainObject(m))
            {
                //IS really an invocation on some domain object?
                Push(walkerFactory.FromMemberAccessNode(m));
            }
            else if (Peek().GetType() == typeof(MemberAccessWalker)) { 
                //Previous element is a memberinvocation so there is the need to compose property
                ((MemberAccessWalker)Peek()).Compose(m.Member.Name, m.Type);
            }

            base.PVisitMemberAccess(m);
        }

        /// <summary>
        /// This member expression is an expression on a real domain object or is
        /// some other thing that are not related to nhibernate query.
        /// consider
        /// DateTime d = DateTime.New;
        /// 
        /// where u.RegistrationDate = d
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Boolean IsInvocationOnDomainObject(System.Linq.Expressions.MemberExpression m)
        {
           //return m.Member.ReflectedType.IsAssignableFrom(rootCriteria.CriteriaClass);
            //This is a property of base class
            if (m.Member.ReflectedType.IsAssignableFrom(rootCriteria.CriteriaClass))
                return true;
            //The previous element is a cast so we change type
            CastWalker t = Peek() as CastWalker;
            if (t != null && m.Member.ReflectedType.IsAssignableFrom(t.CastTo))
                return true;
            return false;
        }

        /// <summary>
        /// Retrieve standard member access such as DateTime.Now, DateTime.Today, etc
        /// </summary>
        /// <param name="m"></param>
        /// <param name="KnownMember"></param>
        /// <returns></returns>
        private WalkedToken GetKnownMemberAccess(System.Linq.Expressions.MemberExpression m, WalkedToken KnownMember)
        {
            if (m.Member.Name == "Today" && m.Member.ReflectedType == typeof(DateTime))
                KnownMember = walkerFactory.FromConstant(DateTime.Today);
            else if (m.Member.Name == "Today" && m.Member.ReflectedType == typeof(DateTime))
                KnownMember = walkerFactory.FromConstant(DateTime.Now);
            //else if (m.Member.Name == "Month") && m.Member.ReflectedType == typeof(DateTime))
            //    KnownMember = 
            return KnownMember;
        }

        /// <summary>
        /// We must examine if it is a known method. The FromMethodCall return
        /// the appropriate walker, null if the method does not need to 
        /// generate any walker
        /// </summary>
        /// <param name="m"></param>
        protected override void PVisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            Walker w = walkerFactory.FromMethodCall(m);
            if (w != null) Push(w);
            base.PVisitMethodCall(m);
        }

        protected override void PVisitConstant(System.Linq.Expressions.ConstantExpression c)
        {
            Push(walkerFactory.FromConstantNode(c));
            base.PVisitConstant(c);
        }

        protected override void PVisitUnary(System.Linq.Expressions.UnaryExpression u)
        {
            if (u.NodeType == System.Linq.Expressions.ExpressionType.Not)
                Push(walkerFactory.Not());
            base.PVisitUnary(u);
        }

        /// <summary>
        /// The NotEqual is not really a binaryOperator, because is a unary (NOT) applyed to binary (equal)
        /// so the base factory cannot return a binary, the factory return a standard equal so it
        /// is duty of the caller verify that is a not
        /// </summary>
        /// <param name="b"></param>
        protected override void PVisitBinary(System.Linq.Expressions.BinaryExpression b)
        {

            Push(walkerFactory.FromBinary(b));
            if (b.NodeType == System.Linq.Expressions.ExpressionType.NotEqual)
                Push(walkerFactory.Not());
            base.PVisitBinary(b);
        }

        protected override void PVisitNew(System.Linq.Expressions.NewExpression nex)
        {
            Push(walkerFactory.FromConstrucorNode(nex));
            base.PVisitNew(nex);
        }
        #endregion

        #region Resolving

        private IList<ICriterion> Walk()
        {
            Stack<WalkedToken> opStack = new Stack<WalkedToken>();
            Int32 I = 0;
            while (I < walkers.Count)
            {
                Walker w = walkers[I++];
                if (w is BinaryWalker)
                {
                    BinaryWalker b = w as BinaryWalker;
                    WalkedToken right = opStack.Pop();
                    WalkedToken left = opStack.Pop();
                    opStack.Push(b.Walk(left, right));
                }
                else if (w is UnaryWalker)
                {
                    opStack.Push(((UnaryWalker)w).Walk(opStack.Pop()));
                }

                //else if (w is QueueWalker)
                //{
                //    opStack.Push(((QueueWalker)w).Walk(walkers));
                //}
                else if (w is StackWalker)
                {
                    opStack.Push(((StackWalker)w).Walk(opStack));
                }
                else if (w is WalkedToken)
                {
                    opStack.Push((WalkedToken)w);
                }
                else
                {
                    throw new NotImplementedException("This type of walker is not supported");
                }
            }
            System.Diagnostics.Debug.Assert(opStack.Count == 1, "Expression does not produce a valid result");
            List<ICriterion> retvalue = new List<ICriterion>();
            retvalue.Add(opStack.Pop().Criterion);
            return retvalue;
        }

        #endregion
    }

#endif
}
