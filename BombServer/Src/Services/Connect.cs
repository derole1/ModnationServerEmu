using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Protocols.Clients;

namespace BombServerEmu_MNR.Src.Services
{
    class Connect
    {
        public static void StartConnectHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetName("connect");
            xml.SetMethod("startConnect");
            if (service.isDirectConnect) {
                Logging.Log(typeof(Connect), "Service is directConnect! Requesting directConnection", LogType.Info);
                xml.AddParam("gameserver", "directConnection");
                //xml.AddParam("gameserver", "directGameServer");
            } else {
                xml.AddParam("bombd_version", "3.2.8");
                xml.AddParam("bombd_builddate", "3/29/2010 4:52:54 PM");
                xml.AddParam("bombd_revision", "1733");
                xml.AddParam("bombd_OS", "0");
                //xml.AddParam("bombd_ServerIP", service.ip);
                //xml.AddParam("bombd_ServerPort", service.port);
                xml.AddParam("serveruuid", "1");
                xml.AddParam("userid", "2059179");
                xml.AddParam("username", "Jonopiel");
            }
            client.SendXmlData(xml);
        }

        public static void TimeSyncRequestHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetName("connect");
            xml.SetMethod("timeSyncRequest");
            xml.AddParam("serverTime", Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
            client.hasDirectConnection = service.isDirectConnect;
            client.SendXmlData(xml);
        }

        public static void TimeSyncRequestHandlerDEBUG(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetName("connect");
            xml.SetMethod("timeSyncRequest");
            xml.AddParam("serverTime", Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
            client.hasDirectConnection = service.isDirectConnect;
            client.SendXmlData(xml);
            //xml.SetName("gamemanager");
            //xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            //xml.SetMethod("joinGameCompleted");
            //xml.AddParam("gamename", "test");
            //xml.AddParam("gamebrowsername", "test");
            //xml.AddParam("gameid", "1");
            //xml.AddParam("numplayerslist", "1");
            //xml.AddParam("playerlist", Convert.ToBase64String(new byte[255]));
            //xml.AddParam("attributes", Convert.ToBase64String(new byte[255]));
            xml.SetName("gamemanager");
            xml.SetTransactionType(BombXml.TRANSACTION_TYPE_REQUEST);
            xml.SetMethod("kickedFromGame");
            xml.AddParam("reason", "test");
            client.SendXmlData(xml);
        }
    }
}
