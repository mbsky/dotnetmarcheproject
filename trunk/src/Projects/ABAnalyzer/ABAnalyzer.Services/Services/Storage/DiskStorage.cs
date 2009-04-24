using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ABAnalyzer.Services.Storage
{
    public class DiskStorage : IBenchStorage
    {
        private readonly string _baseFolder;
        private const string FileExtension = "aba";

        public DiskStorage(string baseFolder)
        {
            _baseFolder = baseFolder;
        }

        public void Save(string name, BenchArchive archive)
        {
            EnsureStorageFolder();

            var formatter = new BinaryFormatter();
            using (var stream = File.Create(MakeFileName(name)))
            {
                formatter.Serialize(stream, archive);
            }
        }

        public BenchArchive Load(string name)
        {
            var fname = MakeFileName(name);
            if(!File.Exists(fname))
                return null;
            
            var formatter = new BinaryFormatter();
            using (var stream = File.OpenRead(MakeFileName(name)))
            {
                var ba = (BenchArchive)formatter.Deserialize(stream);
                if(ba != null)
                    ba.Refresh();
                return ba;
            }
        }

        public IEnumerable<string> List()
        {
            return from name in Directory.GetFiles(_baseFolder, string.Format("*.{0}", FileExtension)) 
                   orderby name ascending
                   select Path.GetFileNameWithoutExtension(name);
        }

        private string MakeFileName(string name)
        {
            return Path.Combine(_baseFolder, string.Format("{0}.{1}", name, FileExtension));
        }

        private void EnsureStorageFolder()
        {
            if(!Directory.Exists(_baseFolder))
               Directory.CreateDirectory(_baseFolder);
        }
    }
}
