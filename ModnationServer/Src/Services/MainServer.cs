using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

using ModnationServer.Src.Log;
using ModnationServer.Src.Protocols;
using ModnationServer.Src.Services.Handlers;
using ModnationServer.Src.Helpers;

namespace ModnationServer.Src.Services
{
    class MainServer
    {
        public static string conStr = "";

        public HttpApi Api { get; }

        public MainServer(string ip, ushort port, string dbFile, string schemaFile)
        {
            conStr = string.Format("Data Source={0};Version=3;", dbFile);
            Database.CheckDB(conStr, schemaFile);
            Api = new HttpApi("PlayerConnect", ip, port);
            //Resources
            Api.RegisterMethod("/resources/preferences.update.xml", null);
            //Methods
            Api.RegisterMethod("/preferences.xml", Preferences.PreferencesHandler);
        }
    }
}
