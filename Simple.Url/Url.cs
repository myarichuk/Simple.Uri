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

    public struct Url : IFormattable
    {
        private const string UrlPathSeparator = "/";

        private readonly Arena.Arena _arena;
        private readonly (IntPtr ptr, int size) _baseUrl;
        private readonly List<(IntPtr ptr, int size)> _urlPathSegments;

        public readonly Schema Schema;

        /// <exception cref="T:System.ArgumentException">base path must have only HTTPS or HTTP schema</exception>
        public Url(Arena.Arena arena, ReadOnlySpan<char> basePath)
        {
            _arena = arena;
            _urlPathSegments = Cache.SegmentListCache.Get();
            _baseUrl = (basePath.ToIntPtr(), basePath.Length);
            
            if (basePath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                Schema = Schema.Http;
            else if (basePath.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
                Schema = Schema.Https;
            else
                throw new ArgumentException("base path must have only HTTPS or HTTP schema", nameof(basePath));

            _arena.Disposed += OnDispose;
        }

        public void AddPathSegment(ReadOnlySpan<char> pathSegment)
        {
            if(!_arena.TryAllocate<char>(pathSegment.Length, out var storedPathSegment))
                ThrowOutOfMemory();

            pathSegment.CopyTo(storedPathSegment);
            _urlPathSegments.Add((storedPathSegment.ToIntPtr(), pathSegment.Length));
        }

        public string GenerateUrl()
        {
            var length = 
                _baseUrl.size +
                _urlPathSegments.Sum(x => x.size) +  //path size
                _urlPathSegments.Count; //path separators

            return string.Create(length, (_baseUrl, _urlPathSegments), 
                (Span<char> output,
                        ((IntPtr ptr, int size) baseUrl, 
                            List<(IntPtr ptr, int size)> segments) urlParts) =>
                {
                    var offset = 0;
                    var separatorAsSpan = UrlPathSeparator.AsSpan();

                    urlParts.baseUrl.AsSpan().CopyTo(output.Slice(offset));
                    offset += urlParts.baseUrl.size;

                    separatorAsSpan.CopyTo(output.Slice(offset++)); //path separator after base url

                    for (int i = 0; i < urlParts.segments.Count; i++)
                    {
                        var currentSpan = urlParts.segments[i].AsSpan();

                        currentSpan.CopyTo(output.Slice(offset));
                        offset += urlParts.segments[i].size;

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
