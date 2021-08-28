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
    class PlayGroup
    {
        public BombService service;

        public PlayGroup(string ip, ushort port)
        {
            service = new BombService("playgroup", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("createPlaygroup", null);
            service.RegisterMethod("joinPlaygroupById", null);
            service.RegisterMethod("inviteUser", null);
            service.RegisterMethod("textMessage", null);
            service.RegisterMethod("notifyGroup", null);
            service.RegisterMethod("requestGroupJoinGame", null);
            service.RegisterMethod("requestGroupLeaveGame", null);
            service.RegisterMethod("leavePlaygroup", null);
        }
    }
}
