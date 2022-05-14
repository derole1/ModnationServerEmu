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
using ModnationServer.Src.Sessions;
using ModnationServer.Src.Log;

namespace ModnationServer.Src.Services.Handlers
{
    class Session
    {
        public static void SessionLoginHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var param = HttpApi.DecodeUriParameters(req.Request.Data, int.Parse(req.Request.GetHeader("Content-Length")), Encoding.UTF8);
            var ticket = new NPTicket(param["ticket"]);
            Logging.Log(typeof(Session), "{0}", LogType.Debug, ticket);

            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            doc.CreateElement(root, "login_data", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("player_id", "2059179"),   //TODO: Get playerId from db
                new KeyValuePair<string, string>("player_name", ticket.OnlineId),   //TODO: Get playerName from db
                new KeyValuePair<string, string>("presence", "ONLINE"),
                new KeyValuePair<string, string>("platform", param["platform"]),

                new KeyValuePair<string, string>("login_time", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss+00:00")),
                new KeyValuePair<string, string>("ip_address", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()),
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
            //TEMP
            if (res.Headers.ContainsKey("Cookie"))
                res.Headers.Remove("Cookie");
            res.Headers.Add("Set-Cookie", $"playerconnect_session_id={new PlayerConnectTicket(req.Session).Serialize()}; path=/");
        }

        public static void SessionPingHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");

            doc.SetResult(0);
            res.Data = doc.Serialize();
        }

        public static void SessionSetPresenceHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            //TODO
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }

        public static void SessionLogoutHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            //TODO
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
