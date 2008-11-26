using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace ExpressionTreeDumper
{
    class PostfixDumpVisitor : PostfixExpressionVisitor 
    {

        private ListView lview;
        ListViewItem current;

        public void PostfixVisit(ListView lv, Expression realexp)
        {
            lview = lv;
           base.Visit(realexp);
        }


        protected override void PVisit(Expression exp)
        {
            if (exp != null) current = lview.Items.Add(exp.ToString());
            base.PVisit(exp);
        }

       
        /// <summary>
        /// When a node is a parameter the base function calls Visit Parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override void PVisitParameter(ParameterExpression p)
        {
            current.Text = String.Format("{0}   [name={1}]", current.Text, p.Name);
            
        }

        protected override void PVisitMemberAccess(MemberExpression m)
        {
            current.Text = String.Format("{0}   [{2}.{1}]",
                current.Text, m.Member.Name, m.Member.DeclaringType.Name);
        }



      
    }
}
