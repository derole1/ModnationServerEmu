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
    class RUDP
    {
        BombService service;
        UdpClient listener;

        public RUDP(BombService service, string ip, ushort port)
        {
            listener = new UdpClient(ip, port);
            Logging.Log(typeof(RUDP), "Unimplemented!", LogType.Error);
        }

        public void Start()
        {
            
        }

        public RUDPClient GetClient()
        {
            //var ipEp = (IPEndPoint)client.Client.RemoteEndPoint;
            //Logging.Log(typeof(SSL), "Connection from {0}:{1}", LogType.Info, ipEp.Address, ipEp.Port);
            return new RUDPClient(service, listener);
        }
    }
}
