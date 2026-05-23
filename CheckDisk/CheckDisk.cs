using log4net;
using ServiceUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CheckDisk
{
    class CheckDisk : IService
    {
        private static readonly ILog _logger = LogManager.GetLogger("CheckDisk");
        private Task _task;
        private CheckDirectory _checkDirectory;
        private int _delay;
        private int _threshold;
        private volatile bool _running = true;
        private List<string> _directories;

        public bool ConsoleMode { get; set; }

        private void MainLoop()
        {
            while (_running)
            {
                _checkDirectory.ReadFile();
                Thread.Sleep(_delay);
            }
            _logger.Info("Exiting main loop");
        }

        private void InitConfig(string[] args)
        {
            _delay = 1000 * int.Parse(ConfigurationManager.AppSettings["PollDelay"]);
            _threshold = int.Parse(ConfigurationManager.AppSettings["Threshold"]);
            _directories = ConfigurationManager.AppSettings["Directories"].ToString().Split(',').ToList();
            _directories.AddRange(args);
            _logger.Info($"Console Mode: {ConsoleMode}");
            _logger.Info($"Nb directories: {_directories.Count}");
            _logger.Info($"Delay: {_delay} ms");
            _logger.Info($"Threshold: {_threshold} us");
        }

        public void OnStart(string[] args)
        {
            _logger.Info("=====================================================");
            InitConfig(args);

            _checkDirectory = new CheckDirectory(_threshold);
            foreach (var d in _directories)
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
