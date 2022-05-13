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
            service.RegisterMethod("listGames", ListGamesHandler);
            service.RegisterMethod("listFakeGames", null);
        }

        void ListGamesHandler(BombService service, SSLClient client, BombXml xml)
        {
            //var attributes = new BombAttributeList(Convert.FromBase64String(xml.GetParam("attributes")));

            //This response is 120% correct, investigate the matchmaking config, thats likely why the game wont create a game
            xml.SetMethod("listGames");
            //xml.SetError("noGamesAvailable");
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write(0.SwapBytes());    //GameCount im pretty sure

            bw.Write(0.SwapBytes());    //Some ID? int32 for sure
            bw.Write(2.SwapBytes());
            bw.Write(Encoding.ASCII.GetBytes("HI"));
            bw.Write(2.SwapBytes());
            bw.Write(Encoding.ASCII.GetBytes("HI"));
            bw.Write(2.SwapBytes());
            bw.Write(Encoding.ASCII.GetBytes("HI"));

            bw.Write(new byte[255]);
            xml.AddParam("serverGameListHeader", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray()));
            //var gameList = new BombAttributeList();
            bw = new BinaryWriter(new MemoryStream());
            bw.Write(1.SwapBytes());    //numFriends (This might actually be a count? Pointer to best game?

            bw.Write(0.SwapBytes());

            bw.Write(2.SwapBytes());
            bw.Write(Encoding.ASCII.GetBytes("HI"));    //Is this a string_member???? It doesnt throw string_member error if its invalid

            bw.Write(0.SwapBytes());    //Count for something

            //bw.Write(2.SwapBytes());
            //bw.Write(Encoding.ASCII.GetBytes("HI"));

            //End of structure for above count

            bw.Write(9.SwapBytes());    //GameName
            bw.Write(Encoding.ASCII.GetBytes("test_game"));

            bw.Write(new byte[255]);
            xml.AddParam("serverGameList", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray())); //This uses BombAttributeList, but maybe with slightly different markers? (Im now doubting this...)
            xml.AddParam("gameListTimeOfDeath", Math.Floor((DateTime.UtcNow.AddHours(1) - new DateTime(1970, 1, 1)).TotalSeconds));
            client.SendXmlData(xml);
        }

        void UnSubscribeGameEventsHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("unSubscribeGameEvents");
            client.SendXmlData(xml);
        }
    }
}
