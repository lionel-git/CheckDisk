using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckDisk
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<=0)
            {
                Console.WriteLine("Syntaxe: checkDisk directory1 directory2 ...");
                return;
            }
            int delay = 10*1000;
            var cd = new CheckDirectory();
            foreach (var d in args)
            {
                Console.WriteLine($"Processing dir {d}");
                cd.ProcessDirectory(d);
                Console.WriteLine($"{cd.Count} total files processed.");
            }
            while (true)
            {
                cd.ReadFile();
                Thread.Sleep(delay);
            }
        }
    }
}
