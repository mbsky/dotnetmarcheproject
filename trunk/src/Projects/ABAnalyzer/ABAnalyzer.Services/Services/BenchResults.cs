using System;
using System.Globalization;

namespace ABAnalyzer.Services
{
    [Serializable]
    public class BenchResults
    {
        private const short DataOffset = 22;

        public string RawData { get; private set; }
        public long TotalRequestTime { get; private set; }
        public long PageServed { get; private set; }
        public string ServerSofware { get; private set; }
        public string ServerHostname { get; private set; }
        public int ServerPort { get; private set; }
        public TimeSpan TimeTaken { get; private set; }

        public int CompleteRequests { get; private set; }
        public int FailedRequests { get; private set; }
        public int WriteErrors { get; private set; }

        public long TotalTransferred { get; private set; }
        public long HTMLTransferred { get; private set; }
        public double RequestsPerSecond { get; private set; }
        public double TimePerRequest { get; private set; }
        public double TransferRate { get; private set; }

        public string DocumentPath { get; private set; }
        public int DocumentLength { get; private set; }

        public BenchResults(string rawdata)
        {
            this.RawData = rawdata;
            this.TimeTaken = TimeSpan.Zero;
            Parse();
        }

        private string GetStringData(string line)
        {
            return line.Substring(DataOffset).Trim();
        }

        private string GetStringData(string line, string[] removeTokens)
        {
            string temp = line.Substring(DataOffset);

            if (null != removeTokens)
            {
                foreach (var s in removeTokens)
                {
                    temp = temp.Replace(s, "");
                }
            }

            return temp.Trim();
        }

        private int GetIntData(string line)
        {
            return int.Parse(GetStringData(line));
        }

        private int GetIntData(string line, string[] removeTokens)
        {
            return int.Parse(GetStringData(line, removeTokens));
        }

        private long GetLongData(string line)
        {
            return long.Parse(GetStringData(line));
        }

        private long GetLongData(string line, string[] removeTokens)
        {
            return long.Parse(GetStringData(line, removeTokens));
        }

        private double GetDoubleData(string line)
        {
            return GetDoubleData(line, null);
        }

        private double GetDoubleData(string line, string[] removeTokens)
        {
            return double.Parse(GetStringData(line, removeTokens).Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        }

        private void Parse()
        {
            // first split in a string array
            string[] lines = this.RawData.Replace("\r\n", "\n").Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("Server Software:"))
                {
                    ServerSofware = GetStringData(line);
                }
                else if (line.StartsWith("Server Hostname:"))
                {
                    ServerHostname = GetStringData(line);
                }
                else if (line.StartsWith("Server Port:"))
                {
                    ServerPort = int.Parse(line.Substring(DataOffset).Trim());
                }
                else if (line.StartsWith("Document Path:"))
                {
                    DocumentPath = GetStringData(line);
                }
                else if (line.StartsWith("Document Length:"))
                {
                    DocumentLength = GetIntData(line, new[] { "bytes" });
                }
                else if (line.StartsWith("Time taken for tests:"))
                {
                    string secondsstr = GetStringData(line, new[] { "seconds" });
                    string[] tk = secondsstr.Split('.');
                    int sec = int.Parse(tk[0]);
                    int ms = int.Parse(tk[1]);

                    TimeTaken = new TimeSpan(0, 0, 0, sec, ms);
                }
                else if (line.StartsWith("Complete requests:"))
                {
                    CompleteRequests = GetIntData(line);
                }
                else if (line.StartsWith("Failed requests:"))
                {
                    FailedRequests = GetIntData(line);
                }
                else if (line.StartsWith("Write errors:"))
                {
                    WriteErrors = GetIntData(line);
                }
                else if (line.StartsWith("Total transferred:"))
                {
                    TotalTransferred = GetLongData(line, new[] { "bytes" });
                }
                else if (line.StartsWith("HTML transferred:"))
                {
                    HTMLTransferred = GetLongData(line, new[] { "bytes" });
                }
                else if (line.StartsWith("Requests per second:"))
                {
                    //506.33 [#/sec] (mean)
                    RequestsPerSecond = GetDoubleData(line, new[] { "[#/sec] (mean)" });
                }
                else if (line.StartsWith("Time per request:") && !line.Contains("across"))
                {
                    //Time per request:       1.975 [ms] (mean)
                    TimePerRequest = GetDoubleData(line, new[] { "[ms] (mean)" });
                }
                else if (line.StartsWith("Transfer rate:"))
                {
                    //4333.96 [Kbytes/sec] received
                    TransferRate = GetDoubleData(line, new[] { "[Kbytes/sec] received" });
                }

            }
        }
    }
}