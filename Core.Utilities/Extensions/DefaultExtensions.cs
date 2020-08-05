using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class DefaultExtensions
    {

        [DebuggerStepThrough]
        public static string Default(this string instr, string value)
        {
            if (instr.IsNullOrEmpty()) return value;
            return instr;
        }

        [DebuggerStepThrough]
        public static long Default(this long? inValue, long value)
        {
            if (!inValue.HasValue) return value;
            return inValue.Value;
        }

        [DebuggerStepThrough]
        public static long Default(this long inValue, long value)
        {
            if (inValue == 0) return value;
            return inValue;
        }

        [DebuggerStepThrough]
        public static int Default(this int? inValue, int value)
        {
            if (!inValue.HasValue) return value;
            return inValue.Value;
        }

        [DebuggerStepThrough]
        public static bool Default(this bool? inValue, bool value)
        {
            if (!inValue.HasValue) return value;
            return inValue.Value;
        }

        [DebuggerStepThrough]
        public static int Default(this int inValue, int value)
        {
            if (inValue==0) return value;
            return inValue;
        }
    }
}
