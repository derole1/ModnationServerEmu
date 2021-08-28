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
    class GameBrowser
    {
        public BombService service;

        public GameBrowser(string ip, ushort port)
        {
            service = new BombService("gamebrowser", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("subscribeGameEvents", null);
            service.RegisterMethod("unSubscribeGameEvents", UnSubscribeGameEventsHandler);
            service.RegisterMethod("listGames", null);
            service.RegisterMethod("listFakeGames", null);
        }

        void UnSubscribeGameEventsHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("unSubscribeGameEvents");
            client.SendXmlData(xml);
        }
    }
}
