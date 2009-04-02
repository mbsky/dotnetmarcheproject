using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blackboard.Models.Storage
{
    public class FileEntry
    {
        public string Name { get; set; }
        public long Bytes { get; set; }
        public String LastModified { get; set; }

        public FileEntry(System.IO.FileInfo fi)
        {
            this.Name = fi.Name;
            this.Bytes = fi.Length;
            this.LastModified = fi.LastWriteTime.ToString();
        }
    }
}
