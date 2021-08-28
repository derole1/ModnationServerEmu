using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

using ModnationServer.Src.Log;

namespace ModnationServer.Src.Services
{
    class CDNServer
    {
        static string rootDir = "";

        HttpListener listener = new HttpListener();
        Dictionary<string, string> contentTypeTable = new Dictionary<string, string>()
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".mp4", "video/mp4" },
            { ".html", "text/html" },
            { ".txt", "text/txt" },
            { ".css", "text/css" },
            { ".ico", "image/x-icon" }
        };

        public CDNServer(string domain, string rootDir)
        {
            try
            {
                CDNServer.rootDir = rootDir;
                CheckDirectories();
                listener.Prefixes.Add(domain);
                listener.Start();
                Logging.Log(typeof(CDNServer), "Started CDNServer on {0}", LogType.Info, domain);
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    new Thread(() => ProcessContext(context)).Start();
                }
            }
            catch (Exception e)
            {
                Logging.Log(typeof(CDNServer), e.ToString(), LogType.Error);
                Debugger.Break();
            }
        }

        void CheckDirectories()
        {
            string dir = string.Format("{0}{1}", rootDir, "player_avatars");
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            dir = string.Format("{0}{1}", rootDir, "player_creations");
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
        }

        public static byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(string.Format("{0}{1}", rootDir, path));
        }

        public static void WriteFile(string path, byte[] data)
        {
            File.WriteAllBytes(string.Format("{0}{1}", rootDir, path), data);
        }

        void ProcessContext(HttpListenerContext context)
        {
            try
            {
                string url = context.Request.RawUrl;
                int apiIndex = url.IndexOf("/cdn") + 4;
                string file = url.Substring(apiIndex, url.Length - apiIndex);
                if (file.Length > 0)
                {
                    if (file[0] == '/') { file = file.Substring(1, file.Length - 1); }
                    Logging.Log(typeof(CDNServer), "Request for {0} from ip {1}", LogType.Info, file, context.Request.RemoteEndPoint.Address);
                    file = rootDir + file;
                    if (File.Exists(file))
                    {
                        byte[] resData = File.ReadAllBytes(file);
                        string ext = Path.GetExtension(file);
                        WriteData(context, 200
                            , contentTypeTable.ContainsKey(ext) ? contentTypeTable[ext] : "application/octet-stream"
                            , resData);
                        return;
                    }
                }
                WriteData(context, 404);
            }
            catch (Exception e)
            {
                Logging.Log(typeof(CDNServer), e.ToString(), LogType.Error);
                context.Response.Close();
                //Debugger.Break();
            }
        }

        void WriteData(HttpListenerContext context, int statusCode, string contentType = null, byte[] data = null)
        {
            context.Response.StatusCode = statusCode;
            if (data != null)
            {
                context.Response.ContentType = contentType;
                context.Response.ContentLength64 = data.Length;
                context.Response.OutputStream.Write(data, 0, data.Length);
            }
            context.Response.Close();
        }
    }
}
