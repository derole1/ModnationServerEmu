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
    class Directory
    {
        public BombService service;

        public Directory(string ip, ushort port)
        {
            service = new BombService("directory", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            service.RegisterMethod("getServiceList", GetServiceListHandler);
        }

        void GetServiceListHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("getServiceList");
            xml.AddParam("servicesList", Convert.ToBase64String(new BombServiceList(Program.services).ToArray()));
            xml.AddParam("ClusterUUID", "1");   //TODO
            client.SendXmlData(xml);
        }
    }
}
