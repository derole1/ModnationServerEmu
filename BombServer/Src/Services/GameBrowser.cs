﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Log;

namespace BombServerEmu_MNR.Src.Services
{
    class GameBrowser
    {
        public BombService Service { get; }

        public GameBrowser(string ip, ushort port)
        {
            Service = new BombService("gamebrowser", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterMethod("subscribeGameEvents", null);
            Service.RegisterMethod("unSubscribeGameEvents", UnSubscribeGameEventsHandler);
            Service.RegisterMethod("listGames", ListGamesHandler);
            Service.RegisterMethod("listFakeGames", null);
        }

        void ListGamesHandler(BombService service, SSLClient client, BombXml xml)
        {
            var attributes = new BombAttributeList(Convert.FromBase64String(xml.GetParam("attributes")));
            Logging.Log(typeof(GameBrowser), "{0}", LogType.Debug, attributes);

            //This response is 120% correct, investigate the matchmaking config, thats likely why the game wont create a game
            xml.SetMethod("listGames");
            var gameList = new BombGameList();
            gameList.Add(new BombGame
            {
                GameName = "test_game",
                Unk1 = "TEST"
            });
            xml.AddParam("serverGameListHeader", Convert.ToBase64String(gameList.SerializeHeader()));
            xml.AddParam("serverGameList", Convert.ToBase64String(gameList.SerializeList()));
            xml.AddParam("gameListTimeOfDeath", Math.Floor((DateTime.UtcNow.AddHours(1) - new DateTime(1970, 1, 1)).TotalSeconds));
            client.SendXmlData(xml);
        }

        void UnSubscribeGameEventsHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("unSubscribeGameEvents");
            //TODO: Unsubscribe from game events
            client.SendXmlData(xml);
        }
    }
}
