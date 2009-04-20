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

namespace ABAnalyzer
{
    public partial class ApacheBenchRunnerForm : Form
    {
        public ApacheBenchRunnerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtAddress1.Text = "http://localhost/mvctemplate/home.mvc/clientsiderender";
            txtAddress2.Text = "http://localhost/mvctemplate/home.mvc/fullrender";
            txtAddress3.Text = "http://localhost/mvctemplate/";
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

        private BenchRunnerOptions CreateOptions(string address)
        {
            if(String.IsNullOrEmpty(address))
                return null;

            var options = new BenchRunnerOptions(address);
                options.Bootstrap = chkBootstrap.Checked;
                options.Concurrency = (short)concurrency.Value;
                options.Requests = (int)requests.Value;
            return options;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Series.Clear();
                chart2.Series.Clear();
                

                var runner = new BenchRunner(txtApacheBenchFileName.Text);

                IList<BenchRunnerOptions> options = new List<BenchRunnerOptions>();

                options.Add(CreateOptions(txtAddress1.Text));
                options.Add(CreateOptions(txtAddress2.Text));
                options.Add(CreateOptions(txtAddress3.Text));

                foreach (var option in options)
                {
                    if(option != null)
                    {
                        var result = runner.Run(option);
                        AddResultToChart(result.DocumentPath, result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddResultToChart(string name, BenchResults result)
        {
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
