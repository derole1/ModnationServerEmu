using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ModnationServer.Src.Helpers;
using ModnationServer.Src.Helpers.Extensions;

namespace ModnationServer.Src.DataTypes
{
    class PlayerConnectTicket
    {
        const byte KEY_MAGIC = 0x3A;

        public class PlayerConnectObject
        {
            public byte Unk1 { get; set; }
            public string Key { get; set; }
            public ushort Unk2 { get; set; }
            public string Value { get; set; }
        }

        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public PlayerConnectTicket() { }
        public PlayerConnectTicket(string data)
        {
            var splitData = data.Split(new string[] { "--" }, StringSplitOptions.None);
            using (var ms = new MemoryStream(Convert.FromBase64String(splitData[0])))
            using (var br = new EndiannessAwareBinaryReader(ms))
            {
                var unk = br.ReadUInt32();
                while (ms.Position < ms.Length)
                {
                    if (br.ReadByte() == KEY_MAGIC)
                    {

                    }
                }
            }
        }
    }
}
