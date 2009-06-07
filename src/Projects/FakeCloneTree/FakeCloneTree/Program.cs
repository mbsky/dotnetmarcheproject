using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FakeCloneTree
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = null;
            string destination = null;

            foreach (var cmdlineparam in args)
            {
                if (source == null)
                {
                    source = cmdlineparam;
                    continue;                 
                }

                if (destination == null)
                {
                    destination = cmdlineparam;
                    continue;                
                }
                
                // undef...
                DisplayUsage();
                return;
            }

            if(source == null || destination == null)
            {
                DisplayUsage();
                return;
            }

            if(!Directory.Exists(source))
            {
                Console.WriteLine("Source folder does not exists!");
                return;
            }

            try
            {
                Console.Write("Cloning....");
                DoClone(source, destination);
                Console.WriteLine("...done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("FakeCloneTree " + typeof(Program).Assembly.GetName().Version);
            Console.WriteLine("usage: FakeCloneTree sourceFolder destinationFolder");
        }

        static private void DoClone(string baseFolder, string destinationFolder)
        {
            // clone folders tree
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string[] folders = Directory.GetDirectories(baseFolder);

            foreach (var folder in folders)
            {
                string dn = folder.Remove(0, baseFolder.Length+1);
                string dest = Path.Combine(destinationFolder, dn);
                if(!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);

                // recursion...
                DoClone(folder, dest);
            }

            // create fake files
            string[] files = Directory.GetFiles(baseFolder);
            foreach (var file in files)
            {
                string dest = Path.Combine(destinationFolder, Path.GetFileName(file));

                if(!File.Exists(dest))
                    File.CreateText(dest).Close();
            }
        }
    }
}
