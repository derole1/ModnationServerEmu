using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;

namespace BombServerEmu_MNR.Src.Protocols
{
    class SSL
    {
        X509Certificate2 cert;

        BombService service;
        TcpListener listener;

        public SSL(BombService service, string ip, ushort port)
        {
            this.service = service;
            listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void SetCert(string certPath, string certPass)
        {
            cert = new X509Certificate2(certPath, certPass);
            Logging.Log(typeof(SSL), "Updated cert for {0}! Using {1}", LogType.Debug, service.name, certPath);
        }

        public void Start()
        {
            listener.Start();
        }

        public SSLClient GetClient()
        {
            var client = listener.AcceptTcpClient();
            var ipEp = (IPEndPoint)client.Client.RemoteEndPoint;
            Logging.Log(typeof(SSL), "Connection from {0}:{1}", LogType.Info, ipEp.Address, ipEp.Port);
            return new SSLClient(service, client, cert);
        }
    }
}
