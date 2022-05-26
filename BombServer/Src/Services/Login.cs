using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;
using BombServerEmu_MNR.Src.Helpers.Extensions;

//TODO: Is this needed?
namespace BombServerEmu_MNR.Src.Services
{
    class Login
    {
        public BombService Service { get; }

        public Login(string ip, ushort port)
        {
            Service = new BombService("login", EProtocolType.TCP, false, ip, port, "output.pfx", "1234");
            Service.RegisterMethod("startConnect", Connect.StartConnectHandler);
            Service.RegisterMethod("timeSyncRequest", Connect.TimeSyncRequestHandler);
        }
    }
}
