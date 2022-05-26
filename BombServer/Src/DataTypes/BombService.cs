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
    public enum EProtocolType
    {
        TCP = 0,
        RUDP = 1
    }

    class BombService
    {
        const int KEEP_ALIVE_INVERVAL = 10000;

        public string Name { get; }
        public EProtocolType Protocol { get; }
        public bool IsDirectConnect { get; }
        public string IP { get; }
        public ushort Port { get; }
        public string Uuid { get; }

        public IProtocol Listener { get; }

        Dictionary<string, Action<BombService, IClient, BombXml>> methods = new Dictionary<string, Action<BombService, IClient, BombXml>>();
        Action<IClient, EndiannessAwareBinaryReader, EndiannessAwareBinaryWriter> directMethod;
        EEndianness directMethodEndianness;

        public BombService(string name, EProtocolType protocol, bool isDirectConnect, string ip, ushort port, string cert = null, string pass = "")
        {
            Name = name;
            Protocol = protocol;
            IsDirectConnect = isDirectConnect;
            IP = ip;
            Port = port;
            Uuid = UUID.GenerateUUID();
            if (protocol == EProtocolType.TCP) {
                Listener = new SSL(this, ip, port);
                Listener.SetCert(string.Format(@"Data\Certs\{0}", cert), pass);
            } else {
                Listener = new RUDP(this, ip, port);
            }
            Listener.Start();
            var thread = new Thread(() => ListenThread(Listener));
            thread.Start();
            Logging.Log(typeof(BombService), "Started service {0} with protocol {1} at {2}:{3}", LogType.Info, name, Enum.GetName(typeof(EProtocolType), protocol), ip, port);
        }

        public void RegisterMethod(string method, Action<BombService, IClient, BombXml> function)
        {
            methods.Add(method, function);
            Logging.Log(typeof(BombService), "Registered method {0} to service {1}", LogType.Debug, method, Name);
            if (function == null)
            {
                Logging.Log(typeof(BombService), "Registered method {0} in service {1} is null! Service will act like it doesn't exist", LogType.Warning, method, Name);
            }
        }

        public void RegisterDirectConnect(Action<IClient, EndiannessAwareBinaryReader, EndiannessAwareBinaryWriter> function, EEndianness endianness)
        {
            directMethod = function;
            directMethodEndianness = endianness;
            Logging.Log(typeof(BombService), "Registered directConnect method to service {0}", LogType.Debug, Name);
        }

        void ListenThread(IProtocol listener)
        {
            while (true)
            {
                var client = listener.GetClient();
                new Thread(() => ProcessThread(client)).Start();
            }
        }

        void ProcessThread(IClient client)
        {
            try
            {
                client.SetKeepAlive(KEEP_ALIVE_INVERVAL);
                while (client.IsConnected)
                {
                    if (client.HasDirectConnection)
                    {
                        var data = client.GetRawData();
                        var br = new EndiannessAwareBinaryReader(new MemoryStream(data), directMethodEndianness);
                        var bw = new EndiannessAwareBinaryWriter(new MemoryStream(), directMethodEndianness);
                        if (directMethod == null)
                        {
                            Logging.Log(typeof(BombService), "No handler exists for directConnect at service {0}!", LogType.Error, Name);
                            break;
                        }
                        Logging.Log(typeof(BombService), "Received directConnect request at service {0}", LogType.Info, Name);
                        directMethod(client, br, bw);
                    }
                    else
                    {
                        var xml = client.GetNetcodeData();
                        string method = xml.GetMethod();
                        if (!methods.ContainsKey(method) || methods[method] == null)
                        {
                            Logging.Log(typeof(BombService), "No handler exists for method {0} at service {1}!", LogType.Error, method, Name);
                            continue;
                        }
                        Logging.Log(typeof(BombService), "Received request for {0} at service {1}", LogType.Info, method, Name);
                        methods[method](this, client, xml);
                    }
                }
            } catch (Exception e) { Logging.Log(typeof(BombService), "{0}", LogType.Debug, e); }
            Logging.Log(typeof(BombService), "Connection lost!", LogType.Info);
            client.Close();
        }
    }
}
