using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Simple.Url
{
    public enum Schema
    {
        Unsupported,
        Http,
        Https
    }

    public struct Url : IFormattable
    {
        private const string UrlPathSeparator = "/";
        private const string QueryParamSeparator = "&";

        private readonly Arena.Arena _arena;
        private readonly StringSegment _baseUrl;
        private readonly List<StringSegment> _pathSegments;

        public readonly List<(StringSegment Name, StringSegment Value)> _queryParameters;

        public readonly Schema Schema;

        public IReadOnlyList<StringSegment> Path => _pathSegments;

        /// <exception cref="T:System.ArgumentException">base path must have only HTTPS or HTTP schema</exception>
        public Url(Arena.Arena arena, ReadOnlySpan<char> fullUrl)
        {
            _arena = arena ?? throw new ArgumentNullException(nameof(arena));
            if(fullUrl.IsEmpty || fullUrl.IsWhiteSpace())
                throw new ArgumentNullException(nameof(fullUrl));

            if(_arena.IsDisposed)
                throw new ObjectDisposedException(nameof(arena));

            _pathSegments = Cache.PathSegmentListCache.Get();
            _queryParameters = Cache.QueryParameterListCache.Get();
            _baseUrl = fullUrl;
            
            if(!UrlHelper.TryParseSchema(fullUrl, out var parsedSchema))
                throw new ArgumentException("base path must have only HTTPS or HTTP schema", nameof(fullUrl));

            Schema = parsedSchema;

            if(!UrlHelper.TryParsePath(fullUrl, _pathSegments))
                throw new ArgumentException("Failed to parse url path");

            _arena.OnReset += OnDispose;
            _arena.OnDispose += OnDispose;
        }

        public void AddPathSegment(ReadOnlySpan<char> pathSegment)
        {
            if(!_arena.TryAllocate<char>(pathSegment.Length, out var storedPathSegment))
                ThrowOutOfMemory();

            pathSegment.CopyTo(storedPathSegment);
            _pathSegments.Add(storedPathSegment.AsStringSegment());
        }

        public string AsString()
        {
            var length = 
                _baseUrl.Length +  //path size
                _pathSegments.Count; //path separators

            //note: this can be done with Linq's Sum(), but that will do delegate allocation
            for(int i = 0; i < _pathSegments.Count; i++)
                length += _pathSegments[i].Length;

            return string.Create(length, (_baseUrl, _pathSegments, _queryParameters), 
                (Span<char> output,
                        (StringSegment baseUrl, 
                         List<StringSegment> segments,
                         List<(StringSegment Name, StringSegment Value)> queryParams) urlParts) =>
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
            Cache.PathSegmentListCache.Return(_pathSegments);
            Cache.QueryParameterListCache.Return(_queryParameters);

            _arena.OnReset -= OnDispose; //prevent memory leak
            _arena.OnDispose -= OnDispose;
        }

        public static implicit operator string(Url url) => url.AsString();

        public override string ToString() => AsString();

        public string ToString(string format, IFormatProvider formatProvider) => AsString();
    }
}
