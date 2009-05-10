using ABAnalyzer.Services;

namespace ABAnalyzer
{
    public class WorkerParams
    {
        public BenchRunner Runner { get; set; }
        public BenchRunnerOptions Options { get; set; }

        public WorkerParams(BenchRunner runner, BenchRunnerOptions options)
        {
            Runner = runner;
            Options = options;
        }
    }
}