using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using BombServerEmu_MNR.Src.Log;
using BombServerEmu_MNR.Src.Protocols;
using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.Helpers;

namespace BombServerEmu_MNR.Src.DataTypes
{
    public enum ProtocolType
    {
        TCP = 0,
        RUDP = 1
    }

    class BombService
    {
        const int KEEP_ALIVE_INVERVAL = 10000;

        public string Name { get; }
        public ProtocolType Protocol { get; }
        public bool IsDirectConnect { get; }
        public string IP { get; }
        public ushort Port { get; }
        public string Uuid { get; }

        SSL sslListener;
        RUDP rudpListener;

        Dictionary<string, Action<BombService, SSLClient, BombXml>> methods = new Dictionary<string, Action<BombService, SSLClient, BombXml>>();
        Action<SSLClient, BinaryReader, BinaryWriter> directMethod;

        public BombService(string name, ProtocolType protocol, bool isDirectConnect, string ip, ushort port, string cert = null, string pass = "")
        {
            Name = name;
            Protocol = protocol;
            IsDirectConnect = isDirectConnect;
            IP = ip;
            Port = port;
            Uuid = UUID.GenerateUUID();
            if (protocol == ProtocolType.TCP) {
                sslListener = new SSL(this, ip, port);
                sslListener.SetCert(string.Format(@"Data\Certs\{0}", cert), pass);
                sslListener.Start();
            } else {
                rudpListener = new RUDP(this, ip, port);
            }
            var thread = protocol == ProtocolType.TCP 
                ? new Thread(() => ListenThread(sslListener)) 
                : new Thread(() => ListenThread(rudpListener));
            thread.Start();
            Logging.Log(typeof(BombService), "Started service {0} with protocol {1} at {2}:{3}", LogType.Info, name, Enum.GetName(typeof(ProtocolType), protocol), ip, port);
        }

        public void RegisterMethod(string method, Action<BombService, SSLClient, BombXml> function)
        {
            methods.Add(method, function);
            Logging.Log(typeof(BombService), "Registered method {0} to service {1}", LogType.Debug, method, Name);
            if (function == null)
            {
                Logging.Log(typeof(BombService), "Registered method {0} in service {1} is null! Service will act like it doesn't exist", LogType.Warning, method, Name);
            }
        }

        public void RegisterDirectConnect(Action<SSLClient, BinaryReader, BinaryWriter> function)
        {
            directMethod = function;
            Logging.Log(typeof(BombService), "Registered directConnect method to service {0}", LogType.Debug, Name);
        }

        void ListenThread(SSL sslListener)
        {
            while (true)
            {
                var client = sslListener.GetClient();
                new Thread(() => ProcessThread(client)).Start();
            }
        }

        void ProcessThread(SSLClient sslClient)
        {
            try
            {
                sslClient.SetKeepAlive(KEEP_ALIVE_INVERVAL);
                while (sslClient.HasConnection())
                {
                    if (sslClient.hasDirectConnection)
                    {
                        var data = sslClient.GetRawData();
                        var br = new BinaryReader(new MemoryStream(data));
                        var bw = new BinaryWriter(new MemoryStream());
                        if (directMethod == null)
                        {
                            Logging.Log(typeof(BombService), "No handler exists for directConnect at service {0}!", LogType.Error, Name);
                            break;
                        }
                        Logging.Log(typeof(BombService), "Received directConnect request at service {0}", LogType.Info, Name);
                        directMethod(sslClient, br, bw);
                    }
                    else
                    {
                        var xml = sslClient.GetXmlData();
                        string method = xml.GetMethod();
                        if (!methods.ContainsKey(method) || methods[method] == null)
                        {
                            Logging.Log(typeof(BombService), "No handler exists for method {0} at service {1}!", LogType.Error, method, Name);
                            continue;
                        }
                        Logging.Log(typeof(BombService), "Received request for {0} at service {1}", LogType.Info, method, Name);
                        methods[method](this, sslClient, xml);
                    }
                }
            } catch (Exception e) { Logging.Log(typeof(BombService), "{0}", LogType.Debug, e); }
            Logging.Log(typeof(BombService), "Connection lost!", LogType.Info);
            sslClient.Close();
        }

        void ListenThread(RUDP rudpListener)
        {

        }

        void ProcessThread(RUDPClient rudpClient)
        {

        }
    }
}
