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
    class SSL : IProtocol
    {
        public X509Certificate2 Cert { get; private set; }

        public BombService Service { get; }
        public TcpListener Listener { get; }

        bool UseSsl { get; }

        public SSL(BombService service, string ip, ushort port, bool useSsl = true)
        {
            Service = service;
            UseSsl = useSsl;
            Listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void SetCert(string certPath, string certPass)
        {
            Cert = new X509Certificate2(certPath, certPass);
            Logging.Log(typeof(SSL), "Updated cert for {0}! Using {1}", LogType.Debug, Service.Name, certPath);
        }

        public void Start()
        {
            Listener.Start();
        }

        public IClient GetClient()
        {
            var client = Listener.AcceptTcpClient();
            var ipEp = (IPEndPoint)client.Client.RemoteEndPoint;
            Logging.Log(typeof(SSL), "Connection from {0}:{1}", LogType.Info, ipEp.Address, ipEp.Port);
            return new SSLClient(Service, client, UseSsl ? Cert : null);
        }
    }
}
