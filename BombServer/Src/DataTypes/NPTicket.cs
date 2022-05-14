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
    class NPTicket
    {
        public class UnkObj
        {
            public uint Unk1 { get; }
            public byte[] Unk2 { get; }

            public UnkObj(uint unk1, byte[] unk2)
            {
                Unk1 = unk1;
                Unk2 = unk2;
            }
        }

        public uint Version { get; }
        public byte[] SerialVec { get; }
        public uint IssuerId { get; }
        public DateTime IssuedDate { get; }
        public DateTime ExpiredDate { get; }
        public ulong UserId { get; }
        public string OnlineId { get; }
        public byte[] Region { get; }
        public string Domain { get; }
        public byte[] ServiceId { get; }
        public uint Status { get; }
        public uint StatusDuration { get; }
        public uint DateOfBirth { get; }
        public UnkObj Unk1 { get; }

        public NPTicket() { }
        public NPTicket(string strTicket)
        {
            while (strTicket.Length % 4 != 0) { strTicket += "="; }
            using (var ms = new MemoryStream(Convert.FromBase64String(strTicket)))
            using (var br = new EndiannessAwareBinaryReader(ms, EEndianness.Big))
            {
                Version = br.ReadUInt32();
                var length = br.ReadUInt32();
                var unk = br.ReadUInt32();

                SerialVec = br.ReadNPAttribute<byte[]>();
                IssuerId = br.ReadNPAttribute<uint>();
                IssuedDate = br.ReadNPAttribute<DateTime>();
                ExpiredDate = br.ReadNPAttribute<DateTime>();
                UserId = br.ReadNPAttribute<ulong>();
                OnlineId = br.ReadNPAttribute<string>();
                Region = br.ReadNPAttribute<byte[]>();
                Domain = br.ReadNPAttribute<string>();
                ServiceId = br.ReadNPAttribute<byte[]>();
                Status = br.ReadNPAttribute<uint>();
                StatusDuration = br.ReadNPAttribute<uint>();
                DateOfBirth = br.ReadNPAttribute<uint>();
                using (var obj = (EndiannessAwareBinaryReader)br.ReadNPAttribute<object>())
                {
                    if (obj != null)
                    {
                        Unk1 = new UnkObj
                        (
                            obj.ReadNPAttribute<uint>(),
                            obj.ReadNPAttribute<byte[]>()
                        );
                    }
                }
            }
        }
    }
}
