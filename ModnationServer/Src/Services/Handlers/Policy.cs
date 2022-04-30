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
    class Policy
    {
        public static void PolicyViewHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            doc.CreateElement(root, "policy", "Modnation server rewrite!", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("id", "1"),
                new KeyValuePair<string, string>("is_accepted", "false"),
                new KeyValuePair<string, string>("name", "Online User Agreement"),
            });
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }

        public static void PolicyAcceptHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            //TODO: Record that the user accepted the policy
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
