using System;
using System.Runtime.CompilerServices;
using Simple.Uri.Parser;
using Superpower.Model;

namespace Simple.Uri
{
    internal static class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> ToSpan(this TextSpan textSpan) => 
            textSpan.Source.AsSpan().Slice(textSpan.Position.Absolute, textSpan.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> ToSpan(this Token<UriToken> token) => 
            !token.HasValue ? new ReadOnlySpan<char>() : token.Span.ToSpan();
        
    }
}
