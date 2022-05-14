using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using ModnationServer.Src.Log;

namespace ModnationServer.Src.Helpers.Extensions
{
    static class BinaryExtensions
    {
        public enum ENPAttributeType
        {
            u32 = 1,
            u64 = 2,
            str = 4,
            time = 7,
            bin = 8,
            obj = 0x3002
        }

        public static T ReadNPAttribute<T>(this EndiannessAwareBinaryReader br)
        {
            var type = (ENPAttributeType)br.ReadUInt16();
            var length = br.ReadUInt16();

            var data = br.ReadBytes(length);

            switch (type)
            {
                case ENPAttributeType.u32:
                    return (T)Convert.ChangeType(br._endianness == EEndianness.Little ? BitConverter.ToUInt32(data, 0) : BitConverter.ToUInt32(data, 0).SwapBytes(), typeof(T));
                case ENPAttributeType.u64:
                    return (T)Convert.ChangeType(br._endianness == EEndianness.Little ? BitConverter.ToUInt64(data, 0) : BitConverter.ToUInt64(data, 0).SwapBytes(), typeof(T));
                case ENPAttributeType.str:
                    return (T)Convert.ChangeType(Encoding.ASCII.GetString(data).Trim('\0'), typeof(T));
                case ENPAttributeType.time:
                    return (T)Convert.ChangeType(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(br._endianness == EEndianness.Little ? BitConverter.ToUInt64(data, 0) : BitConverter.ToUInt64(data, 0).SwapBytes()), typeof(T));
                case ENPAttributeType.bin:
                    return (T)Convert.ChangeType(data, typeof(T));
                case ENPAttributeType.obj:
                    Logging.Log(typeof(BinaryExtensions), "Unsupported ENPAttributeType {0}", LogType.Warning, type);
                    return default(T);
                default:
                    Logging.Log(typeof(BinaryExtensions), "Unknown ENPAttributeType {0}", LogType.Warning, type);
                    return default(T);
            }
        }
    }
}
