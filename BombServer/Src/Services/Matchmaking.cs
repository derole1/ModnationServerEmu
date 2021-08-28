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
        public BombService service;

        public Matchmaking(string ip, ushort port)
        {
            service = new BombService("matchmaking", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("beginMatchmaking", null);
            service.RegisterMethod("cancelMatchmaking", null);
        }
    }
}
