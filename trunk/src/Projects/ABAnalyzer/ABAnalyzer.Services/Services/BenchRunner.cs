using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ABAnalyzer.Services
{
    public class BenchRunner
    {
        public string ABPath { get; private set; }
        public BenchRunner(string benchPath)
        {
            this.ABPath = benchPath;
        }

        public BenchResults Run(BenchRunnerOptions options)
        {
            if(options.Bootstrap)
                ExecuteAndWaitEnd(options.CreateBootstrapOptions().ToArguments());

            var result = ExecuteAndWaitEnd(options.ToArguments());
            
            return new BenchResults(options, result);
        }

        public void Update(BenchResults bench)
        {
            if(bench.Options.Bootstrap)
                ExecuteAndWaitEnd(bench.Options.CreateBootstrapOptions().ToArguments());
            
            var result = ExecuteAndWaitEnd(bench.Options.ToArguments());

            bench.Update(result);
        
        }

        private string ExecuteAndWaitEnd(string arguments)
        {
            if (String.IsNullOrEmpty(ABPath) || !File.Exists(ABPath))
                throw new ArgumentException("Set ab.exe path before running tests");
            
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ABPath;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

    }
}
