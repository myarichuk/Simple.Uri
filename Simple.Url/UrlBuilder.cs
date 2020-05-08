using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Simple.Arena;

namespace Simple.Url
{
    public enum Schema
    {
        Http,
        Https
    }

    public struct UrlBuilder : IFormattable
    {
        private const string UrlPathSeparator = "/";

        private readonly Arena.Arena _arena;
        private readonly StringSegment _baseUrl;
        private readonly List<StringSegment> _urlPathSegments;
        

        public readonly Schema Schema;

        /// <exception cref="T:System.ArgumentException">base path must have only HTTPS or HTTP schema</exception>
        public UrlBuilder(Arena.Arena arena, ReadOnlySpan<char> baseUrl)
        {
            _arena = arena ?? throw new ArgumentNullException(nameof(arena));
            if(baseUrl.IsEmpty || baseUrl.IsWhiteSpace())
                throw new ArgumentNullException(nameof(baseUrl));

            if(_arena.IsDisposed)
                throw new ObjectDisposedException(nameof(arena));

            _urlPathSegments = Cache.SegmentListCache.Get();
            _baseUrl = baseUrl;
            
            if (baseUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                Schema = Schema.Http;
            else if (baseUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                Schema = Schema.Https;
            else
                throw new ArgumentException("base path must have only HTTPS or HTTP schema", nameof(baseUrl));

            _arena.Disposed += OnDispose;
        }

        public void AddPathSegment(ReadOnlySpan<char> pathSegment)
        {
            if(!_arena.TryAllocate<char>(pathSegment.Length, out var storedPathSegment))
                ThrowOutOfMemory();

            pathSegment.CopyTo(storedPathSegment);
            _urlPathSegments.Add(storedPathSegment.AsStringSegment());
        }

        public string GenerateUrl()
        {
            var length = 
                _baseUrl.Length +  //path size
                _urlPathSegments.Count; //path separators

            //note: this can be done with Linq's Sum(), but that will do delegate allocation
            for(int i = 0; i < _urlPathSegments.Count; i++)
                length += _urlPathSegments[i].Length;

            return string.Create(length, (_baseUrl, _urlPathSegments), 
                (Span<char> output,
                        (StringSegment baseUrl, 
                         List<StringSegment> segments) urlParts) =>
                {
                    var offset = 0;
                    var separatorAsSpan = UrlPathSeparator.AsSpan();

                    urlParts.baseUrl.AsSpan().CopyTo(output.Slice(offset));
                    offset += urlParts.baseUrl.Length;

                    if(urlParts.segments.Count > 0) //in an edge case, there is only a base path and no path
                        separatorAsSpan.CopyTo(output.Slice(offset++)); //path separator after base url

                    for (int i = 0; i < urlParts.segments.Count; i++)
                    {
                        var currentSpan = urlParts.segments[i].AsSpan();

                        currentSpan.CopyTo(output.Slice(offset));
                        offset += urlParts.segments[i].Length;

                        if(i < urlParts.segments.Count - 1)
                            separatorAsSpan.CopyTo(output.Slice(offset++));
                    }
                });
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowOutOfMemory() =>
            throw new OutOfMemoryException(
                "Not enough memory to store a path segment. Consider enlarging Arena allocator max size.");

        private void OnDispose()
        {
            Cache.SegmentListCache.Return(_urlPathSegments);
            _arena.Disposed -= OnDispose; //prevent memory leak
        }

        public override string ToString() => GenerateUrl();

        public string ToString(string format, IFormatProvider formatProvider) => GenerateUrl();
    }
}
