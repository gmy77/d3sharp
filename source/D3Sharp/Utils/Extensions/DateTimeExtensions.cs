using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static uint ToUnixTime(this DateTime time)
        {
            return (uint)((time.ToUniversalTime().Ticks - 621355968000000000L) / 10000000L);
        }
    }
}
