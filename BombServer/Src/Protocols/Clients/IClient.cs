using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers;

namespace BombServerEmu_MNR.Src.Protocols.Clients
{
    interface IClient
    {
        bool IsConnected { get; }
        bool HasDirectConnection { get; set; }
        IPEndPoint RemoteEndPoint { get; }
        BombService Service { get; }

        void SetKeepAlive(int interval);
        BombXml GetNetcodeData();
        void SendNetcodeData(BombXml xml);
        byte[] GetRawData();
        void SendReliableGameData(EndiannessAwareBinaryWriter bw);
        void SendUnreliableGameData(EndiannessAwareBinaryWriter bw);
        void SendKeepAlive();
        void SendReset();
        void SendAcknowledge();
        void SendSync();
        void Close();
    }
}
