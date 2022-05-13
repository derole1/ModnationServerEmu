using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class SSLClient
    {
        public bool hasDirectConnection = false;

        BombService service;

        public TcpClient client;
        X509Certificate2 cert;

        UniversalNetworkStream stream;

        public SSLClient(BombService service, TcpClient client, X509Certificate2 cert = null)
        {
            this.service = service;
            this.client = client;
            this.cert = cert;
            //SetKeepAlive(5000);
            if (cert == null)
            {
                stream = new UniversalNetworkStream(client.GetStream());
                return;
            }
            Logging.Log(typeof(SSLClient), "Attempting to get SSLStream...", LogType.Debug);
            var sslStream = new FixedSslStream(client.GetStream(), false);
            sslStream.AuthenticateAsServer(cert, false, SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Tls, false);
            stream = new UniversalNetworkStream(sslStream);
            Logging.Log(typeof(SSLClient), "SSLStream OK!", LogType.Debug);
        }

        public void SetKeepAlive(uint interval)
        {
            int size = sizeof(uint);
            byte[] keepAlive = new byte[size * 3];

            // Pack the byte array:
            // Turn keepalive on
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, size);
            // Set amount of time without activity before sending a keepalive to 5 seconds
            Buffer.BlockCopy(BitConverter.GetBytes(interval), 0, keepAlive, size, size);
            // Set keepalive interval to 5 seconds
            Buffer.BlockCopy(BitConverter.GetBytes(interval), 0, keepAlive, size * 2, size);

            // Set the keep-alive settings on the underlying Socket
            client.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);
            Logging.Log(typeof(SSLClient), "Updated KeepAlive interval to {0}ms", LogType.Debug, interval);
        }

        public BombXml GetXmlData()
        {
            return new BombXml(service, Encoding.ASCII.GetString(ReadSocket()));
        }

        public void SendXmlData(BombXml xml)
        {
            WriteSocket(Encoding.ASCII.GetBytes(xml.GetResDoc()));
        }

        public byte[] GetRawData()
        {
            return ReadSocket();
        }

        public void SendRawData(byte[] data)
        {
            WriteSocket(data);
        }

        public void SendRawData(BinaryWriter bw)
        {
            WriteSocket(((MemoryStream)bw.BaseStream).ToArray());
        }

        public bool HasConnection()
        {
            return client.Connected && stream.CanRead && stream.CanWrite;
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        ~SSLClient()
        {
            Close();
        }

        int x = 0;
        byte[] ReadSocket()
        {
            byte[] headerBuf = new byte[4];
            stream.Read(ref headerBuf, 0, headerBuf.Length);
            int len = BitConverter.ToInt32(headerBuf, 0x00).SwapBytes();
            byte[] buf = new byte[len];
            int bytesRead = 0;
            do
            {
                bytesRead += stream.Read(ref buf, bytesRead, buf.Length - bytesRead);
                Logging.Log(typeof(SSLClient), "Read {0}/{1} bytes", LogType.Debug, bytesRead, buf.Length);
            } while (bytesRead < len);

            File.WriteAllBytes(string.Format("req{0}_{1}.bin", x, GetHashCode()), headerBuf.Concat(buf).ToArray());
            x++;

            return buf.Skip(20).ToArray();
        }

        int i = 0;
        void WriteSocket(byte[] data)
        {
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write((data.Length+21).SwapBytes());
            bw.Write(new byte[16]);
            //bw.Write((ulong)0xFFFFFFFFFFFFFFFF);
            //bw.Write((ulong)0xFFFFFFFFFFFFFFFF);
            bw.Write(0x64FEFFFF.SwapBytes());    //Protocol type (TCP=0x64FEFFFF)
            bw.Write(data);
            bw.Write((byte)0);
            byte[] buf = ((MemoryStream)bw.BaseStream).ToArray();

            File.WriteAllBytes(string.Format("res{0}_{1}.bin", i, GetHashCode()), buf);
            i++;

            //File.WriteAllBytes("debug.bin", buf);
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
