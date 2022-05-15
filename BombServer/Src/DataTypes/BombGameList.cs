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
    class BombGameList
    {
        public class BombGame
        {
            public class UnkClass
            {
                public string Unk1 { get; set; }
                public string Unk2 { get; set; }

                public byte[] ToArray()
                {
                    using (var ms = new MemoryStream())
                    using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
                    {
                        bw.WriteStringMember(Unk1);
                        bw.WriteStringMember(Unk2);
                        bw.Flush();
                        return ms.ToArray();
                    }
                }
            }

            public List<string> Friends { get; set; } = new List<string>();
            public List<string> Guests { get; set; } = new List<string>();
            public string GameName { get; set; }
            public string Unk1 { get; set; }
            public List<UnkClass> Unk2 { get; set; } = new List<UnkClass>();
        }

        public List<BombGame> Games { get; set; } = new List<BombGame>();
        
        public int Unk1 { get; set; }
        public string Unk2 { get; set; }
        public string Unk3 { get; set; }
        public string Unk4 { get; set; }

        public byte[] SerializeHeader()
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                bw.Write(Games.Count);
                bw.Write(Unk1);
                bw.WriteStringMember(Unk2);
                bw.WriteStringMember(Unk3);
                bw.WriteStringMember(Unk4);
                //TODO
                bw.Write(new byte[255]);    //HACK, the game uses a fixed amount of results, with unused slots being null, implement this
                bw.Flush();
                return ms.ToArray();
            }
        }

        public byte[] SerializeList()
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                foreach (var game in Games)
                {
                    bw.Write(game.Friends.Count);
                    for (int i=0; i<game.Friends.Count; i++)
                    {
                        bw.Write(i);
                        bw.WriteStringMember(game.Friends[i]);
                    }
                    bw.Write(game.Guests.Count);
                    for (int i = 0; i < game.Guests.Count; i++)
                        bw.WriteStringMember(game.Guests[i]);
                    bw.WriteStringMember(game.GameName);
                    bw.WriteStringMember(game.Unk1);
                    bw.Write(game.Unk2.Count);
                    for (int i = 0; i < game.Unk2.Count; i++)
                        bw.Write(game.Unk2[i].ToArray());
                }
                bw.Write(new byte[255]);    //HACK, the game uses a fixed amount of results, with unused slots being null, implement this
                bw.Flush();
                return ms.ToArray();
            }
        }
    }
}
