using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombServerEmu_MNR.Src.Helpers
{
    class UUID
    {
        public static string GenerateUUID()
        {
            Guid uuid = Guid.NewGuid();
            return uuid.ToString();
        }
    }
}
