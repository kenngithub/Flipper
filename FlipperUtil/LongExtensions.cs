using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipperUtil
{
    public static class LongExtensions
    {
        public static string ExpirySecondsToMin(this int expiry)
        {
            return (expiry / 60) + "." + (expiry % 60).ToString("D2");
        }
    }
}
