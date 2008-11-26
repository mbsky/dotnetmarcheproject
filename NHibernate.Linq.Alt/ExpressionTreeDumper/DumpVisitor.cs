using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ExpressionTreeDumper
{
    class DumpVisitor : ExpressionVisitor
    {
        private TreeView tv;
        private TreeNode Current { get { return nodeStack.Peek(); } }
        private TreeNode root;
        public void Visit(TreeView tv, Expression ex)
        {
            this.tv = tv;
            root = tv.Nodes.Add(ex.ToString());
            nodeStack.Push(root);
            Visit(ex);
        }

        /// <summary>
        /// The complete expression stack visited
        /// </summary>
        private Stack<TreeNode> nodeStack = new Stack<TreeNode>();

        /// <summary>
        /// Visit a node, append the node to the tree.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public override System.Linq.Expressions.Expression Visit(
            System.Linq.Expressions.Expression exp)
        {
            String NodeText = exp == null ? "NULL NODE" : exp.NodeType.ToString();
            if (CallPosition() != -1)
                NodeText = String.Format("{0}) {1}",
                    CallPosition() == 0 ? "O" : "P" + CallPosition().ToString(),
                    NodeText);
            //Add a new node into the stack, attach the node with the topm
            TreeNode newNode = nodeStack.Peek().Nodes.Add(NodeText);
            newNode.Tag = exp;
            nodeStack.Push(newNode);
            Current.Tag = exp;
            Expression result = base.Visit(exp);
            nodeStack.Pop();
            return result;
        }

        /// <summary>
        /// When a node is a parameter the base function calls Visit Parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            Current.Text = String.Format("{0}   [name={1}]", Current.Text, p.Name);
            return base.VisitParameter(p);
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            Current.Text = String.Format("{0}   [{2}.{1}]",
                Current.Text, m.Member.Name, m.Member.DeclaringType.Name);
            return base.VisitMemberAccess(m);
        }


        /// <summary>
        /// The method call is particular, in the base visitor function the visitor
        /// first visit his "Object" expression that returns the object to invokate
        /// the method on and then the parameters, so it is better to show this relation
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {

            StringBuilder parameterList = new StringBuilder();
            foreach (ParameterInfo pi in m.Method.GetParameters())
            {
                parameterList.Append(pi.ParameterType.Name);
                parameterList.Append(", ");
            }
            if (parameterList.Length > 0) parameterList.Length -= 2;
            Current.Text = String.Format("{0}   [{3} {1}.{2}({4})]",
                Current.Text, m.Method.DeclaringType.Name, m.Method.Name,
                m.Method.ReturnType.Name, parameterList.ToString());
            return base.VisitMethodCall(m);
        }



    }
}
