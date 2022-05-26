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
        public BombService Service;

        public PlayGroup(string ip, ushort port)
        {
            Service = new BombService("playgroup", EProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterMethod("createPlaygroup", null);
            Service.RegisterMethod("joinPlaygroupById", null);
            Service.RegisterMethod("inviteUser", null);
            Service.RegisterMethod("textMessage", null);
            Service.RegisterMethod("notifyGroup", null);
            Service.RegisterMethod("requestGroupJoinGame", null);
            Service.RegisterMethod("requestGroupLeaveGame", null);
            Service.RegisterMethod("leavePlaygroup", null);
        }
    }
}
