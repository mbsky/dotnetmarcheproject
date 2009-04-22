using System;
using System.Globalization;

namespace ABAnalyzer.Services
{
    [Serializable]
    public class BenchResults
    {
        [NonSerialized] private const short DataOffset = 22;
        [NonSerialized] private int _completeRequests;
        [NonSerialized] private int _documentLength;
        [NonSerialized] private string _documentPath;
        [NonSerialized] private int _failedRequests;
        [NonSerialized] private long _htmlTransferred;
        [NonSerialized] private long _pageServed;
        [NonSerialized] private double _requestsPerSecond;
        [NonSerialized] private string _serverHostname;
        [NonSerialized] private int _serverPort;
        [NonSerialized] private string _serverSofware;
        [NonSerialized] private double _timePerRequest;
        [NonSerialized] private TimeSpan _timeTaken;

        [NonSerialized] private long _totalRequestTime;
        [NonSerialized] private long _totalTransferred;
        [NonSerialized] private double _transferRate;
        [NonSerialized] private int _writeErrors;

        public BenchResults(string rawdata)
        {
            RawData = rawdata;
            TimeTaken = TimeSpan.Zero;
            Parse();
        }

        public string RawData { get; private set; }

        public long TotalRequestTime
        {
            get { return _totalRequestTime; }
            private set { _totalRequestTime = value; }
        }


        public long PageServed
        {
            get { return _pageServed; }
            private set { _pageServed = value; }
        }


        public string ServerSofware
        {
            get { return _serverSofware; }
            private set { _serverSofware = value; }
        }

        public string ServerHostname
        {
            get { return _serverHostname; }
            private set { _serverHostname = value; }
        }

        public int ServerPort
        {
            get { return _serverPort; }
            private set { _serverPort = value; }
        }

        public TimeSpan TimeTaken
        {
            get { return _timeTaken; }
            private set { _timeTaken = value; }
        }


        public int CompleteRequests
        {
            get { return _completeRequests; }
            private set { _completeRequests = value; }
        }

        public int FailedRequests
        {
            get { return _failedRequests; }
            private set { _failedRequests = value; }
        }

        public int WriteErrors
        {
            get { return _writeErrors; }
            private set { _writeErrors = value; }
        }


        public long TotalTransferred
        {
            get { return _totalTransferred; }
            private set { _totalTransferred = value; }
        }

        public long HTMLTransferred
        {
            get { return _htmlTransferred; }
            private set { _htmlTransferred = value; }
        }

        public double RequestsPerSecond
        {
            get { return _requestsPerSecond; }
            private set { _requestsPerSecond = value; }
        }

        public double TimePerRequest
        {
            get { return _timePerRequest; }
            private set { _timePerRequest = value; }
        }

        public double TransferRate
        {
            get { return _transferRate; }
            private set { _transferRate = value; }
        }

        public string DocumentPath
        {
            get { return _documentPath; }
            private set { _documentPath = value; }
        }

        public int DocumentLength
        {
            get { return _documentLength; }
            private set { _documentLength = value; }
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
                foreach (string s in removeTokens)
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
            return
                double.Parse(GetStringData(line, removeTokens).Replace(".",
                                                                       CultureInfo.CurrentCulture.NumberFormat.
                                                                           NumberDecimalSeparator));
        }

        private void Parse()
        {
            // first split in a string array
            string[] lines = RawData.Replace("\r\n", "\n").Split('\n');

            foreach (string line in lines)
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
                    DocumentLength = GetIntData(line, new[] {"bytes"});
                }
                else if (line.StartsWith("Time taken for tests:"))
                {
                    string secondsstr = GetStringData(line, new[] {"seconds"});
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
                    TotalTransferred = GetLongData(line, new[] {"bytes"});
                }
                else if (line.StartsWith("HTML transferred:"))
                {
                    HTMLTransferred = GetLongData(line, new[] {"bytes"});
                }
                else if (line.StartsWith("Requests per second:"))
                {
                    //506.33 [#/sec] (mean)
                    RequestsPerSecond = GetDoubleData(line, new[] {"[#/sec] (mean)"});
                }
                else if (line.StartsWith("Time per request:") && !line.Contains("across"))
                {
                    //Time per request:       1.975 [ms] (mean)
                    TimePerRequest = GetDoubleData(line, new[] {"[ms] (mean)"});
                }
                else if (line.StartsWith("Transfer rate:"))
                {
                    //4333.96 [Kbytes/sec] received
                    TransferRate = GetDoubleData(line, new[] {"[Kbytes/sec] received"});
                }
            }
        }

        public void Refresh()
        {
            Parse();
        }
    }
}