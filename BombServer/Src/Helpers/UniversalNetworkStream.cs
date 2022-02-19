using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;

namespace BombServerEmu_MNR.Src.Helpers
{
    public class UniversalNetworkStream
    {
        SslStream sslStream;
        NetworkStream netStream;

        public bool IsSsl { get; }

        public bool CanRead
        {
            get
            {
                return IsSsl ? sslStream.CanRead : netStream.CanRead;
            }
        }

        public bool CanWrite
        {
            get
            {
                return IsSsl ? sslStream.CanWrite : netStream.CanWrite;
            }
        }

        public UniversalNetworkStream(SslStream stream)
        {
            sslStream = stream;
            IsSsl = true;
        }

        public UniversalNetworkStream(NetworkStream stream)
        {
            netStream = stream;
            IsSsl = false;
        }

        public int Read(ref byte[] buffer)
        {
            return IsSsl ? sslStream.Read(buffer, 0, buffer.Length) : netStream.Read(buffer, 0, buffer.Length);
        }

        public int Read(ref byte[] buffer, int offset, int size)
        {
            return IsSsl ? sslStream.Read(buffer, offset, size) : netStream.Read(buffer, offset, size);
        }

        public void Write(ref byte[] buffer)
        {
            if (IsSsl)
                sslStream.Write(buffer, 0, buffer.Length);
            else
                netStream.Write(buffer, 0, buffer.Length);
        }

        public void Write(ref byte[] buffer, int offset, int size)
        {
            if (IsSsl)
                sslStream.Write(buffer, offset, size);
            else
                netStream.Write(buffer, offset, size);
        }

        public void Close()
        {
            if (IsSsl)
                sslStream.Close();
            else
                netStream.Close();
        }

        ~UniversalNetworkStream()
        {
            Close();
        }
    }
}
