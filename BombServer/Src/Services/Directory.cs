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
            var bw = new BinaryWriter(new MemoryStream());
            int i = 0;
            foreach (var s in Program.services) {
                if (s.name != "connect" &&
                    s.name != "directory") {
                    string name = s.name.AddNullTerminator();
                    bw.Write(name.Length.SwapBytes());
                    bw.Write(Encoding.ASCII.GetBytes(name));
                    bw.Write(1.SwapBytes());    //Number of servers
                    string ip = s.ip.AddNullTerminator();
                    bw.Write(ip.Length.SwapBytes());
                    bw.Write(Encoding.ASCII.GetBytes(ip));
                    bw.Write(0.SwapBytes());
                    string port = s.port.ToString().AddNullTerminator();
                    bw.Write(port.Length.SwapBytes());
                    bw.Write(Encoding.ASCII.GetBytes(port));
                    string serviceProtocol = (s.protocol == ProtocolType.TCP ? "TCP" : "RUDP").AddNullTerminator();
                    bw.Write(serviceProtocol.Length.SwapBytes());
                    bw.Write(Encoding.ASCII.GetBytes(serviceProtocol));
                    //(s.name == "gamemanager" ? 1 : 0)
                    bw.Write(0.SwapBytes());    //???
                    bw.Write(i.SwapBytes());    //SessionKey
                }
                i++;
            }
            bw.Write(new byte[0xFF]);   //Hack
            xml.AddParam("servicesList", Convert.ToBase64String(((MemoryStream)bw.BaseStream).ToArray()));
            xml.AddParam("ClusterUUID", "1");   //TODO
            client.SendXmlData(xml);
        }
    }
}
