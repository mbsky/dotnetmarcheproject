using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExpressionTreeDumper
{
    public partial class ParameterSetterNotBaseType : Form
    {
        private Object currentObject;
        public Object SelectedValue { get { return currentObject; } }
        public ParameterSetterNotBaseType(Object obj)
        {
            InitializeComponent();
            currentObject = obj;
            propertyGrid1.SelectedObject = obj;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }


    }
}
