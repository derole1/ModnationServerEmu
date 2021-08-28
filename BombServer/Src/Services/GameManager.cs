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
    class GameManager
    {
        public BombService service;

        public GameManager(string ip, ushort port)
        {
            service = new BombService("gamemanager", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("logClientMessage", null);
            service.RegisterMethod("registerSessionKeyWithTargetBombd", null);
            service.RegisterMethod("createGame", null);
            service.RegisterMethod("joinEmptyGame", null);
            service.RegisterMethod("leaveGame", null);
            service.RegisterMethod("leaveCurrentGame", LeaveCurrentGameHandler);
            service.RegisterMethod("reserveGame", null);
            service.RegisterMethod("reserveGameSlotsForPlayers", null);
            service.RegisterMethod("dropReservedGame", null);
            service.RegisterMethod("migrateToGame", null);
            service.RegisterMethod("requestDirectHostConnection", null);    //???
            service.RegisterMethod("directConnectionStatus", null);
            service.RegisterMethod("publishAttributes", null);
            service.RegisterMethod("kickPlayer", null);
        }

        void LeaveCurrentGameHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("leaveCurrentGame");
            client.SendXmlData(xml);
        }
    }
}
