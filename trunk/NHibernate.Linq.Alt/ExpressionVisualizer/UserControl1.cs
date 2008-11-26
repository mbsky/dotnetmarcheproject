using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace ExpressionVisualizer
{
    public partial class UserControl1 : UserControl
    {

        private DumpVisitor mDumpVisitor = new DumpVisitor();
        private PostfixDumpVisitor mPostfixDumpVisitor = new PostfixDumpVisitor();

        public UserControl1()
        {
            InitializeComponent();
        }

        private List<Expression> expressions = new List<Expression>();
        public void AddExpression(Expression expression)
        {
            expressions.add(expression);
            UpdateTree();
        }

        private void UpdateTree()
        {
            treeView1.Nodes.Clear();
            for (Int32 I = 0; I < expressions.Count; ++I)
            {
                mDumpVisitor.Visit(treeView1, expressions[I]);
            }
        }

        private void UpdateList()
        {
            lstPostOrder.Items.Clear();
            for (Int32 I = 0; I < listBox1.SelectedIndices.Count; ++I)
            {
                KeyValuePair<string, Expression> selectedElement =
                    (KeyValuePair<string, Expression>)listBox1.Items[listBox1.SelectedIndices[I]];
                mPostfixDumpVisitor.PostfixVisit(lstPostOrder, selectedElement.Value);
                lstPostOrder.Items.Add("-------------------------------------------");
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
            PrepareDynamicExecution(e.Node.Tag as LambdaExpression);
        }

        private List<TextBox> txtBoxes = new List<TextBox>();
        private void PrepareDynamicExecution(LambdaExpression ex)
        {
            lstParameters.Items.Clear();
            if (ex == null)
            {
                splitContainer2.Panel2Collapsed = true;
                return;
            }
            else
            {
                splitContainer2.Panel2Collapsed = false;
            }
            foreach (ParameterExpression p in ex.Parameters)
            {
                ListViewItem lvi = lstParameters.Items.Add(p.Name);
                lvi.SubItems.Add("Not Set");
                lvi.Tag = p;
            }
        }

        private void btnSetValue_Click(object sender, EventArgs e)
        {
            if (lstParameters.SelectedIndices.Count != 1) return;
            ParameterExpression p = (ParameterExpression)lstParameters.SelectedItems[0].Tag;
            //Check if the parameter is a base type if not we need another form.

            if (p.Type.IsPrimitive)
            {
                ParameterSetter setter = new ParameterSetter(p);
                if (setter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lstParameters.SelectedItems[0].SubItems[1].Tag = setter.SelectedValue;
                    lstParameters.SelectedItems[0].SubItems[1].Text = setter.SelectedValue.ToString();
                }
            }
            else
            {
                Object obj = lstParameters.SelectedItems[0].SubItems[1].Tag ?? Activator.CreateInstance(p.Type);
                ParameterSetterNotBaseType setter = new ParameterSetterNotBaseType(obj);
                if (setter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lstParameters.SelectedItems[0].SubItems[1].Tag = setter.SelectedValue;
                    lstParameters.SelectedItems[0].SubItems[1].Text = setter.SelectedValue.ToString();
                }
            }

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            LambdaExpression ex = treeView1.SelectedNode.Tag as LambdaExpression;
            if (ex == null) return;
            Delegate d = ex.Compile();
            Object[] parameters = new Object[lstParameters.Items.Count];
            for (Int32 I = 0; I < ex.Parameters.Count; ++I)
            {
                parameters[I] = lstParameters.Items[I].SubItems[1].Tag;
            }
            Object result = d.DynamicInvoke(parameters);
            txtResult.Text = (result ?? "NULL").ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                treeView1.Nodes.Clear();

                KeyValuePair<string, Expression> selectedElement =
                    (KeyValuePair<string, Expression>)listBox1.Items[listBox1.SelectedIndex];

                mDumpVisitor.Visit(treeView1, Evaluator.PartialEval(selectedElement.Value));
            }

        }

    }
}
