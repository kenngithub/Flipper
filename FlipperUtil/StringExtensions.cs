using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipperUtil
{
    public static class StringExtensions
    {
        public static string Args(this string @string, params object[] args)
        {
            return string.Format(@string, args);
        }
    }
}
