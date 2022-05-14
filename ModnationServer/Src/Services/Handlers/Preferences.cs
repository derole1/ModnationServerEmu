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
    class Preferences
    {
        public static void PreferencesHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var param = HttpApi.DecodeUriParameters(req.Request.Data, int.Parse(req.Request.GetHeader("Content-Length")), Encoding.UTF8);

            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            doc.CreateElement(root, "preference", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("domain", param["preference[domain]"]),
                new KeyValuePair<string, string>("ip_address", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()),
                new KeyValuePair<string, string>("language_code", param["preference[language_code]"]),
                new KeyValuePair<string, string>("region_code", param["preference[region_code]"]),
                new KeyValuePair<string, string>("timezone", param["timezone"]),
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
            //TEMP
            if (res.Headers.ContainsKey("Cookie"))
                res.Headers.Remove("Cookie");
            res.Headers.Add("Set-Cookie", "playerconnect_session_id=BAh7DToPc2Vzc2lvbl9pZCIlMGVlZjEyNDk0NmJiOThmM2ZlYmQzODM0NmUwYTc3MDU6C2RvbWFpbiIAOhJsYW5ndWFnZV9jb2RlIgplbi11czoQcmVnaW9uX2NvZGUiCXNjZWE6DXRpbWV6b25lIgktMzAwOg1wbGF0Zm9ybSIIUFMzOg1wcmVzZW5jZUkiC09OTElORQY6DWVuY29kaW5nIg1VUy1BU0NJSUkiCmZsYXNoBjsMQA1JQzonQWN0aW9uQ29udHJvbGxlcjo6Rmxhc2g6OkZsYXNoSGFzaHsGOgtub3RpY2VJIiVQcmVmZXJlbmNlcyBzdWNlc3NmdWxseSB1cGRhdGVkIQY7DEANBjoKQHVzZWR7BjsORg%3D%3D--4f22b4223b072ed5c7a47bd909a907d16f3e7a7c; path=/");
        }
    }
}
