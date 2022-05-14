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
    class Announcements
    {
        public static void AnnouncementsHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            var urls = doc.CreateElement(root, "announcements", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("total", "1"),
            });
            {
                doc.CreateElement(urls, "announcement", "In ModSpot you will be forced offline, as matchmaking is currently not implemented.", new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("id", "1"),
                    new KeyValuePair<string, string>("subject", "Matchmaking Unimplemented"),
                    new KeyValuePair<string, string>("language_code", "en-us"),
                    new KeyValuePair<string, string>("created_at", "2022-05-14T21:17:49+00:00"),
                });
            }
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
