using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombServerEmu_MNR.Src.Helpers.Extensions
{
    static class BinaryExtensions
    {
        public static void WriteStringMember(this EndiannessAwareBinaryWriter bw, object value, Encoding enc)
        {
            var str = value.ToString();
            bw.Write(str.Length+1);
            bw.Write(enc.GetBytes(str));
            bw.Write((byte)0);
        }
        public static void WriteStringMember(this EndiannessAwareBinaryWriter bw, object value) => WriteStringMember(bw, value, Encoding.ASCII);
    }
}
