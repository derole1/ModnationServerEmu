using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ModnationServer.Src.Helpers;
using ModnationServer.Src.Helpers.Extensions;
using ModnationServer.Src.Sessions;

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
        public PlayerConnectTicket(SessionManager.Session session)
        {
            //Unimplemented
        }

        public string Serialize()
        {
            //TODO
            // return "BAh7EzoPc2Vzc2lvbl9pZCIlMGVlZjEyNDk0NmJiOThmM2ZlYmQzODM0NmUwYTc3MDU6C2RvbWFpbiIAOhJsYW5ndWFnZV9jb2RlIgplbi11czoQcmVnaW9uX2NvZGUiCXNjZWE6DXRpbWV6b25lIgktMzAwOg1wbGF0Zm9ybSIIUFMzOg1wcmVzZW5jZUkiC09OTElORQY6DWVuY29kaW5nIg1VUy1BU0NJSUkiCmZsYXNoBjsMQA1JQzonQWN0aW9uQ29udHJvbGxlcjo6Rmxhc2g6OkZsYXNoSGFzaHsGOgtub3RpY2VJIiVQcmVmZXJlbmNlcyBzdWNlc3NmdWxseSB1cGRhdGVkIQY7DEANBjoKQHVzZWR7BjsORjoPaXBfYWRkcmVzcyISNzMuMjE1LjE4NS42NDoOcGxheWVyX2lkaQO3JB86DXVzZXJuYW1lIg1zaGFuejJuZDoPY29uc29sZV9pZCIYMDA6MzY6MTQxOjE4MToxMzo1OToVdmlld2luZ19wbGF0Zm9ybSIIUFMzOhZsYXN0X3JlcXVlc3RfbWFkZWwrB3yiqFs=--9d686236e0bc9f78ebc46df5cf9be36797c258cf"
            return $"{Convert.ToBase64String(Encoding.ASCII.GetBytes("TEMP TICKET"))}--00000000000000000000000000000000";
        }

        public override string ToString()
        {
            return $"PlayerConnectTicket:\n\nUnimplemented!";
        }
    }
}
