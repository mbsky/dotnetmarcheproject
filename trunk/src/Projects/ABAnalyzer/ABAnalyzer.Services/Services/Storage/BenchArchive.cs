using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABAnalyzer.Services.Storage
{
    [Serializable]
    public class BenchArchive
    {
        private IList<BenchResults> _results;

        public BenchArchive()
        {
            _results = new List<BenchResults>();
        }

        public BenchResults Add(BenchResults benchResults)
        {
            // replace??
            for (int c = 0; c < _results.Count; c++)
            {
                BenchResults result = _results[c];
                if (string.Compare(result.Options.Name, benchResults.Options.Name, true) == 0)
                {
                    _results[c] = benchResults;
                    return benchResults;
                }
            }

            // append..
            _results.Add(benchResults);
            return benchResults;
        }

        public void Refresh()
        {
            foreach (var result in Results)
            {
                result.Update();
            }
        }

        public IEnumerable<BenchResults> Results { get { return _results; } }

        public void Remove(string selected)
        {
            // replace??
            for (int c = 0; c < _results.Count; c++)
            {
                BenchResults result = _results[c];
                if (string.Compare(result.Options.Name, selected, true) == 0)
                {
                    _results.RemoveAt(c);
                    return ;
                }
            }
        }
    }
}
