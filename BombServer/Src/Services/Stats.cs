﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Services
{
    class Stats
    {
        public BombService Service { get; }

        public Stats(string ip, ushort port)
        {
            Service = new BombService("stats", EProtocolType.RUDP, true, ip, port);
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterDirectConnect(DirectConnectHandler, EEndianness.Big);
        }

        void DirectConnectHandler(IClient client, EndiannessAwareBinaryReader br, EndiannessAwareBinaryWriter bw)
        {
            bw.Write(new byte[0xFF]);
            client.SendUnreliableGameData(bw);
        }
    }
}
