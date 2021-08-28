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
    class TextComm
    {
        public BombService service;

        public TextComm(string ip, ushort port)
        {
            service = new BombService("textcomm", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("CreateGroup", CreateGroupHandler);
            service.RegisterMethod("JoinGroup", CreateGroupHandler);
            service.RegisterMethod("LeaveGroup", CreateGroupHandler);
            service.RegisterMethod("InviteUserToGroup", CreateGroupHandler);
            service.RegisterMethod("TextMessage", CreateGroupHandler);
        }

        void CreateGroupHandler(BombService service, SSLClient client, BombXml xml)
        {
            client.SendXmlData(xml);
        }

        void JoinGroupHandler(BombService service, SSLClient client, BombXml xml)
        {
            client.SendXmlData(xml);
        }

        void LeaveGroupHandler(BombService service, SSLClient client, BombXml xml)
        {
            client.SendXmlData(xml);
        }

        void InviteUserToGroupHandler(BombService service, SSLClient client, BombXml xml)
        {
            client.SendXmlData(xml);
        }

        void TextMessageHandler(BombService service, SSLClient client, BombXml xml)
        {
            client.SendXmlData(xml);
        }
    }
}
