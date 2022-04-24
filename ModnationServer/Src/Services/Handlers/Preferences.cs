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
        public static void PreferencesHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            res.Data = Encoding.ASCII.GetBytes(((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
        }
    }
}
