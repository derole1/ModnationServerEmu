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
    class Server
    {
        public static void ServerSelectHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            var server = doc.CreateElement(root, "server", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("server_type", "DIRECTORY"),
                new KeyValuePair<string, string>("address", "192.168.1.196"),
                new KeyValuePair<string, string>("port", "10501"),
                new KeyValuePair<string, string>("session_uuid", "9aa555a8-cc1a-11e8-81c9-22000acbd9b1"),
                new KeyValuePair<string, string>("server_private_key", "MIGrAgEAAiEAq0cOe8L1tOpnc7e+ouVD"),
            });
            {
                doc.CreateElement(server, "ticket", null, new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("session_uuid", "9aa555a8-cc1a-11e8-81c9-22000acbd9b1"),
                    new KeyValuePair<string, string>("player_id", "2059179"),
                    new KeyValuePair<string, string>("username", "Jonopiel"),
                    new KeyValuePair<string, string>("expiration_date", "Tue Oct 09 23:25:57 +0000 2022"),
                    new KeyValuePair<string, string>("signature", "98b93493e8beb1318533fb87897f1e80"),
                });
            }
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
