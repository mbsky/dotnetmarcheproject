namespace ABAnalyzer
{
    partial class ApacheBenchRunnerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.btnBrowseForAB = new System.Windows.Forms.Button();
            this.ofdAB = new System.Windows.Forms.OpenFileDialog();
            this.txtApacheBenchFileName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.concurrency = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.requests = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress3 = new System.Windows.Forms.TextBox();
            this.chkBootstrap = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowseForAB
            // 
            this.btnBrowseForAB.Location = new System.Drawing.Point(443, 271);
            this.btnBrowseForAB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBrowseForAB.Name = "btnBrowseForAB";
            this.btnBrowseForAB.Size = new System.Drawing.Size(51, 25);
            this.btnBrowseForAB.TabIndex = 8;
            this.btnBrowseForAB.Text = "...";
            this.btnBrowseForAB.UseVisualStyleBackColor = true;
            this.btnBrowseForAB.Click += new System.EventHandler(this.btnBrowseForAB_Click);
            // 
            // ofdAB
            // 
            this.ofdAB.FileName = "ab.exe";
            this.ofdAB.Filter = "Apache Bench|ab.exe|All files|*.*";
            // 
            // txtApacheBenchFileName
            // 
            this.txtApacheBenchFileName.Location = new System.Drawing.Point(13, 273);
            this.txtApacheBenchFileName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtApacheBenchFileName.Name = "txtApacheBenchFileName";
            this.txtApacheBenchFileName.Size = new System.Drawing.Size(422, 22);
            this.txtApacheBenchFileName.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(394, 167);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 25);
            this.button2.TabIndex = 6;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(502, 12);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(636, 256);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "Page Served";
            title1.Name = "Request per second";
            title1.Text = "Request per second";
            this.chart1.Titles.Add(title1);
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(16, 30);
            this.txtAddress1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(478, 22);
            this.txtAddress1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Url 1";
            // 
            // concurrency
            // 
            this.concurrency.Location = new System.Drawing.Point(16, 170);
            this.concurrency.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.concurrency.Name = "concurrency";
            this.concurrency.Size = new System.Drawing.Size(81, 22);
            this.concurrency.TabIndex = 3;
            this.concurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.concurrency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 153);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "Concurrency";
            // 
            // requests
            // 
            this.requests.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.requests.Location = new System.Drawing.Point(105, 170);
            this.requests.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.requests.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.requests.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.requests.Name = "requests";
            this.requests.Size = new System.Drawing.Size(90, 22);
            this.requests.TabIndex = 4;
            this.requests.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.requests.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 153);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "Requests";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "Url 2";
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(16, 74);
            this.txtAddress2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(478, 22);
            this.txtAddress2.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 101);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "Url 3";
            // 
            // txtAddress3
            // 
            this.txtAddress3.Location = new System.Drawing.Point(16, 118);
            this.txtAddress3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAddress3.Name = "txtAddress3";
            this.txtAddress3.Size = new System.Drawing.Size(478, 22);
            this.txtAddress3.TabIndex = 2;
            this.txtAddress3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // chkBootstrap
            // 
            this.chkBootstrap.AutoSize = true;
            this.chkBootstrap.Checked = true;
            this.chkBootstrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBootstrap.Location = new System.Drawing.Point(217, 171);
            this.chkBootstrap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkBootstrap.Name = "chkBootstrap";
            this.chkBootstrap.Size = new System.Drawing.Size(88, 18);
            this.chkBootstrap.TabIndex = 5;
            this.chkBootstrap.Text = "Bootstrap";
            this.chkBootstrap.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "Path to Apache Bench tool ";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(502, 274);
            this.chart2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(636, 256);
            this.chart2.TabIndex = 16;
            this.chart2.Text = "Page Served";
            title2.Name = "lbl";
            title2.Text = "Document length (Kb)";
            this.chart2.Titles.Add(title2);
            // 
            // ApacheBenchRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 600);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkBootstrap);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAddress3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.requests);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.concurrency);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtApacheBenchFileName);
            this.Controls.Add(this.btnBrowseForAB);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ApacheBenchRunnerForm";
            this.Text = "Apache Bench Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseForAB;
        private System.Windows.Forms.OpenFileDialog ofdAB;
        private System.Windows.Forms.TextBox txtApacheBenchFileName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown concurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown requests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddress3;
        private System.Windows.Forms.CheckBox chkBootstrap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
    }
}

