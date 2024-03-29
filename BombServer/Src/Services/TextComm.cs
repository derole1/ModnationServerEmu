﻿using System;
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
        public BombService Service { get; }

        public TextComm(string ip, ushort port)
        {
            Service = new BombService("textcomm", EProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterMethod("CreateGroup", CreateGroupHandler);
            Service.RegisterMethod("JoinGroup", CreateGroupHandler);
            Service.RegisterMethod("LeaveGroup", CreateGroupHandler);
            Service.RegisterMethod("InviteUserToGroup", CreateGroupHandler);
            Service.RegisterMethod("TextMessage", CreateGroupHandler);
        }

        void CreateGroupHandler(BombService service, IClient client, BombXml xml)
        {
            client.SendNetcodeData(xml);
        }

        void JoinGroupHandler(BombService service, IClient client, BombXml xml)
        {
            client.SendNetcodeData(xml);
        }

        void LeaveGroupHandler(BombService service, IClient client, BombXml xml)
        {
            client.SendNetcodeData(xml);
        }

        void InviteUserToGroupHandler(BombService service, IClient client, BombXml xml)
        {
            client.SendNetcodeData(xml);
        }

        void TextMessageHandler(BombService service, IClient client, BombXml xml)
        {
            client.SendNetcodeData(xml);
        }
    }
}
