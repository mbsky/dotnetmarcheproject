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
        private class ComboItem
        {
            public string Name { get; private set; }
            public BenchResults Result { get; private set; }

            public ComboItem(BenchResults result)
            {
                this.Result = result;
                this.Name = result.Options.Name;
            }
        }
        
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
            cbxHistory.Text = "demo";
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
            var options = new BenchRunnerOptions(cbxHistory.Text, txtAddress.Text);
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
            chart1.ResetAutoValues();
            chart2.ResetAutoValues();
            
            foreach (var result in this.Archive.Results)
            {
                AddResultToChart(result);
            }


            UpdateCombo();
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
            Archive = Storage.Load("latest");
            UpdateVisualization();
        }

        private void btnAddToHistory_Click(object sender, EventArgs e)
        {
            BenchRunnerOptions option = CreateOptions();
            BenchResults result = new BenchResults(option, null);
            Archive.Add(result);

            UpdateVisualization();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = cbxHistory.Text;
            Archive.Remove(selected);
            UpdateVisualization();
        }

        private void UpdateCombo()
        {
            cbxHistory.BeginUpdate();
            cbxHistory.DisplayMember = "Name";
            cbxHistory.ValueMember = "Result";
            cbxHistory.Items.Clear();
            cbxHistory.Items.AddRange((from a in Archive.Results select new ComboItem (a)).ToArray());
            cbxHistory.EndUpdate();
        }

        private void cbxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem current = (ComboItem)cbxHistory.SelectedItem;
            if(current != null)
            {
                txtAddress.Text = current.Result.Options.Url;
                chkBootstrap.Checked = current.Result.Options.Bootstrap;
                requests.Value = current.Result.Options.Requests;
                concurrency.Value = current.Result.Options.Concurrency;
            }
            else
            {
                txtAddress.Text = string.Empty;
                chkBootstrap.Checked = false;
                requests.Value = 1;
                concurrency.Value = 1;
            }
        }
    }
}
