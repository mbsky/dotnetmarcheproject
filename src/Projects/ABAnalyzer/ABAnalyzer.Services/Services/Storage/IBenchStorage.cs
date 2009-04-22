using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABAnalyzer.Services.Storage
{
    public interface IBenchStorage
    {
        void Save(string name, BenchArchive archive);
        BenchArchive Load(string name);
        //IEnumerable<string> List();
    }
}
