using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Services;
using BombServerEmu_MNR.Src.Helpers;

namespace BombServerEmu_MNR.Src
{
    class Program
    {
        public static List<BombService> Services { get; } = new List<BombService>();

        public static string ClusterUuid { get; } = UUID.GenerateUUID();

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
            Services.Add(new Directory("192.168.1.196", 10501).Service);

            Services.Add(new Matchmaking("192.168.1.196", 10510).Service);  //Made up port
            Services.Add(new GameManager("192.168.1.196", 10505).Service);
            Services.Add(new GameBrowser("192.168.1.196", 10412).Service);

            Services.Add(new TextComm("192.168.1.196", 10513).Service);  //Made up port
            Services.Add(new PlayGroup("192.168.1.196", 10514).Service);  //Made up port
            Services.Add(new Stats("192.168.1.196", 50002).Service);

            // TEST
            new GameServer("192.168.1.196", 1234);

            //Services.Add(new Directory("192.168.1.196", 11501).Service);

            //Services.Add(new Matchmaking("192.168.1.196", 11510).Service);
            //Services.Add(new GameManager("192.168.1.196", 11511).Service);
            //Services.Add(new GameBrowser("192.168.1.196", 11512).Service);

            //Services.Add(new TextComm("192.168.1.196", 11513).Service);
            //Services.Add(new PlayGroup("192.168.1.196", 11514).Service);
            //Services.Add(new Stats("192.168.1.196", 11515).Service);
        }

        static bool CheckCerts()
        {
            try
            {
                Logging.Log(typeof(Program), System.IO.Path.GetFullPath(@"Certs\output.pfx"), LogType.Debug);
                if (!System.IO.File.Exists(@"Data\Certs\output.pfx"))
                {
                    var proc = Process.Start(@"Data\Scripts\GenCert.bat");
                    proc.WaitForExit();
                    if (System.IO.File.Exists(@"C:\Program Files\OpenSSL-Win64\bin\certs\output.pfx"))
                    {
                        System.IO.File.Move(@"C:\Program Files\OpenSSL-Win64\bin\certs\output.pfx", @"Data\Certs\output.pfx");
                        System.IO.Directory.Delete(@"C:\Program Files\OpenSSL-Win64\bin\certs", true);
                        return true;
                    }
                    return false;
                }
                return true;
            } catch { return false; }
        }

        static void SetCipherSuite()
        {
            var proc = Process.Start("PowerShell", string.Format("\"{0}\"", System.IO.Path.GetFullPath(@"Data\Scripts\SetCipherSuite.ps1")));
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
