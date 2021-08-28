using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombServerEmu_MNR.Src.Helpers.Extensions
{
    public static class Endian
    {
        public static int SwapBytes(this int num)
        {
            uint x = (uint)num;
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return (int)(((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8));
        }

        public static uint SwapBytes(this uint num)
        {
            uint x = num;
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return (((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8));
        }
    }
}
