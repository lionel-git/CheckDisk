using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CheckDisk
{
    public class CheckDirectory
    {
        private static readonly ILog _logger = LogManager.GetLogger("CheckDir");

        private readonly Random _rnd;

        private List<string> _fileNames;

        public int Count { get { return _fileNames.Count; } }

        const int MaxRandom = 2_000_000_000;

        public CheckDirectory()
        {
            _fileNames = new List<string>();
            _rnd = new Random();
        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public void ProcessFile(string path)
        {
            //Console.WriteLine("Processed file '{0}'.", path);
            _fileNames.Add(path);
            if (_fileNames.Count%100==0)
                _logger.Info($"{_fileNames.Count} files processed.");
        }

        public void ReadFile()
        {
           
            try
            {
                int r = _rnd.Next(_fileNames.Count);
                var fileName = _fileNames[r];
                _logger.Info($"will treat {fileName}...");
                FileInfo fi = new FileInfo(fileName);
                int L = (fi.Length > MaxRandom ? MaxRandom : (int)fi.Length);
                int p = _rnd.Next(L);
                var buffer = new byte[16];  
                using (var f = File.OpenRead(fileName))
                {
                    f.Seek(p, SeekOrigin.Begin);
                    int nb=f.Read(buffer, 0, 16);
                    _logger.Info($"Read {nb} bytes at pos {L}.");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
