using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public static class Extensions
    {
        const string CANCEL = "cancel";

        public static bool IsCancel(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.ToLower() == CANCEL;
        }
    }
}
