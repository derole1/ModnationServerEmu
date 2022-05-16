using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BombServerEmu_MNR.Src.Protocols.Clients;
using BombServerEmu_MNR.Src.DataTypes;

namespace BombServerEmu_MNR.Src.Protocols
{
    interface IProtocol
    {
        BombService Service { get; }

        void SetCert(string certPath, string certPass);
        void Start();
        IClient GetClient();
    }
}
