using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
//TEMP
using BombServerEmu_MNR.Src.Helpers;

using BombServerEmu_MNR.Src.Helpers.Extensions;

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
            //var attributes = new BombAttributeList(Convert.FromBase64String(xml.GetParam("attributes")));

            //This response is 120% correct, investigate the matchmaking config, thats likely why the game wont create a game
            xml.SetMethod("listGames");
            //xml.SetError("noGamesAvailable");
            var bw = new EndiannessAwareBinaryWriter(new MemoryStream(), EEndianness.Big);
            bw.Write(2);    //GameCount im pretty sure

            bw.Write(1);    //Some ID? int32 for sure
            bw.WriteStringMember("test_gameA"); //How many of these actually exist?
            bw.WriteStringMember("test_gameA");
            bw.WriteStringMember("test_gameA");

            //I dont think theres anything after this

            xml.AddParam("serverGameListHeader", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray()));
            //var gameList = new BombAttributeList();
            bw = new EndiannessAwareBinaryWriter(new MemoryStream(), EEndianness.Big);
            bw.Write(2);    //numFriends

            bw.Write(0);    //PlayerID?
            bw.WriteStringMember("TST1");   //PlayerName?

            bw.Write(0);    //PlayerID?
            bw.WriteStringMember("TST2");   //PlayerName?

            //End of friend structure

            bw.Write(1);    //numGuests (max: 8)

            bw.WriteStringMember("TEST2");   //GuestName?

            //End of structure for above count

            bw.WriteStringMember("test_gameA");  //GameName

            bw.WriteStringMember("TEST3");

            bw.Write(1);//Unknown structure

            bw.WriteStringMember("TEST4");
            bw.WriteStringMember("TEST5");

            //End of structure for above count

            //End of first entry

            bw.Write(2);    //numFriends

            bw.Write(0);    //PlayerID?
            bw.WriteStringMember("TST1");   //PlayerName?

            bw.Write(0);    //PlayerID?
            bw.WriteStringMember("TST2");   //PlayerName?

            //End of friend structure

            bw.Write(1);    //numGuests (max: 8)

            bw.WriteStringMember("TEST2");   //GuestName?

            //End of structure for above count

            bw.WriteStringMember("test_gameB");  //GameName

            bw.WriteStringMember("TEST3");

            bw.Write(1);//Unknown structure

            bw.WriteStringMember("TEST4");
            bw.WriteStringMember("TEST5");

            //End of structure for above count

            //bw.Write(new byte[255]);
            xml.AddParam("serverGameList", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray())); //This uses BombAttributeList, but maybe with slightly different markers? (Im now doubting this...)
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
