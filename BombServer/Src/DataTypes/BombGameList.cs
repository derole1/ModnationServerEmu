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

    class BombGameList : List<BombGame>
    {
        public int Unk1 { get; set; }
        public string Unk2 { get; set; }
        public string Unk3 { get; set; }
        public string Unk4 { get; set; }

        public byte[] SerializeHeader()
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                bw.Write(Count);
                bw.Write(Unk1);
                bw.WriteStringMember(Unk2);
                bw.WriteStringMember(Unk3);
                bw.WriteStringMember(Unk4);
                bw.Flush();
                return ms.ToArray();
            }
        }

        public byte[] SerializeList()
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                foreach (var game in this)
                    bw.Write(game.ToArray());
                bw.Flush();
                return ms.ToArray();
            }
        }
    }
}
