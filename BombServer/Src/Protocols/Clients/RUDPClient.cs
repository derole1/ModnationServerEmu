using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;

namespace BombServerEmu_MNR.Src.Protocols.Clients
{
    class RUDPClient
    {
        BombService service;
        UdpClient client;

        public RUDPClient(BombService service, UdpClient listener)
        {
            this.service = service;
            this.client = listener;
            Logging.Log(typeof(RUDPClient), "Unimplemented!", LogType.Error);
        }
    }
}
