using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ABAnalyzer.Services;
using ABAnalyzer.Services.Storage;

namespace ABAnalyzer
{
    public partial class ApacheBenchRunnerForm : Form
    {
        private readonly IBenchStorage Storage;
        private BenchArchive Archive { get; set; }
        public ApacheBenchRunnerForm()
        {
            InitializeComponent();

            this.Storage = new DiskStorage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage"));
            this.Archive = new BenchArchive();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Archive = new BenchArchive();
            
            txtAddress.Text = "http://localhost/mvctemplate/home.mvc/clientsiderender";
            txtShortDescription.Text = "demo";
            SearchAB();
        }

        private void SearchAB()
        {
            string defPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                @"Apache Software Foundation\Apache2.2\bin\ab.exe"
            );

            if(File.Exists(defPath))
            {
                txtApacheBenchFileName.Text = defPath;
            }
        }

        private BenchRunnerOptions CreateOptions()
        {
            var options = new BenchRunnerOptions(txtShortDescription.Text, txtAddress.Text);
                options.Bootstrap = chkBootstrap.Checked;
                options.Concurrency = (short)concurrency.Value;
                options.Requests = (int)requests.Value;
            return options;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                BenchRunnerOptions option = CreateOptions();
                
                var runner = new BenchRunner(txtApacheBenchFileName.Text);
                var result = runner.Run(option);
                Archive.Add(result);

                UpdateVisualization();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateVisualization()
        {
            chart1.Series.Clear();
            chart2.Series.Clear();
            
            foreach (var result in this.Archive.Results)
            {
                AddResultToChart(result);
            }
        }

        private void AddResultToChart(BenchResults result)
        {
            string name = result.Options.Name;

            {
                var serie = new Series(name);
                serie.Points.AddY(result.RequestsPerSecond);
                chart1.Series.Add(serie);
            }
            
            {
                var serie = new Series(name);
                serie.Points.AddY(result.DocumentLength / 1024.0);
                chart2.Series.Add(serie);
            }
        }

        private void btnBrowseForAB_Click(object sender, EventArgs e)
        {
            if(ofdAB.ShowDialog() == DialogResult.OK)
            {
                txtApacheBenchFileName.Text = ofdAB.FileName;
            }
        }

        private void btnTestSave_Click(object sender, EventArgs e)
        {
            Storage.Save("latest", Archive);
        }

        private void btnTestLoad_Click(object sender, EventArgs e)
        {
            this.Archive = Storage.Load("latest");
            UpdateVisualization();
        }

        private void btnAddToHistory_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
