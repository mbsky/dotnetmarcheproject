using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABAnalyzer.Services.Storage
{
    [Serializable]
    public class BenchArchive
    {
        public IList<BenchResults> Results { get; private set; }

        public BenchArchive()
        {
            Results = new List<BenchResults>();
        }

        public BenchResults Add(BenchResults benchResults)
        {
            Results.Add(benchResults);
            return benchResults;
        }
    }
}
