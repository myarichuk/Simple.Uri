using System;
using System.Diagnostics;

namespace Simple.Uri
{
    [DebuggerDisplay("{AsSpan().ToString()}")]
    public unsafe readonly struct StringSegment
    {
        internal readonly IntPtr Ptr;
        internal readonly int Length;

        public StringSegment(IntPtr ptr, int length)
        {
            Ptr = ptr;
            Length = length;
        }

        public override string ToString() => AsSpan().ToString();

        public Span<char> AsSpan() => new Span<char>(Ptr.ToPointer(), Length);
        public ReadOnlySpan<char> AsReadOnlySpan() => new ReadOnlySpan<char>(Ptr.ToPointer(), Length);
    }
}
