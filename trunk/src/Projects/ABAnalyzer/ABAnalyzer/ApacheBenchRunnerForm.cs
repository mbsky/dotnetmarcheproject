using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string _baseFolder;
        private BackgroundWorker _singleTestWorker;
        private BackgroundWorker _allTestsWorker;

        public ApacheBenchRunnerForm()
        {
            InitializeComponent();

            _baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            this.Storage = new DiskStorage(Path.Combine(_baseFolder, "Storage"));
            this.Archive = new BenchArchive();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _singleTestWorker = new BackgroundWorker();
            _singleTestWorker.DoWork += SingleTestWorkerDoWork;
            _singleTestWorker.RunWorkerCompleted += SingleTestWorkerRunWorkerCompleted;

            _allTestsWorker = new BackgroundWorker();
            _allTestsWorker.DoWork += AllTestsWorkerDoWork;
            _allTestsWorker.ProgressChanged += AllTestsWorkerProgressChanged;
            _allTestsWorker.WorkerReportsProgress = true;
            _allTestsWorker.WorkerSupportsCancellation = true;
            _allTestsWorker.RunWorkerCompleted += AllTestsWorkerRunWorkerCompleted;

            SetupVersion();
            this.Archive = new BenchArchive();

            //            txtAddress.Text = "http://localhost/mvctemplate/home.mvc/clientsiderender";
            cbxHistory.Text = "demo";
            SearchAB();
        }


        void SingleTestWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BenchResults result = (BenchResults) e.Result;
            if (null != result)
            {
                Archive.Add(result);
                UpdateVisualization();
            }
        }

        private void SingleTestWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            WorkerParams workerParams = (WorkerParams)e.Argument;
            e.Result = workerParams.Runner.Run(workerParams.Options);
        }

        private void SetupVersion()
        {
            lblVersion.Text = "Version " +
            Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void SearchAB()
        {
            var searchPaths = new[]
                {
                    Path.Combine(_baseFolder, "ApacheBench\\ab.exe"),
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        @"Apache Software Foundation\Apache2.2\bin\ab.exe"
                        )
                };


            foreach (var path in searchPaths)
            {
                if (File.Exists(path))
                {
                    txtApacheBenchFileName.Text = path;
                    return;
                }
            }
        }

        private BenchRunnerOptions CreateOptions()
        {
            var options = new BenchRunnerOptions(cbxHistory.Text, txtAddress.Text);
            options.Bootstrap = chkBootstrap.Checked;
            options.Concurrency = (short)concurrency.Value;
            options.Requests = (int)requests.Value;
            options.Headers.AddRange(txtHeaders.Lines);
            return options;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAddress.Text))
                return;

            BenchRunnerOptions option = CreateOptions();
            var runner = new BenchRunner(txtApacheBenchFileName.Text);
            WorkerParams p = new WorkerParams(runner, option);
            _singleTestWorker.RunWorkerAsync(p);
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
            if (ofdAB.ShowDialog() == DialogResult.OK)
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
            var loaded = Storage.Load("latest");
            if (loaded != null)
            {
                Archive = loaded;
                UpdateVisualization();
            }
            else
            {
                MessageBox.Show("File not found!");
            }
        }

        private void btnAddToHistory_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAddress.Text))
                return;

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
            var list = (from a in Archive.Results select new ComboItem(a)).ToList();
            
            string current = cbxHistory.Text;
            
            cbxHistory.BeginUpdate();
            cbxHistory.DisplayMember = "Name";
            cbxHistory.ValueMember = "Result";
            cbxHistory.Items.Clear();
            cbxHistory.Items.AddRange(list.ToArray());

            if (list.Count( x => String.Compare(x.Name, current, true) == 0) > 0)
            {
                cbxHistory.Text = current;
            }
            else
            {
                if (cbxHistory.Items.Count > 0)
                    cbxHistory.Text = ((ComboItem) (cbxHistory.Items[0])).Name;
            }

            cbxHistory.EndUpdate();
        }

        private void cbxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem current = (ComboItem)cbxHistory.SelectedItem;
            if (current != null)
            {
                txtAddress.Text = current.Result.Options.Url;
                chkBootstrap.Checked = current.Result.Options.Bootstrap;
                requests.Value = current.Result.Options.Requests;
                concurrency.Value = current.Result.Options.Concurrency;
                txtRawData.Text = current.Result.RawData;
                txtHeaders.Text = string.Join("\r\n", current.Result.Options.Headers.ToArray());
            }
            else
            {
                txtAddress.Text = string.Empty;
                txtRawData.Text = string.Empty;
                txtHeaders.Text = string.Empty;
                chkBootstrap.Checked = false;
                requests.Value = 1;
                concurrency.Value = 1;
            }
        }

        private void hlCompression_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtHeaders.Lines.LastOrDefault()))
                txtHeaders.Text += "\r\n";
            txtHeaders.Text += "Accept-Encoding:gzip,deflate";
        }

        private void btnRedoAllTests_Click(object sender, EventArgs e)
        {
            if (_allTestsWorker.IsBusy)
            {
                _allTestsWorker.CancelAsync();
                btnRedoAllTests.Text = "Run all tests";
                return;
            }

            if (Archive.Results.Count() > 0)
            {
                runningProgressBar.Value = 0;
                runningProgressBar.Maximum = 100;

                btnRedoAllTests.Text = "Stop";
                var runner = new BenchRunner(txtApacheBenchFileName.Text);
                _allTestsWorker.RunWorkerAsync(runner);
            }
        }
        
        void AllTestsWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateVisualization();
        }

        void AllTestsWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            double step = 100.0 / this.Archive.Results.Count();
            BenchRunner runner = (BenchRunner)e.Argument;
            BackgroundWorker worker = (BackgroundWorker) sender;

            int nCount = 0;
            foreach (var test in this.Archive.Results)
            {
                if(worker.CancellationPending)
                {
                    break;
                }
                
                nCount++;
                runner.Update(test);
                worker.ReportProgress((int)(nCount * step));
            }
            
            worker.ReportProgress(0);
        }
        
        void AllTestsWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            runningProgressBar.Value = e.ProgressPercentage;
        }
    }
}
