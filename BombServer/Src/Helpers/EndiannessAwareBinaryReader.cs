using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BombServerEmu_MNR.Src.Helpers
{
    public enum EEndianness
    {
        Little,
        Big,
    }

    public class EndiannessAwareBinaryReader : BinaryReader
    {
        private readonly EEndianness _endianness = EEndianness.Little;

        public EndiannessAwareBinaryReader(Stream input) : base(input)
        {
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public EndiannessAwareBinaryReader(Stream input, EEndianness endianness) : base(input)
        {
            _endianness = endianness;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, EEndianness endianness) : base(input, encoding)
        {
            _endianness = endianness;
        }

        public EndiannessAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen, EEndianness endianness) : base(input, encoding, leaveOpen)
        {
            _endianness = endianness;
        }

        public override short ReadInt16() => ReadInt16(_endianness);

        public override int ReadInt32() => ReadInt32(_endianness);

        public override long ReadInt64() => ReadInt64(_endianness);

        public override ushort ReadUInt16() => ReadUInt16(_endianness);

        public override uint ReadUInt32() => ReadUInt32(_endianness);

        public override ulong ReadUInt64() => ReadUInt64(_endianness);

        public override float ReadSingle() => ReadSingle(_endianness);

        public override double ReadDouble() => ReadDouble(_endianness);

        public short ReadInt16(EEndianness endianness) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), endianness), 0);

        public int ReadInt32(EEndianness endianness) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), endianness), 0);

        public long ReadInt64(EEndianness endianness) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), endianness), 0);

        public ushort ReadUInt16(EEndianness endianness) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), endianness), 0);

        public uint ReadUInt32(EEndianness endianness) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), endianness), 0);

        public ulong ReadUInt64(EEndianness endianness) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), endianness), 0);

        public float ReadSingle(EEndianness endianness) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), endianness), 0);

        public double ReadDouble(EEndianness endianness) => BitConverter.ToDouble(ReadForEndianness(sizeof(double), endianness), 0);

        private byte[] ReadForEndianness(int bytesToRead, EEndianness endianness)
        {
            var bytesRead = ReadBytes(bytesToRead);

            if ((endianness == EEndianness.Little && !BitConverter.IsLittleEndian)
                || (endianness == EEndianness.Big && BitConverter.IsLittleEndian))
            {
                Array.Reverse(bytesRead);
            }

            return bytesRead;
        }
    }
}
