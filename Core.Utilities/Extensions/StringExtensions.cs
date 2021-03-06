﻿using System;
using System.Diagnostics;

namespace Core.Utilities.Extensions
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
        public static bool IsNullOrWhiteSpace(this string instr)
        {
            return string.IsNullOrWhiteSpace(instr);
        }

        [DebuggerStepThrough]
        public static bool IsEqual(this string instr, string compareTo, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
        {
            return string.Compare(instr, compareTo, stringComparison) == 0;
        }


        [DebuggerStepThrough]
        public static DateTime? ToDate(this string value, DateTime? defaultDate = null)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return defaultDate;
            }

            if (DateTime.TryParse(value.Trim(), out var item)) return item;
            return null;
        }

        [DebuggerStepThrough]
        public static int? ToInteger(this string instr, int? defaultValue = null)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return defaultValue;
            }

            if (int.TryParse(instr, out var val)) return val;
            return null;
        }

        [DebuggerStepThrough]
        public static long? ToLong(this string instr, long? defaultValue = null)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return defaultValue;
            }

            if (long.TryParse(instr, out var val)) return val;
            return null;
        }

        [DebuggerStepThrough]
        public static double? ToDouble(this string instr, long? defaultValue = null)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return defaultValue;
            }
            double val;
            if (double.TryParse(instr, out val)) return val;
            return null;
        }


        [DebuggerStepThrough]
        public static string UseDefault(this string instr, string defaultValue)
        {
            if (instr.IsNullOrEmpty()) return defaultValue;
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
        public static string NullToUpper(this string instr)
        {
            if (instr.IsNullOrEmpty()) return null;
            return instr.ToUpperInvariant();
        }

        [DebuggerStepThrough]
        public static string NullToLower(this string instr)
        {
            if (instr.IsNullOrEmpty()) return null;
            return instr.ToUpperInvariant();
        }

        [DebuggerStepThrough]
        public static string EmptyToNull(this string instr)
        {
            if (instr.IsNullOrEmpty()) return null;
            return instr;
        }

        [DebuggerStepThrough]
        public static string NullToEmpty(this string instr)
        {
            if (instr.IsNullOrEmpty()) return "";
            return instr;
        }

        [DebuggerStepThrough]
        public static bool ToBool(this string instr, bool defaultValue = false)
        {
            if (instr.IsNullOrEmpty()) return defaultValue;

            instr = instr.ToLowerInvariant();

            if (instr == "y" || instr == "t" || instr == "true" || instr == "yes") return true;
            return false;
        }

        [DebuggerStepThrough]
        public static string ToYN(this bool instr)
        {
            return instr ? "Y" : "N";
        }

        [DebuggerStepThrough]
        public static string ToYN(this bool? instr)
        {
            return instr.HasValue && instr.Value ? "Y" : "N";
        }
    }
}

