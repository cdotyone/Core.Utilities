using System;
using System.Diagnostics;

namespace Core.Utilities.Extensions
{
    public static class GuidExtensions
    {
        [DebuggerStepThrough]
        public static string ToUID(this Guid guid)
        {
            return guid.ToString().Replace("-", "");
        }
    }
}

