using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

using HttpLib;

using ModnationServer.Src.Protocols;
using ModnationServer.Src.Helpers;
using ModnationServer.Src.Log;

namespace ModnationServer.Src.Services.Handlers
{
    class Resources
    {
        const string RESOURCE_PATH = @"Data\Resources\";

        public static void GetResourceHandler(TcpClient client, HttpApi.ModnationRequest req, HttpResponse res)
        {
            //TODO: Filter characters in uri, dont trust the user!
            var uri = req.Request.Uri.Split('/', '\\');
            res.Data = File.ReadAllBytes(RESOURCE_PATH + uri.Last());
            res.Headers["Content-Type"] = "text/xml";
        }
    }
}
