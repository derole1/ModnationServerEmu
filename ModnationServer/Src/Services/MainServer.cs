using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

using ModnationServer.Src.Log;
using ModnationServer.Src.Helpers;

namespace ModnationServer.Src.Services
{
    class MainServer
    {
        public static string conStr = "";

        HttpListener listener = new HttpListener();

        public MainServer(string domain, string dbFile, string schemaFile)
        {
            try
            {
                conStr = string.Format("Data Source={0};Version=3;", dbFile);
                Database.CheckDB(conStr, schemaFile);
                listener.Prefixes.Add(domain);
                listener.Start();
                Logging.Log(typeof(MainServer), "Started MainServer on {0}", LogType.Info, domain);
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    new Thread(() => ProcessContext(context)).Start();
                }
            }
            catch (Exception e)
            {
                Logging.Log(typeof(MainServer), e.ToString(), LogType.Error);
                Debugger.Break();
            }
        }

        void ProcessContext(HttpListenerContext context)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Logging.Log(typeof(MainServer), e.ToString(), LogType.Error);
                context.Response.Close();
                //Debugger.Break();
            }
        }
    }
}
