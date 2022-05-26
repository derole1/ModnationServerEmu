using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BombServerEmu_MNR.Src.Helpers
{
    class BombHMAC
    {
        public static ushort GetMD516(byte[] data) //TODO: Salt
        {
            switch (data[0])
            {
                case 0x66:
                    Buffer.BlockCopy(new byte[2], 0, data, 12, 2);
                    break;
                case 0x65:
                    Buffer.BlockCopy(new byte[2], 0, data, 6, 2);
                    break;
                case 0x62:
                    Buffer.BlockCopy(new byte[2], 0, data, 2, 2);
                    break;
            }

            var hmac = HMACMD5.Create().ComputeHash(data);

            ushort result = 0;
            for (int i=0; i<16; i+=2) //Squish down our HMAC into 16 bits
                result ^= (ushort)((hmac[i + 1] << 8) ^ hmac[i]);
            return result;
        }
        public static uint GetMD532(byte[] data) //TODO: Salt
        {
            switch (data[0])
            {
                case 0x64:
                    Buffer.BlockCopy(new byte[4], 0, data, 8, 4);
                    break;
            }

            var hmac = HMACMD5.Create().ComputeHash(data);

            uint result = 0;
            for (int i = 0; i < 16; i += 4) //Squish down our HMAC into 32 bits
                result ^= (ushort)(hmac[i] ^ (hmac[i + 1] << 8) ^ (hmac[i + 2] << 16) ^ (hmac[i + 3] << 24));
            return result;
        }
    }
}
