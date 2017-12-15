﻿using System;
using System.Diagnostics;

namespace Civic.Core.Framework.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string Truncate(this string instr, int maxLength)
        {
            if (maxLength > instr.Length) maxLength = instr.Length;
            return instr.Substring(0, maxLength);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string instr)
        {
            return string.IsNullOrEmpty(instr);
        }


        [DebuggerStepThrough]
        public static int? ToInteger(this string instr)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return null;
            }
            int val;
            if (int.TryParse(instr, out val)) return val;
            return null;
        }


        [DebuggerStepThrough]
        public static double? ToDouble(this string instr)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return null;
            }
            double val;
            if (double.TryParse(instr, out val)) return val;
            return null;
        }


        [DebuggerStepThrough]
        public static string UseDefault(this string instr, string def)
        {
            if (instr.IsNullOrEmpty()) return def;
            return instr;
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTime(this string instr)
        {
            if (instr.IsNullOrEmpty()) return null;

            DateTime val;
            if (DateTime.TryParse(instr, out val)) return val;
            return null;
        }

        [DebuggerStepThrough]
        public static bool ToBool(this string instr)
        {
            if (instr.IsNullOrEmpty()) return false;

            instr = instr.ToLowerInvariant();

            if (instr == "y" || instr == "t" || instr == "true" || instr == "yes") return true;
            return false;
        }

    }
}