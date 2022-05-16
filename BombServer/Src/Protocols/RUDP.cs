using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using BombServerEmu_MNR.Src.DataTypes;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.Protocols.Clients;

namespace BombServerEmu_MNR.Src.Protocols
{
    class RUDP : IProtocol
    {
        public BombService Service { get; }
        public UdpClient Listener { get; }

        public RUDP(BombService service, string ip, ushort port)
        {
            Listener = new UdpClient(ip, port);
            Logging.Log(typeof(RUDP), "Unimplemented!", LogType.Error);
        }

        public void SetCert(string certPath, string certPass) => Logging.Log(typeof(RUDP), "Cannot set cert for RUDP protocol!", LogType.Warning);

        public void Start()
        {
            Logging.Log(typeof(RUDP), "Start: Unimplemented!", LogType.Error);
        }

        public IClient GetClient()
        {
            //Infinitely block for now
            while (true)
                System.Threading.Thread.Sleep(1000);
            //var ipEp = (IPEndPoint)client.Client.RemoteEndPoint;
            //Logging.Log(typeof(SSL), "Connection from {0}:{1}", LogType.Info, ipEp.Address, ipEp.Port);
            return new RUDPClient(Service, Listener);
        }
    }
}
