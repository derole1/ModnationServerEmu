using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers;
using BombServerEmu_MNR.Src.Helpers.Extensions;

namespace BombServerEmu_MNR.Src.Protocols.Clients
{
    class SSLClient : IClient
    {
        public bool IsConnected
        {
            get { return Client.Connected && stream.CanRead && stream.CanWrite; }
        }
        public bool HasDirectConnection { get; set; }
        public IPEndPoint RemoteEndPoint { get; }

        public BombService Service { get; }

        public TcpClient Client { get; }
        public X509Certificate2 Cert { get; }

        UniversalNetworkStream stream;

        Timer keepAlive;

        public SSLClient(BombService service, TcpClient client, X509Certificate2 cert = null)
        {
            Service = service;
            Client = client;
            Cert = cert;
            //SetKeepAlive(5000);
            if (cert == null)
            {
                stream = new UniversalNetworkStream(client.GetStream());
                return;
            }
            Logging.Log(typeof(SSLClient), "Attempting to get SSLStream...", LogType.Debug);
            RemoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            var sslStream = new FixedSslStream(client.GetStream(), false);
            sslStream.AuthenticateAsServer(cert, false, SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Tls, false);
            stream = new UniversalNetworkStream(sslStream);
            Logging.Log(typeof(SSLClient), "SSLStream OK!", LogType.Debug);
        }

        public void SetKeepAlive(int interval)
        {
            keepAlive = new Timer(SendKeepAlive, new AutoResetEvent(false), 0, interval);
            Logging.Log(typeof(SSLClient), "Updated KeepAlive interval to {0}ms", LogType.Debug, interval);
        }

        public BombXml GetNetcodeData()
        {
            return new BombXml(Service, Encoding.ASCII.GetString(ReadSocket()));
        }

        public void SendNetcodeData(BombXml xml)
        {
            WriteSocket(Encoding.ASCII.GetBytes(xml.GetResDoc()), EBombPacketType.NetcodeData);
        }

        public byte[] GetRawData()
        {
            return ReadSocket();
        }

        public void SendReliableGameData(EndiannessAwareBinaryWriter bw)
        {
            WriteSocket(((MemoryStream)bw.BaseStream).ToArray(), EBombPacketType.ReliableGameData);
        }

        public void SendUnreliableGameData(EndiannessAwareBinaryWriter bw)
        {
            WriteSocket(((MemoryStream)bw.BaseStream).ToArray(), EBombPacketType.UnreliableGameData);
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

        public void SendAcknowledge()
        {
            WriteSocket(new byte[0], EBombPacketType.Acknowledge);
        }

        public void SendSync()
        {
            WriteSocket(new byte[0], EBombPacketType.Sync);
        }

        public void Close()
        {
            if (keepAlive != null)
                keepAlive.Dispose();
            stream.Close();
            Client.Close();
        }

        ~SSLClient()
        {
            Close();
        }

        int x = 0;
        byte[] ReadSocket()
        {
            byte[] headerBuf = new byte[24];
            stream.Read(ref headerBuf, 0, headerBuf.Length);
            int len = BitConverter.ToInt32(headerBuf, 0x00).SwapBytes() - 20;
            var type = (EBombPacketType)headerBuf[20];
            if (type == EBombPacketType.NetcodeData)
            {
                byte[] buf = new byte[len];
                int bytesRead = 0;
                do
                {
                    bytesRead += stream.Read(ref buf, bytesRead, buf.Length - bytesRead);
                    Logging.Log(typeof(SSLClient), "Read {0}/{1} bytes", LogType.Debug, bytesRead, buf.Length);
                } while (bytesRead < len);
                return buf.ToArray();
            }
            else
            {
                //Check if we got a KeepAlive packet
                //The server should periodically ping the client with a KeepAlive, that way if the user
                //Is sitting somewhere with no matching, e.g. creation station, the game wont disconnect
                //When the server sends a KeepAlive, the client responds with a KeepAlive and resets its timer
                //
                //We can also use this on the production server to stop modspot connection errors from appearing!
                //Might be a nice temporary solution until matching works
                if (type == EBombPacketType.KeepAlive)
                    Logging.Log(typeof(SSLClient), "KeepAlive recieved!", LogType.Debug);
                else
                    Logging.Log(typeof(SSLClient), "Unsupported EBombPacketType {0}!", LogType.Debug, type);
                return type == 0 ? null : ReadSocket();
            }
        }

        int i = 0;
        void WriteSocket(byte[] data, EBombPacketType packetType)
        {
            using (var ms = new MemoryStream())
            using (var bw = new EndiannessAwareBinaryWriter(ms, EEndianness.Big))
            {
                bw.Write(data.Length + 21);
                bw.Write(new byte[16]);
                bw.Write((byte)packetType);    //Protocol type (TCP=0x64FEFFFF)
                bw.Write((byte)0xFE);
                bw.Write((byte)0xFF);
                bw.Write((byte)0xFF);
                bw.Write(data);
                bw.Write((byte)0);
                byte[] buf = ((MemoryStream)bw.BaseStream).ToArray();

                int bytesWritten = 0;
                do
                {
                    int toWrite = Math.Min(1024, buf.Length - bytesWritten);
                    stream.Write(ref buf, bytesWritten, toWrite);
                    bytesWritten += toWrite;
                    Logging.Log(typeof(SSLClient), "Wrote {0}/{1} bytes", LogType.Debug, bytesWritten, buf.Length);
                } while (bytesWritten < buf.Length);
            }
        }
    }
}
