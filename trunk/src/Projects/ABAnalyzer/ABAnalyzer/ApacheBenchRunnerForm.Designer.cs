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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApacheBenchRunnerForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.btnBrowseForAB = new System.Windows.Forms.Button();
            this.ofdAB = new System.Windows.Forms.OpenFileDialog();
            this.txtApacheBenchFileName = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.concurrency = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.requests = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chkBootstrap = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnTestSave = new System.Windows.Forms.Button();
            this.btnTestLoad = new System.Windows.Forms.Button();
            this.cbxHistory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddToHistory = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.picDotNetMarcheLogo = new System.Windows.Forms.PictureBox();
            this.txtRawData = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGraphReqSec = new System.Windows.Forms.TabPage();
            this.tabGraphDocLen = new System.Windows.Forms.TabPage();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabRawResults = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.runningProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.txtHeaders = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.hlCompression = new System.Windows.Forms.LinkLabel();
            this.btnRedoAllTests = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ttDotNetMarche = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDotNetMarcheLogo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabGraphReqSec.SuspendLayout();
            this.tabGraphDocLen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.tabRawResults.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowseForAB
            // 
            this.btnBrowseForAB.Location = new System.Drawing.Point(441, 390);
            this.btnBrowseForAB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBrowseForAB.Name = "btnBrowseForAB";
            this.btnBrowseForAB.Size = new System.Drawing.Size(51, 23);
            this.btnBrowseForAB.TabIndex = 11;
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
            this.txtApacheBenchFileName.Location = new System.Drawing.Point(10, 391);
            this.txtApacheBenchFileName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtApacheBenchFileName.Name = "txtApacheBenchFileName";
            this.txtApacheBenchFileName.Size = new System.Drawing.Size(419, 22);
            this.txtApacheBenchFileName.TabIndex = 10;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(417, 430);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            legend1.Title = "Req / sec";
            legend1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            legend1.TitleSeparator = System.Windows.Forms.DataVisualization.Charting.LegendSeparatorStyle.GradientLine;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(433, 498);
            this.chart1.TabIndex = 12;
            this.chart1.Text = "Page Served";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(6, 38);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(473, 22);
            this.txtAddress.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "URL";
            // 
            // concurrency
            // 
            this.concurrency.Location = new System.Drawing.Point(6, 87);
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
            this.label2.Location = new System.Drawing.Point(6, 70);
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
            this.requests.Location = new System.Drawing.Point(97, 87);
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
            this.label3.Location = new System.Drawing.Point(93, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "Requests";
            // 
            // chkBootstrap
            // 
            this.chkBootstrap.AutoSize = true;
            this.chkBootstrap.Checked = true;
            this.chkBootstrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBootstrap.Location = new System.Drawing.Point(215, 87);
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
            this.label6.Location = new System.Drawing.Point(7, 373);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "Path to Apache Bench tool ";
            // 
            // btnTestSave
            // 
            this.btnTestSave.Location = new System.Drawing.Point(10, 430);
            this.btnTestSave.Name = "btnTestSave";
            this.btnTestSave.Size = new System.Drawing.Size(140, 23);
            this.btnTestSave.TabIndex = 6;
            this.btnTestSave.Text = "Save";
            this.btnTestSave.UseVisualStyleBackColor = true;
            this.btnTestSave.Click += new System.EventHandler(this.btnTestSave_Click);
            // 
            // btnTestLoad
            // 
            this.btnTestLoad.Location = new System.Drawing.Point(158, 430);
            this.btnTestLoad.Name = "btnTestLoad";
            this.btnTestLoad.Size = new System.Drawing.Size(140, 23);
            this.btnTestLoad.TabIndex = 7;
            this.btnTestLoad.Text = "Load";
            this.btnTestLoad.UseVisualStyleBackColor = true;
            this.btnTestLoad.Click += new System.EventHandler(this.btnTestLoad_Click);
            // 
            // cbxHistory
            // 
            this.cbxHistory.FormattingEnabled = true;
            this.cbxHistory.Location = new System.Drawing.Point(9, 28);
            this.cbxHistory.Name = "cbxHistory";
            this.cbxHistory.Size = new System.Drawing.Size(401, 22);
            this.cbxHistory.TabIndex = 0;
            this.cbxHistory.SelectedIndexChanged += new System.EventHandler(this.cbxHistory_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 14);
            this.label5.TabIndex = 22;
            this.label5.Text = "Test label";
            // 
            // btnAddToHistory
            // 
            this.btnAddToHistory.Location = new System.Drawing.Point(336, 430);
            this.btnAddToHistory.Name = "btnAddToHistory";
            this.btnAddToHistory.Size = new System.Drawing.Size(75, 23);
            this.btnAddToHistory.TabIndex = 8;
            this.btnAddToHistory.Text = "Add";
            this.btnAddToHistory.UseVisualStyleBackColor = true;
            this.btnAddToHistory.Click += new System.EventHandler(this.btnAddToHistory_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(419, 28);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Remove";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // picDotNetMarcheLogo
            // 
            this.picDotNetMarcheLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picDotNetMarcheLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDotNetMarcheLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDotNetMarcheLogo.Image = ((System.Drawing.Image)(resources.GetObject("picDotNetMarcheLogo.Image")));
            this.picDotNetMarcheLogo.Location = new System.Drawing.Point(10, 483);
            this.picDotNetMarcheLogo.Name = "picDotNetMarcheLogo";
            this.picDotNetMarcheLogo.Size = new System.Drawing.Size(151, 50);
            this.picDotNetMarcheLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDotNetMarcheLogo.TabIndex = 25;
            this.picDotNetMarcheLogo.TabStop = false;
            this.picDotNetMarcheLogo.Click += new System.EventHandler(this.picDotNetMarcheLogo_Click);
            // 
            // txtRawData
            // 
            this.txtRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRawData.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRawData.Location = new System.Drawing.Point(3, 3);
            this.txtRawData.Multiline = true;
            this.txtRawData.Name = "txtRawData";
            this.txtRawData.ReadOnly = true;
            this.txtRawData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRawData.Size = new System.Drawing.Size(433, 498);
            this.txtRawData.TabIndex = 26;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.Location = new System.Drawing.Point(338, 519);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(156, 13);
            this.lblVersion.TabIndex = 27;
            this.lblVersion.Text = "Version 0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGraphReqSec);
            this.tabControl1.Controls.Add(this.tabGraphDocLen);
            this.tabControl1.Controls.Add(this.tabRawResults);
            this.tabControl1.Location = new System.Drawing.Point(501, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(447, 531);
            this.tabControl1.TabIndex = 28;
            // 
            // tabGraphReqSec
            // 
            this.tabGraphReqSec.Controls.Add(this.chart1);
            this.tabGraphReqSec.Location = new System.Drawing.Point(4, 23);
            this.tabGraphReqSec.Name = "tabGraphReqSec";
            this.tabGraphReqSec.Padding = new System.Windows.Forms.Padding(3);
            this.tabGraphReqSec.Size = new System.Drawing.Size(439, 504);
            this.tabGraphReqSec.TabIndex = 0;
            this.tabGraphReqSec.Text = "Request / sec";
            this.tabGraphReqSec.UseVisualStyleBackColor = true;
            // 
            // tabGraphDocLen
            // 
            this.tabGraphDocLen.Controls.Add(this.chart2);
            this.tabGraphDocLen.Location = new System.Drawing.Point(4, 23);
            this.tabGraphDocLen.Name = "tabGraphDocLen";
            this.tabGraphDocLen.Padding = new System.Windows.Forms.Padding(3);
            this.tabGraphDocLen.Size = new System.Drawing.Size(439, 504);
            this.tabGraphDocLen.TabIndex = 2;
            this.tabGraphDocLen.Text = "Document length";
            this.tabGraphDocLen.UseVisualStyleBackColor = true;
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Alignment = System.Drawing.StringAlignment.Center;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            legend2.Title = "Document length (Kb)";
            legend2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            legend2.TitleSeparator = System.Windows.Forms.DataVisualization.Charting.LegendSeparatorStyle.GradientLine;
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(3, 3);
            this.chart2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(433, 498);
            this.chart2.TabIndex = 14;
            this.chart2.Text = "Page Served";
            // 
            // tabRawResults
            // 
            this.tabRawResults.Controls.Add(this.txtRawData);
            this.tabRawResults.Location = new System.Drawing.Point(4, 23);
            this.tabRawResults.Name = "tabRawResults";
            this.tabRawResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabRawResults.Size = new System.Drawing.Size(439, 504);
            this.tabRawResults.TabIndex = 1;
            this.tabRawResults.Text = "Raw result";
            this.tabRawResults.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runningProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(952, 22);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // runningProgressBar
            // 
            this.runningProgressBar.Name = "runningProgressBar";
            this.runningProgressBar.Size = new System.Drawing.Size(400, 16);
            // 
            // txtHeaders
            // 
            this.txtHeaders.AcceptsReturn = true;
            this.txtHeaders.Location = new System.Drawing.Point(5, 146);
            this.txtHeaders.Multiline = true;
            this.txtHeaders.Name = "txtHeaders";
            this.txtHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHeaders.Size = new System.Drawing.Size(475, 84);
            this.txtHeaders.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 14);
            this.label4.TabIndex = 31;
            this.label4.Text = "Headers (1 for line)";
            // 
            // hlCompression
            // 
            this.hlCompression.AutoSize = true;
            this.hlCompression.Location = new System.Drawing.Point(232, 126);
            this.hlCompression.Name = "hlCompression";
            this.hlCompression.Size = new System.Drawing.Size(81, 14);
            this.hlCompression.TabIndex = 32;
            this.hlCompression.TabStop = true;
            this.hlCompression.Text = "gzip,deflate";
            this.hlCompression.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hlCompression_LinkClicked);
            // 
            // btnRedoAllTests
            // 
            this.btnRedoAllTests.Location = new System.Drawing.Point(336, 459);
            this.btnRedoAllTests.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRedoAllTests.Name = "btnRedoAllTests";
            this.btnRedoAllTests.Size = new System.Drawing.Size(156, 23);
            this.btnRedoAllTests.TabIndex = 33;
            this.btnRedoAllTests.Text = "Run all tests";
            this.btnRedoAllTests.UseVisualStyleBackColor = true;
            this.btnRedoAllTests.Click += new System.EventHandler(this.btnRedoAllTests_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.hlCompression);
            this.groupBox1.Controls.Add(this.concurrency);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtHeaders);
            this.groupBox1.Controls.Add(this.requests);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkBootstrap);
            this.groupBox1.Location = new System.Drawing.Point(8, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 239);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // ApacheBenchRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 566);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRedoAllTests);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picDotNetMarcheLogo);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAddToHistory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbxHistory);
            this.Controls.Add(this.btnTestLoad);
            this.Controls.Add(this.btnTestSave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtApacheBenchFileName);
            this.Controls.Add(this.btnBrowseForAB);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(8, 600);
            this.Name = "ApacheBenchRunnerForm";
            this.Text = "Apache Bench Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDotNetMarcheLogo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabGraphReqSec.ResumeLayout(false);
            this.tabGraphDocLen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.tabRawResults.ResumeLayout(false);
            this.tabRawResults.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseForAB;
        private System.Windows.Forms.OpenFileDialog ofdAB;
        private System.Windows.Forms.TextBox txtApacheBenchFileName;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown concurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown requests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkBootstrap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnTestSave;
        private System.Windows.Forms.Button btnTestLoad;
        private System.Windows.Forms.ComboBox cbxHistory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddToHistory;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.PictureBox picDotNetMarcheLogo;
        private System.Windows.Forms.TextBox txtRawData;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGraphReqSec;
        private System.Windows.Forms.TabPage tabRawResults;
        private System.Windows.Forms.TabPage tabGraphDocLen;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox txtHeaders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel hlCompression;
        private System.Windows.Forms.Button btnRedoAllTests;
        private System.Windows.Forms.ToolStripProgressBar runningProgressBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip ttDotNetMarche;
    }
}

