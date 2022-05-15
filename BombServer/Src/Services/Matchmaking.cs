using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Services
{
    class Matchmaking
    {
        public BombService Service { get; }

        public Matchmaking(string ip, ushort port)
        {
            Service = new BombService("matchmaking", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterMethod("beginMatchmaking", null);
            Service.RegisterMethod("cancelMatchmaking", null);
        }
    }
}
