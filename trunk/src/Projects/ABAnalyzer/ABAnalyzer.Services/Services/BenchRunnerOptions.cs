using System;
using System.Collections.Generic;
using System.Text;

namespace ABAnalyzer.Services
{
    [Serializable]
    public class BenchRunnerOptions
    {
        public string Url { get; set; }
        public bool Bootstrap { get; set; }
        public int Requests { get; set; }
        public short Concurrency { get; set; }
        public string Name { get; set; }
        private List<string> _headers;
        public List<string> Headers
        {
            get { return _headers ?? (_headers = new List<string>()); }
        }

        public BenchRunnerOptions(string name, string url)
        {
            this.Name = name;
            this.Url = url;
            this.Concurrency = 1;
            this.Requests = 1;
        }

        public BenchRunnerOptions  CreateBootstrapOptions()
        {
            return new BenchRunnerOptions("bootstrapper", this.Url);
        }

        public string ToArguments()
        {
            var sb = new StringBuilder();

            sb .AppendFormat(" -c {0} ", this.Concurrency)
               .AppendFormat(" -n {0} ", this.Requests);

            foreach (var header in Headers)
            {
                if(!String.IsNullOrEmpty(header))
                    sb.AppendFormat(" -H \"{0}\" ", header);
            }
            
            sb.Append(this.Url);
            
            return sb.ToString();
        }
    }
}