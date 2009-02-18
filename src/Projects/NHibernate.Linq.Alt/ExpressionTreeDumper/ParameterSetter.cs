using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace ExpressionTreeDumper
{
    public partial class ParameterSetter : Form
    {
        private ParameterExpression currentParameter;
        public ParameterSetter(ParameterExpression p)
        {
            InitializeComponent();
            lblName.Text = p.Name;
            txtValue.Left = lblName.Right + 4;
            txtValue.Width = btnCancel.Left - txtValue.Left - 4;
            currentParameter = p;
        }

        private object selectedValue;
        public T GetSelectedValue<T>()
        {
            return (T)selectedValue;
        }
        public object SelectedValue
        {
            get { return selectedValue; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            selectedValue = Convert.ChangeType(txtValue.Text, currentParameter.Type);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }


    }
}
