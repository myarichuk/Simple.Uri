using System;

namespace Simple.Url
{
    public static class Extensions
    {
        //implicit conversion operator here
        public static unsafe StringSegment AsStringSegment(this ReadOnlySpan<char> span) => span;

        //implicit conversion operator here
        public static unsafe StringSegment AsStringSegment(this Span<char> span) => span;
    }
}
