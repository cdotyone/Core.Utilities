using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civic.Core.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? StartOfDay(this DateTime?  value)
        {
            if (value.HasValue) return DateTime.Parse(value.Value.ToShortDateString());
            return null;
        }

        public static DateTime? EndofDay(this DateTime? value)
        {
            if (value.HasValue) return DateTime.Parse(value.Value.ToShortDateString()).AddDays(1).AddMilliseconds(-1);
            return null;
        }
    }
}
