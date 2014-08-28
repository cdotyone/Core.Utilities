namespace Civic.Core.Framework.Extensions
{
    public static class Strings
    {
        internal static string Truncate(this string instr, int maxLength)
        {
            if (maxLength > instr.Length) maxLength = instr.Length;
            return instr.Substring(0, maxLength);
        }
    }
}
