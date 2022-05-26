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
        public BombService Service { get; }

        public GameManager(string ip, ushort port)
        {
            Service = new BombService("gamemanager", EProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandlerDEBUG);

            Service.RegisterMethod("logClientMessage", null);
            Service.RegisterMethod("registerSessionKeyWithTargetBombd", null);
            Service.RegisterMethod("createGame", null);
            Service.RegisterMethod("joinEmptyGame", null);
            Service.RegisterMethod("leaveGame", null);
            Service.RegisterMethod("leaveCurrentGame", LeaveCurrentGameHandler);
            Service.RegisterMethod("reserveGame", null);
            Service.RegisterMethod("reserveGameSlotsForPlayers", null);
            Service.RegisterMethod("dropReservedGame", null);
            Service.RegisterMethod("migrateToGame", null);
            Service.RegisterMethod("requestDirectHostConnection", null);    //???
            Service.RegisterMethod("directConnectionStatus", null);
            Service.RegisterMethod("publishAttributes", null);
            Service.RegisterMethod("kickPlayer", null);
        }

        void CreateGameHandler(BombService service, IClient client, BombXml xml)
        {
            //gamename,internalIP,externalIP,listenPort
            //xml.SetMethod("createGame");
            //client.SendNetcodeData(xml);
        }

        void LeaveCurrentGameHandler(BombService service, IClient client, BombXml xml)
        {
            xml.SetMethod("leaveCurrentGame");
            client.SendNetcodeData(xml);
        }
    }
}
