using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Civic.Core.Framework.Extensions
{
    // ReSharper disable PossibleInvalidOperationException

    public static class DictionaryExtension
    {
        [DebuggerStepThrough]
        public static string MacroReplace<T>(this Dictionary<string, T> nvc, string instr, bool removeUnknown = false)
        {
            for (var i = 0; i < 5; i++)
            {
                if (!instr.Contains("{")) return instr;
                instr = macroReplace(nvc, instr, removeUnknown);
            }
            return instr;
        }

        private static string macroReplace<T>(Dictionary<string, T> nvc, string instr, bool removeUnknown)
        {
            var allx = new Regex(@"\{+(\w*)\}+");

            var lnames = new Dictionary<string, string>();
            foreach (var key in nvc.Keys)
            {
                lnames[key.ToLowerInvariant()] = key;
            }

            foreach (Match match in allx.Matches(instr))
            {
                if (match.Success)
                {
                    var fname = match.Value.Trim(new[] {'{', '}'}).ToLowerInvariant();
                    if (lnames.ContainsKey(fname))
                    {
                        instr = instr.Replace(match.Value, nvc[lnames[fname]].ToString());
                    }
                    else
                    {
                        if (removeUnknown)
                        {
                            instr = instr.Replace(match.Value, "");                            
                        }
                    }
                }
            }

            return instr;
        }

        [DebuggerStepThrough]
        public static Dictionary<string, T> Clone<T>(this Dictionary<string, T> nvc)
        {
            var clone = new Dictionary<string, T>();
            foreach (var pair in nvc)
            {
                clone[pair.Key] = pair.Value;
            }
            return clone;
        }

        [DebuggerStepThrough]
        public static T GetWithDefault<T>(this Dictionary<string, T> dict, string key, T defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key].ToString().IsNullOrWhiteSpace()) return defaultValue;
            return dict[key];
        }

        [DebuggerStepThrough]
        public static bool GetWithDefault(this Dictionary<string, string> dict, string key, bool defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key].IsNullOrWhiteSpace()) return defaultValue;
            return dict[key].ToBool(defaultValue);
        }

        [DebuggerStepThrough]
        public static int GetWithDefault(this Dictionary<string, string> dict, string key, int defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key].IsNullOrWhiteSpace()) return defaultValue;
            return dict[key].ToInteger(defaultValue).Value;
        }

        [DebuggerStepThrough]
        public static long GetWithDefault(this Dictionary<string, string> dict, string key, long defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key].IsNullOrWhiteSpace()) return defaultValue;
            return dict[key].ToLong(defaultValue).Value;
        }

        [DebuggerStepThrough]
        public static DateTime GetWithDefault(this Dictionary<string, string> dict, string key, DateTime defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key].IsNullOrWhiteSpace()) return defaultValue;
            return dict[key].ToDate(defaultValue).Value;
        }

    }
}
