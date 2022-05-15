using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

public enum LogType
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Debug = 3
}

public enum LogLevel
{
    None = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    Debug = 4
}

namespace ModnationServer.Src.Log
{
    class Logging
    {
        //public static LogLevel logLevel = LogLevel.Error;
        public static LogLevel logLevel = LogLevel.Debug;

        static FileStream logFile;

        public static void OpenLogFile()
        {
            try
            {
                if (!Directory.Exists("logs")) { Directory.CreateDirectory("logs"); }
                if (File.Exists("logs\\log.txt"))
                {
                    string dateTime = DateTime.Now.ToString("yyyyMMdd");
                    File.Copy("logs\\log.txt", string.Format("logs\\log_{0}{1}.txt", dateTime, Directory.GetFiles("logs", string.Format("log_{0}*.txt", dateTime)).Length.ToString().PadLeft(2, '0')));
                }
                logFile = File.Create("logs\\log.txt");
            }
            catch
            {
                //TODO: Retry after certain amount of time
                Log("Logging", "Error opening log file! Logs will NOT be saved for this session.", LogType.Error);
            }
        }

        static bool logThreadStarted = false;

        public struct LogEntry
        {
            public object module;
            public string message;
            public LogType type;
            public object[] param;
        }

        static List<LogEntry> logQueue = new List<LogEntry>();

        public static void Log(object module, string message, LogType type, params object[] param)
        {
            if ((int)logLevel > (int)type)
            {
                if (!logThreadStarted)
                {
                    new Thread(() => LogThread()).Start();
                    logThreadStarted = true;
                }
                log:
                try
                {
                    LogEntry log = new LogEntry();
                    log.module = module;
                    log.message = message;
                    log.type = type;
                    log.param = param;
                    logQueue.Add(log);
                }
                catch { goto log; }
            }
        }

        public static void RealLog(object module, string message, LogType type, params object[] param)
        {
            if ((int)logLevel > (int)type)
            {
                string[] moduleSplit = module.ToString().Split('.');
                foreach (string line in message.Split('\n'))
                {
                    switch (type)
                    {
                        case LogType.Info:
                            Write(ConsoleColor.Gray, "[{0}] I:{1}: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), moduleSplit[moduleSplit.Length - 1]);
                            break;
                        case LogType.Warning:
                            Write(ConsoleColor.Yellow, "[{0}] W:{1}: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), moduleSplit[moduleSplit.Length - 1]);
                            break;
                        case LogType.Error:
                            Write(ConsoleColor.Red, "[{0}] E:{1}: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), moduleSplit[moduleSplit.Length - 1]);
                            break;
                        case LogType.Debug:
                            Write(ConsoleColor.Magenta, "[{0}] D:{1}: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), moduleSplit[moduleSplit.Length - 1]);
                            break;
                    }
                    for (int i = 0; i < param.Length; i++)
                    {
                        var paramStr = param[i].ToString();
                        if (paramStr.Length > 4096)
                        {
                            param[i] = paramStr.Substring(0, 512) + " ... " + paramStr.Substring(paramStr.Length - 512, 512);
                        }
                    }
                    Write(ConsoleColor.White, line + "\r\n", param);
                }
            }
        }

        public static void LogThread()
        {
            while (true)
            {
                while (logQueue.Count > 0)
                {
                    try
                    {
                        LogEntry log = logQueue[0];
                        logQueue.RemoveAt(0);
                        RealLog(log.module, log.message, log.type, log.param);
                    }
                    catch { }
                }
                Thread.Sleep(10);
            }
        }

        static void Flush()
        {
            start:
            Thread.Sleep(10);
            try
            {
                logFile.Flush();
            }
            catch { goto start; }
        }

        static void Write(ConsoleColor color, string message, params object[] args)
        {
            Console.ForegroundColor = color;
            string msg = string.Format(message, args);
            Console.Write(msg);
            if (logFile != null && logFile.CanWrite)
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                logFile.Write(msgBytes, 0, msgBytes.Length);
                //new Thread(() => Flush()).Start();
                logFile.Flush();
            }
        }
    }
}
