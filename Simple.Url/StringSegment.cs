using Simple.Arena;
using System;

namespace Simple.Url
{
    public readonly struct StringSegment
    {
        public readonly IntPtr Ptr;
        public readonly int Length;

        public static implicit operator ReadOnlySpan<char>(StringSegment @string) =>
            @string.Ptr.ToSpan<char>(@string.Length);

        public static implicit operator Span<char>(StringSegment @string) =>
            @string.Ptr.ToSpan<char>(@string.Length);

        public static implicit operator (IntPtr ptr, int length)(StringSegment @string) =>
            (@string.Ptr, @string.Length);
    }
}
