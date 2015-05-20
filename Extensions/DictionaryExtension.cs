using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Civic.Core.Framework.Extensions
{
    public static class DictionaryExtension
    {
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

        public static Dictionary<string, T> Clone<T>(this Dictionary<string, T> nvc)
        {
            var clone = new Dictionary<string, T>();
            foreach (var pair in nvc)
            {
                clone[pair.Key] = pair.Value;
            }
            return clone;
        }
    }
}
