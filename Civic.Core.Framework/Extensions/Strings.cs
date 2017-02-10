namespace Civic.Core.Framework.Extensions
{
    public static class Strings
    {
        public static string Truncate(this string instr, int maxLength)
        {
            if (maxLength > instr.Length) maxLength = instr.Length;
            return instr.Substring(0, maxLength);
        }

        public static bool IsNullOrEmpty(this string instr)
        {
            return string.IsNullOrEmpty(instr);
        }
    }
}
