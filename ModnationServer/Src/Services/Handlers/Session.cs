using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using HttpLib;

using ModnationServer.Src.Protocols;
using ModnationServer.Src.Helpers;
using ModnationServer.Src.Log;

namespace ModnationServer.Src.Services.Handlers
{
    class Session
    {
        public static void SessionLoginHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            doc.CreateElement(root, "login_data", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("player_id", "2059179"),
                new KeyValuePair<string, string>("player_name", "Jonopiel"),
                new KeyValuePair<string, string>("presence", "ONLINE"),
                new KeyValuePair<string, string>("platform", "PS3"),

                new KeyValuePair<string, string>("login_time", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss+00:00")),
                new KeyValuePair<string, string>("ip_address", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()),
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
            //TEMP
            if (res.Headers.ContainsKey("Cookie"))
                res.Headers.Remove("Cookie");
            res.Headers.Add("Set-Cookie", "playerconnect_session_id=BAh7EzoPc2Vzc2lvbl9pZCIlMGVlZjEyNDk0NmJiOThmM2ZlYmQzODM0NmUwYTc3MDU6C2RvbWFpbiIAOhJsYW5ndWFnZV9jb2RlIgplbi11czoQcmVnaW9uX2NvZGUiCXNjZWE6DXRpbWV6b25lIgktMzAwOg1wbGF0Zm9ybSIIUFMzOg1wcmVzZW5jZUkiC09OTElORQY6DWVuY29kaW5nIg1VUy1BU0NJSUkiCmZsYXNoBjsMQA1JQzonQWN0aW9uQ29udHJvbGxlcjo6Rmxhc2g6OkZsYXNoSGFzaHsGOgtub3RpY2VJIiVQcmVmZXJlbmNlcyBzdWNlc3NmdWxseSB1cGRhdGVkIQY7DEANBjoKQHVzZWR7BjsORjoPaXBfYWRkcmVzcyISNzMuMjE1LjE4NS42NDoOcGxheWVyX2lkaQO3JB86DXVzZXJuYW1lIg1zaGFuejJuZDoPY29uc29sZV9pZCIYMDA6MzY6MTQxOjE4MToxMzo1OToVdmlld2luZ19wbGF0Zm9ybSIIUFMzOhZsYXN0X3JlcXVlc3RfbWFkZWwrB3yiqFs%3D--9d686236e0bc9f78ebc46df5cf9be36797c258cf; path=/");
        }

        public static void SessionPingHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");

            doc.SetResult(0);
            res.Data = doc.Serialize();
        }

        public static void SessionSetPresenceHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            //TODO
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }

        public static void SessionLogoutHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            //TODO
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
