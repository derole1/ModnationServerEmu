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
        }

        public void SetCert(string certPath, string certPass) => Logging.Log(typeof(RUDP), "Cannot set cert for RUDP protocol!", LogType.Warning);

        public void Start() { }

        public IClient GetClient()
        {
            var ipEp = new IPEndPoint(IPAddress.None, 0);
            while (true)
            {
                var data = Listener.Receive(ref ipEp);
                if ((EBombPacketType)data[0] == EBombPacketType.Sync)
                {
                    Listener.Connect(ipEp); //UDP is connectionless, all this is doing is telling the .net library where to route responses to
                    Logging.Log(typeof(RUDP), "Connection from {0}:{1}", LogType.Info, ipEp.Address, ipEp.Port);
                    return new RUDPClient(Service, Listener);
                }
            }
        }
    }
}
