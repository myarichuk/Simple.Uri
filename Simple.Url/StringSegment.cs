using Simple.Arena;
using System;

namespace Simple.Url
{
    public readonly struct StringSegment
    {
        internal readonly IntPtr Ptr;
        internal readonly int Length;

        public StringSegment(IntPtr ptr, int length)
        {
            Ptr = ptr;
            Length = length;
        }

        public Span<char> AsSpan() => this;

        public static implicit operator StringSegment(ReadOnlySpan<char> span) =>
            new StringSegment(span.ToIntPtr(), span.Length);

        public static implicit operator StringSegment(Span<char> span) =>
            new StringSegment(span.ToIntPtr(), span.Length);

        public unsafe static implicit operator ReadOnlySpan<char>(StringSegment @string) =>
            new ReadOnlySpan<char>(@string.Ptr.ToPointer(), @string.Length);

        public unsafe static implicit operator Span<char>(StringSegment @string) =>
             new Span<char>(@string.Ptr.ToPointer(), @string.Length);

        public static implicit operator (IntPtr ptr, int length)(StringSegment @string) =>
            (@string.Ptr, @string.Length);
    }
}
