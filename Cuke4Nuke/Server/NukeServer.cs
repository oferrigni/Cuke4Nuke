﻿using System.IO;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class NukeServer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Listener _listener;
        readonly Options _options;

        public NukeServer(Listener listener, Options options)
        {
            _listener = listener;
            _options = options;
        }

        public void Start()
        {
            if (_options.ShowHelp)
            {
                ShowHelp();
            }
            else
            {
                Run();
            }
        }

        void Run()
        {
            _listener.MessageLogged += listener_LogMessage;
            _listener.Start();
            _listener.Stop();
        }

        void ShowHelp()
        {
            Log("Usage: Cuke4Nuke.Server.exe [OPTIONS]");
            Log("Start the Cuke4Nuke server to invoke .NET Cucumber step definitions.");
            Log("");
            Log(_options.ToString());
        }

        void listener_LogMessage(object sender, LogEventArgs e)
        {
            Log(e.Message);
        }

        void Log(string message)
        {
            log.Info(message);
        }
    }
}