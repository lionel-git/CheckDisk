using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.Configuration;
using ServiceUtils;

namespace CheckDisk
{
    class Program
    {
        static void Main(string[] args)
        {
            Starter<CheckDisk>.Start("CheckDisk", args, true);
        }
    }
}
