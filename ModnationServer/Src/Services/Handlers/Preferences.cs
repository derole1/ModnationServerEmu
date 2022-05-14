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
using ModnationServer.Src.DataTypes;
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
                new KeyValuePair<string, string>("timezone", param["preference[timezone]"]),
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
            //TEMP
            if (res.Headers.ContainsKey("Cookie"))
                res.Headers.Remove("Cookie");
            res.Headers.Add("Set-Cookie", $"playerconnect_session_id={new PlayerConnectTicket(req.Session).Serialize()}; path=/");
        }
    }
}
