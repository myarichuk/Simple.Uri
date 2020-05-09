using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Simple.Uri.Parser
{
    internal static class Validator
    {
        public readonly static HashSet<char> Reserved = new HashSet<char> { ';', '/', '?', ':', '@', '=', '&' };
        public readonly static HashSet<char> Special = new HashSet<char> { '$', '-', '_', '.', '+', '!', '*', '\'', '(', ')' };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidForSchema(char c) => char.IsLetter(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidForPath(char c) => char.IsLetterOrDigit(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidForQueryName(char c) => char.IsLetterOrDigit(c);
    }
}
