using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModnationServer.Src.Helpers.Extensions
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

        public static long SwapBytes(this long num)
        {
            ulong x = (ulong)num;
            // swap adjacent 32-bit blocks
            x = (x >> 32) | (x << 32);
            // swap adjacent 16-bit blocks
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            // swap adjacent 8-bit blocks
            return (long)(((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8));
        }

        public static ulong SwapBytes(this ulong num)
        {
            ulong x = num;
            // swap adjacent 32-bit blocks
            x = (x >> 32) | (x << 32);
            // swap adjacent 16-bit blocks
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            // swap adjacent 8-bit blocks
            return (((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8));
        }
    }
}
