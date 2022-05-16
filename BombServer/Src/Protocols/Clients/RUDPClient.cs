using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Protocols.Clients
{
    class RUDPClient : IClient
    {
        public enum EBombPacketType : byte
        {
            KeepAlive = 0x61,
            Data = 0x64,
            Unk1 = 0x66,    //Unk1 and Unk2 exist in the switch statement, but seem to be unimplemented
            Unk2 = 0x65,
            Reset = 0x60    //This seems to mainly be used in RUDP protocol
        }

        public bool IsConnected
        {
            get { return true; }    //TODO: Get connection state
        }
        public bool HasDirectConnection { get; set; }
        public IPEndPoint RemoteEndPoint { get; }

        public BombService Service { get; }

        public UdpClient Client { get; }

        Timer keepAlive;

        public RUDPClient(BombService service, UdpClient listener)
        {
            Service = service;
            Client = listener;
            RemoteEndPoint = (IPEndPoint)Client.Client.RemoteEndPoint;
            Logging.Log(typeof(RUDPClient), "Unimplemented!", LogType.Error);
        }

        public void SetKeepAlive(int interval)
        {
            keepAlive = new Timer(SendKeepAlive, new AutoResetEvent(false), 0, interval);
            Logging.Log(typeof(RUDPClient), "Updated KeepAlive interval to {0}ms", LogType.Debug, interval);
        }

        public BombXml GetXmlData()
        {
            return new BombXml(Service, Encoding.ASCII.GetString(ReadSocket()));
        }

        public void SendXmlData(BombXml xml)
        {
            WriteSocket(Encoding.ASCII.GetBytes(xml.GetResDoc()), EBombPacketType.Data);
        }

        public byte[] GetRawData()
        {
            return ReadSocket();
        }

        public void SendRawData(byte[] data)
        {
            WriteSocket(data, EBombPacketType.Data);
        }

        public void SendRawData(EndiannessAwareBinaryWriter bw)
        {
            WriteSocket(((MemoryStream)bw.BaseStream).ToArray(), EBombPacketType.Data);
        }

        public void SendKeepAlive()
        {
            WriteSocket(new byte[0], EBombPacketType.KeepAlive);
        }
        void SendKeepAlive(object stateInfo) => SendKeepAlive();    //For timer

        public void SendReset()
        {
            WriteSocket(new byte[0], EBombPacketType.Reset);
        }

        public void Close()
        {
            if (keepAlive != null)
                keepAlive.Dispose();
            //TODO: Stream
            Client.Close();
        }

        ~RUDPClient()
        {
            Close();
        }

        int x = 0;
        byte[] ReadSocket()
        {
            Logging.Log(typeof(RUDPClient), "ReadSocket: Unimplemented!", LogType.Error);
            return null;
        }

        int i = 0;
        void WriteSocket(byte[] data, EBombPacketType packetType)
        {
            Logging.Log(typeof(RUDPClient), "WriteSocket: Unimplemented!", LogType.Error);
        }
    }
}
