using System;

namespace Simple.Url
{
    internal static class Extensions
    {
        public static unsafe Span<char> AsSpan(this (IntPtr ptr, int size) stringAsPtr) => 
            new Span<char>(stringAsPtr.ptr.ToPointer(), stringAsPtr.size);
    }
}
