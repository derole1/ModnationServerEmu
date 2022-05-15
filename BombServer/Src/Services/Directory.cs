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
        public BombService Service { get; }

        public Directory(string ip, ushort port)
        {
            Service = new BombService("directory", ProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);

            Service.RegisterMethod("getServiceList", GetServiceListHandler);
        }

        void GetServiceListHandler(BombService service, SSLClient client, BombXml xml)
        {
            xml.SetMethod("getServiceList");
            xml.AddParam("servicesList", Convert.ToBase64String(new BombServiceList(Program.Services).ToArray()));
            xml.AddParam("ClusterUUID", Program.ClusterUuid);
            client.SendXmlData(xml);
        }
    }
}
