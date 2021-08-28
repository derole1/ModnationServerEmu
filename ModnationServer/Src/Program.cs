using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ModnationServer.Src.Log;
using ModnationServer.Src.Services;

namespace ModnationServer.Src
{
    class Program
    {
        static void Main(string[] args)
        {
            Logging.OpenLogFile();
            Logging.RealLog(typeof(Program), "ModnationServer  Copyright (C) 2021  derole\n" +
                "This program comes with ABSOLUTELY NO WARRANTY! This is free software, and you are welcome to redistribute it under certain conditions\n", LogType.Info);
            CheckArgs(args);
            new Thread(() => new MainServer("http://*/", "Db\\MainServer.sqlite", "Db\\Schema\\MainServer.sql")).Start();
            new Thread(() => new CDNServer("http://*/cdn/", "Db\\Content\\")).Start();
        }

        static void CheckArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-loglevel":
                        Logging.logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), args[i + 1]);
                        break;
                }
            }
        }
    }
}
