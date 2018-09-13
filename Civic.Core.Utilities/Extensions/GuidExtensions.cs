﻿using System;
using System.Diagnostics;

namespace Civic.Core.Utilities.Extensions
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

