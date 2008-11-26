namespace ExpressionTreeDumper
{
    partial class TreeVisualizer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.lstPostOrder = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.gbDynamic = new System.Windows.Forms.GroupBox();
            this.lstParameters = new System.Windows.Forms.ListView();
            this.pname = new System.Windows.Forms.ColumnHeader();
            this.pvalue = new System.Windows.Forms.ColumnHeader();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnSetValue = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbDynamic.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(549, 335);
            this.splitContainer1.SplitterDistance = 319;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.lstPostOrder);
            this.splitContainer4.Size = new System.Drawing.Size(319, 335);
            this.splitContainer4.SplitterDistance = 161;
            this.splitContainer4.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(161, 335);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // lstPostOrder
            // 
            this.lstPostOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstPostOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPostOrder.Location = new System.Drawing.Point(0, 0);
            this.lstPostOrder.Name = "lstPostOrder";
            this.lstPostOrder.Size = new System.Drawing.Size(154, 335);
            this.lstPostOrder.TabIndex = 0;
            this.lstPostOrder.UseCompatibleStateImageBehavior = false;
            this.lstPostOrder.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Postorder expression";
            this.columnHeader1.Width = 227;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gbDynamic);
            this.splitContainer2.Size = new System.Drawing.Size(226, 335);
            this.splitContainer2.SplitterDistance = 164;
            this.splitContainer2.TabIndex = 2;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(226, 164);
            this.propertyGrid1.TabIndex = 0;
            // 
            // gbDynamic
            // 
            this.gbDynamic.Controls.Add(this.lstParameters);
            this.gbDynamic.Controls.Add(this.txtResult);
            this.gbDynamic.Controls.Add(this.label1);
            this.gbDynamic.Controls.Add(this.btnExecute);
            this.gbDynamic.Controls.Add(this.btnSetValue);
            this.gbDynamic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDynamic.Location = new System.Drawing.Point(0, 0);
            this.gbDynamic.Name = "gbDynamic";
            this.gbDynamic.Size = new System.Drawing.Size(226, 167);
            this.gbDynamic.TabIndex = 1;
            this.gbDynamic.TabStop = false;
            this.gbDynamic.Text = "Dynamic Invocation";
            // 
            // lstParameters
            // 
            this.lstParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pname,
            this.pvalue});
            this.lstParameters.HideSelection = false;
            this.lstParameters.LabelEdit = true;
            this.lstParameters.Location = new System.Drawing.Point(6, 19);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(214, 115);
            this.lstParameters.TabIndex = 0;
            this.lstParameters.UseCompatibleStateImageBehavior = false;
            this.lstParameters.View = System.Windows.Forms.View.Details;
            // 
            // pname
            // 
            this.pname.Text = "Param Name";
            this.pname.Width = 156;
            // 
            // pvalue
            // 
            this.pvalue.Text = "Param Value";
            this.pvalue.Width = 196;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(211, 140);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(8, 20);
            this.txtResult.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Result";
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExecute.Location = new System.Drawing.Point(87, 138);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnSetValue
            // 
            this.btnSetValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetValue.Location = new System.Drawing.Point(6, 138);
            this.btnSetValue.Name = "btnSetValue";
            this.btnSetValue.Size = new System.Drawing.Size(75, 23);
            this.btnSetValue.TabIndex = 1;
            this.btnSetValue.Text = "Set Value";
            this.btnSetValue.UseVisualStyleBackColor = true;
            this.btnSetValue.Click += new System.EventHandler(this.btnSetValue_Click);
            // 
            // TreeVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TreeVisualizer";
            this.Size = new System.Drawing.Size(549, 335);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.gbDynamic.ResumeLayout(false);
            this.gbDynamic.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView lstPostOrder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.GroupBox gbDynamic;
        private System.Windows.Forms.ListView lstParameters;
        private System.Windows.Forms.ColumnHeader pname;
        private System.Windows.Forms.ColumnHeader pvalue;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnSetValue;
    }
}
