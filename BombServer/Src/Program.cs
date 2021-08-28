using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Services;

namespace BombServerEmu_MNR.Src
{
    class Program
    {
        public static List<BombService> services = new List<BombService>();

        static void Main(string[] args)
        {
            Logging.OpenLogFile();
            Logging.RealLog(typeof(Program), "BombServer  Copyright (C) 2021  derole\n" +
                "This program comes with ABSOLUTELY NO WARRANTY! This is free software, and you are welcome to redistribute it under certain conditions\n", LogType.Info);
            CheckArgs(args);
            //SetCipherSuite();
            if (!CheckCerts()) {
                Logging.Log(typeof(Program), "Failed to find a certificate in the Certs folder!", LogType.Error);
                Console.ReadKey();
                return;
            }
            services.Add(new Directory("192.168.1.46", 10501).service);

            services.Add(new Matchmaking("192.168.1.46", 10510).service);
            services.Add(new GameManager("192.168.1.46", 10511).service);
            services.Add(new GameBrowser("192.168.1.46", 10512).service);

            services.Add(new TextComm("192.168.1.46", 10513).service);
            services.Add(new PlayGroup("192.168.1.46", 10514).service);
            services.Add(new Stats("192.168.1.46", 10515).service);
        }

        static bool CheckCerts()
        {
            try
            {
                if (!System.IO.File.Exists("Certs\\output.pfx"))
                {
                    var proc = Process.Start("Scripts\\GenCert.bat");
                    proc.WaitForExit();
                    if (System.IO.File.Exists("C:\\Program Files\\OpenSSL-Win64\\bin\\certs\\output.pfx"))
                    {
                        System.IO.File.Move("C:\\Program Files\\OpenSSL-Win64\\bin\\certs\\output.pfx", "Certs\\output.pfx");
                        System.IO.Directory.Delete("C:\\Program Files\\OpenSSL-Win64\\bin\\certs", true);
                        return true;
                    }
                    return false;
                }
                return true;
            } catch { return false; }
        }

        static void SetCipherSuite()
        {
            var proc = Process.Start("PowerShell", string.Format("\"{0}\"", System.IO.Path.GetFullPath("Scripts\\SetCipherSuite.ps1")));
            proc.WaitForExit();
        }

        static void CheckArgs(string[] args)
        {
            for (int i=0; i<args.Length; i++) {
                switch (args[i]) {
                    case "-loglevel":
                        Logging.logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), args[i + 1]);
                        break;
                }
            }
        }
    }
}
