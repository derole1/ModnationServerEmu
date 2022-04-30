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
            var mainServer = new MainServer("127.0.0.1", 10050, "Db\\MainServer.sqlite", "Db\\Schema\\MainServer.sql");
            //new Thread(() => new CDNServer("http://*:10050/cdn/", "Db\\Content\\")).Start();  //TODO
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
