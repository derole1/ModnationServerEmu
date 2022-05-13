using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Helpers;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.DataTypes
{
    class BombServiceList
    {
        public Dictionary<string, List<BombService>> ServiceList { get; private set; } = new Dictionary<string, List<BombService>>();

        public BombServiceList() { }
        public BombServiceList(BombService service) => Add(service);
        public BombServiceList(params BombService[] services) => AddRange(services);
        public BombServiceList(IEnumerable<BombService> services) => AddRange(services.ToArray());

        public void Add(BombService service)
        {
            if (!ServiceList.ContainsKey(service.name))
                ServiceList.Add(service.name, new List<BombService>());
            ServiceList[service.name].Add(service);
        }

        public void AddRange(params BombService[] services)
        {
            foreach (var service in services)
                Add(service);
        }

        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                foreach (var serviceType in ServiceList)
                {
                    bw.WriteStringMember(serviceType.Key);
                    bw.Write(serviceType.Value.Count);
                    foreach (var service in serviceType.Value)
                    {
                        bw.WriteStringMember(service.ip);
                        bw.WriteStringMember("9aa555a8-cc1a-11e8-81c9-22000acbd9b1");
                        bw.WriteStringMember(service.port);
                        bw.WriteStringMember(service.protocol);
                        bw.Write(0);
                        bw.Write(1);    //SessionKey
                    }
                }
                bw.Write(new byte[255]);    //HACK, the game uses a fixed amount of results, with unused slots being null, implement this
                bw.Flush();
                File.WriteAllBytes("ServiceList.bin", ms.ToArray());
                return ms.ToArray();
            }
        }
    }
}
