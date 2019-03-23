using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? StartOfDay(this DateTime?  value)
        {
            if (value.HasValue) return DateTime.Parse(value.Value.ToShortDateString());
            return null;
        }

        public static DateTime? EndOfDay(this DateTime? value)
        {
            if (value.HasValue) return DateTime.Parse(value.Value.ToShortDateString()).AddDays(1).AddMilliseconds(-1);
            return null;
        }
    }
}
