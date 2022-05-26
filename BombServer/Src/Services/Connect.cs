using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Protocols.Clients;
//TEMP
using BombServerEmu_MNR.Src.Helpers;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Services
{
    class Connect
    {
        public static void StartConnectHandler(BombService service, IClient client, BombXml xml)
        {
            var ticket = new NPTicket(xml.GetParam("NPTicket"));

            xml.SetName("connect");
            xml.SetMethod("startConnect");
            if (service.IsDirectConnect) {
                Logging.Log(typeof(Connect), "Service is directConnect! Requesting directConnection", LogType.Info);
                xml.AddParam("gameserver", "directConnection");
                //xml.AddParam("gameserver", "directGameServer");
            } else {
                xml.AddParam("bombd_version", "3.2.8");
                xml.AddParam("bombd_builddate", "3/29/2010 4:52:54 PM");
                xml.AddParam("bombd_revision", "1733");
                xml.AddParam("bombd_OS", "1");
                xml.AddParam("bombd_ServerIP", service.IP);
                xml.AddParam("bombd_ServerPort", service.Port);
                xml.AddParam("serveruuid", service.Uuid);
                xml.AddParam("username", ticket.OnlineId);
                xml.AddParam("userid", ticket.UserId);
                // In the packet logs, this is the service that gets a config (a fairly large one that is, likely >1024 bytes)
                // Its still unknown what type of config is sent, there seems to be multiple config types, the type is decided by the root node? (Need to verify how the XML works first)
                // The config in MMConfig.xml is a rough guess, it seems to be almost correct :D
                if (service.Name == "gamemanager")
                {
                    var config = File.ReadAllBytes(@"Data\Resources\MMConfig.xml");
                    xml.AddParam("MMConfigFile", Convert.ToBase64String(config));
                    xml.AddParam("MMConfigFileSize", config.Length);
                }
                //xml.AddParam("MMConfigFile", Convert.ToBase64String(new byte[1024]));
                //xml.AddParam("MMConfigFileSize", "1024");
            }
            client.SendNetcodeData(xml);
        }

        public static void TimeSyncRequestHandler(BombService service, IClient client, BombXml xml)
        {
            xml.SetName("connect");
            xml.SetMethod("timeSyncRequest");
            xml.AddParam("serverTime", Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
            client.HasDirectConnection = service.IsDirectConnect;
            client.SendNetcodeData(xml);
        }

        public static void TimeSyncRequestHandlerDEBUG(BombService service, IClient client, BombXml xml)
        {
            xml.SetName("connect");
            xml.SetMethod("timeSyncRequest");
            xml.AddParam("serverTime", Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
            client.HasDirectConnection = service.IsDirectConnect;
            client.SendNetcodeData(xml);
            //xml.SetName("gamemanager");
            //xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            //xml.SetMethod("joinGameCompleted");
            //xml.AddParam("gamename", "test");
            //xml.AddParam("gamebrowsername", "test");
            //xml.AddParam("gameid", "1");
            //xml.AddParam("numplayerslist", "1");
            //xml.AddParam("playerlist", Convert.ToBase64String(new byte[255]));
            //xml.AddParam("attributes", Convert.ToBase64String(new byte[255]));
            var ipEndPoint = (System.Net.IPEndPoint)client.RemoteEndPoint;
            //xml.SetName("gamemanager");
            //xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            //xml.SetMethod("updateP2PInfo");
            //xml.AddParam("playername", "Jonopiel");
            //xml.AddParam("p2pAddr", BinaryExtensions.SerializeIPAddress(ipEndPoint.Address));
            //xml.AddParam("p2pPort", "1234");
            //xml.AddParam("p2pAddrPrivate", BinaryExtensions.SerializeIPAddress(ipEndPoint.Address));
            //xml.AddParam("p2pPortPrivate", "1234");
            //client.SendNetcodeData(xml);
            //xml.SetName("gamemanager");
            //xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            //xml.SetMethod("joinGameCompleted");
            //xml.AddParam("gamename", "test_game");
            //xml.AddParam("gamebrowsername", "test_game");
            //xml.AddParam("gameid", "1");
            //xml.AddParam("numplayerslist", "1");
            //xml.AddParam("playerlist", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==");
            //var bw = new EndiannessAwareBinaryWriter(new MemoryStream(), EEndianness.Big);
            //bw.Write(1);
            //bw.WriteStringMember("HI");
            //bw.Write(new byte[255]);
            //bw.Flush();
            //xml.AddParam("attributes", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray()));
            //client.SendNetcodeData(xml);
            System.Threading.Thread.Sleep(5000);
            xml.SetName("gamemanager");
            xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            xml.SetMethod("requestDirectHostConnection");
            xml.AddParam("listenIP", "192.168.1.196");
            xml.AddParam("listenPort", "50002");
            xml.AddParam("hashSalt", "");
            xml.AddParam("sessionId", "1");
            client.SendNetcodeData(xml);
        }
    }
}
