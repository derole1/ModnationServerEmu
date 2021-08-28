using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombServerEmu_MNR.Src.Helpers.Extensions
{
    public static class String
    {
        public static string AddNullTerminator(this string str)
        {
            return str + "\0";
        }
    }
}
