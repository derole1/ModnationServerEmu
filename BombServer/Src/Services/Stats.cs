using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Services
{
    class Stats
    {
        public BombService service;

        public Stats(string ip, ushort port)
        {
            service = new BombService("stats", ProtocolType.RUDP, true, ip, port);
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterDirectConnect(DirectConnectHandler);
        }

        void DirectConnectHandler(SSLClient client, BinaryReader br, BinaryWriter bw)
        {
            bw.Write(new byte[0xFF]);
            client.SendRawData(bw);
        }
    }
}
