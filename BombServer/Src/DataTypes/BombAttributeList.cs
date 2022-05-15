using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BombServerEmu_MNR.Src.Helpers;

namespace BombServerEmu_MNR.Src.DataTypes
{
    class BombAttributeList : Dictionary<string, string>
    {
        const int LIST_START = 0x00E86998;
        const int LIST_ENTRY = 0x00E869B0;
        const int LIST_COUNT = 0x20;

        public BombAttributeList() { }
        public BombAttributeList(byte[] data)
        {
            if (data.Length != 0x884)
                return;
            using (var ms = new MemoryStream(data))
            using (var br = new EndiannessAwareBinaryReader(ms, EEndianness.Big))
            {
                if (br.ReadInt32() == LIST_START)
                {
                    for (int i=0; i<LIST_COUNT; i++)
                    {
                        if (br.ReadInt32() == LIST_ENTRY && br.PeekChar() != 0)
                            Add(Encoding.ASCII.GetString(br.ReadBytes(0x20)).Trim('\0'), Encoding.ASCII.GetString(br.ReadBytes(0x20)).Trim('\0'));
                    }
                }
            }
        }

        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(LIST_START);
                for (int i=0; i<LIST_COUNT; i++)
                {
                    bw.Write(LIST_ENTRY);
                    if (i < Count)
                    {
                        var entry = this.ElementAt(i);
                        //TODO: This could be exploited to perform buffer overflows, leave a note here so I dont forget to fix it before release!
                        //      Considering user input doesnt get passed here (yet), we should be safe for now
                        bw.Write(Encoding.ASCII.GetBytes(entry.Key.PadRight(0x20, '\0')));
                        bw.Write(Encoding.ASCII.GetBytes(entry.Value.PadRight(0x20, '\0')));
                    }
                    else
                    {
                        bw.Write(new byte[0x20]);
                        bw.Write(new byte[0x20]);
                    }
                }
                return ms.ToArray();
            }
        }

        public override string ToString()
        {
            var str = "BombAttributeList:\n\n";
            foreach (var attribute in this)
            {
                str += $"{attribute.Key}: {attribute.Value}\n";
            }
            return str;
        }
    }
}
