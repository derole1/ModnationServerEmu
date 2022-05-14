using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BombServerEmu_MNR.Src.Helpers
{
    public class EndiannessAwareBinaryWriter : BinaryWriter
    {
        private readonly EEndianness _endianness = EEndianness.Little;

        public EndiannessAwareBinaryWriter(Stream input) : base(input)
        {
        }

        public EndiannessAwareBinaryWriter(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public EndiannessAwareBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public EndiannessAwareBinaryWriter(Stream input, EEndianness endianness) : base(input)
        {
            _endianness = endianness;
        }

        public EndiannessAwareBinaryWriter(Stream input, EEndianness endianness, Encoding encoding) : base(input, encoding)
        {
            _endianness = endianness;
        }

        public EndiannessAwareBinaryWriter(Stream input, EEndianness endianness, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            _endianness = endianness;
        }

        public override void Write(short value) => Write(value, _endianness);

        public override void Write(int value) => Write(value, _endianness);

        public override void Write(long value) => Write(value, _endianness);

        public override void Write(ushort value) => Write(value, _endianness);

        public override void Write(uint value) => Write(value, _endianness);

        public override void Write(ulong value) => Write(value, _endianness);

        public override void Write(float value) => Write(value, _endianness);

        public override void Write(double value) => Write(value, _endianness);

        public override void Write(bool value) => Write(value, _endianness);

        public void Write(short value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(int value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(long value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(ushort value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(uint value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(ulong value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(float value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(double value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(bool value, EEndianness endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        private void WriteForEndianness(byte[] buffer, EEndianness endianness)
        {
            if ((endianness == EEndianness.Little && !BitConverter.IsLittleEndian)
                || (endianness == EEndianness.Big && BitConverter.IsLittleEndian))
            {
                Array.Reverse(buffer);
            }

            Write(buffer);
        }
    }
}
