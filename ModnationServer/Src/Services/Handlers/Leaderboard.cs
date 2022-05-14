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
    class Leaderboard
    {
        public static void LeaderboardViewHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            doc.CreateElement(root, "my_stats");
            doc.CreateElement(root, "leaderboard", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("total", "0"),
                new KeyValuePair<string, string>("row_start", "0"),
                new KeyValuePair<string, string>("row_end", "0"),
                new KeyValuePair<string, string>("page", "0"),
                new KeyValuePair<string, string>("total_pages", "0"),
                new KeyValuePair<string, string>("type", req.Request.UriParameters["type"]),
                new KeyValuePair<string, string>("game_type", req.Request.UriParameters["game_type"])
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
