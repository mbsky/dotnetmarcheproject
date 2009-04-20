using System.Text;

namespace ABAnalyzer.Services
{
    public class BenchRunnerOptions
    {
        public string Url { get; set; }
        public bool Bootstrap { get; set; }
        public int Requests { get; set; }
        public short Concurrency { get; set; }
        
        public BenchRunnerOptions(string url)
        {
            this.Url = url;
            this.Concurrency = 1;
            this.Requests = 1;
        }

        public BenchRunnerOptions  CreateBootstrapOptions()
        {
            return new BenchRunnerOptions(this.Url);
        }

        public string ToArguments()
        {
            var sb = new StringBuilder();

            sb .AppendFormat(" -c {0} ", this.Concurrency)
               .AppendFormat(" -n {0} ", this.Requests)
               .Append(this.Url);
            
            return sb.ToString();
        }
    }
}