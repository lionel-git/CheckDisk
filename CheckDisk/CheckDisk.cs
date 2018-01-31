using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceUtils;
using System.Threading;
using log4net;
using System.Configuration;

namespace CheckDisk 
{
    class CheckDisk : IService
    {
        private static readonly ILog _logger = LogManager.GetLogger("CheckDisk");
        private Task _task;
        private CheckDirectory _checkDirectory;
        private int _delay;
        volatile bool _running = true;

        private void MainLoop()
        {
            while (_running)
            {
                _checkDirectory.ReadFile();
                Thread.Sleep(_delay);
            }
            _logger.Info("Exiting main loop");
        }

        public void OnStart(string[] args)
        {
            _logger.Info("=====================================================");
            var directories = ConfigurationManager.AppSettings["Directories"].ToString().Split(',').ToList();
            directories.AddRange(args);

            _delay = 10 * 1000;
            _checkDirectory = new CheckDirectory();
            foreach (var d in directories)
            {
                _logger.Info($"Processing dir {d}");
                try
                {
                    _checkDirectory.ProcessDirectory(d);
                }
                catch (Exception e)
                {
                    _logger.Info(e);
                }
                _logger.Info($"{_checkDirectory.Count} total files processed.");
            }
            _logger.Info($"Files used: {_checkDirectory.Count}");

            _task = new Task(new Action(MainLoop));
            _task.Start();
        }

        public void OnStop()
        {
            _running = false;
           // Thread.Sleep(_delay);
            _logger.Info("Exiting On Stop");
        }
    }
}
