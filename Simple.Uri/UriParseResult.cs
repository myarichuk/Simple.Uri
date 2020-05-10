using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Uri.Parser;
using Superpower.Model;

namespace Simple.Uri
{
    public ref struct PathSegmentCollection
    {
        private readonly Token<UriToken>[] _pathElements;

        public int Count => _pathElements.Length;

        public ReadOnlySpan<char> this[int index] => _pathElements[index].ToSpan();

        internal PathSegmentCollection(Token<UriToken>[] pathElements) => _pathElements = pathElements;

        public IEnumerable<string> AsEnumerable() => _pathElements.Select(x => x.Span.ToStringValue());
    }

    public ref struct QueryParameter
    {
        public ReadOnlySpan<char> Name { get; set; }

        public ReadOnlySpan<char> Value { get; set; }
    }

    public ref struct QueryParameterCollection
    {
        private readonly (Token<UriToken> Name, Token<UriToken> Value)[] _params;

        public QueryParameterCollection((Token<UriToken> Name, Token<UriToken> Value)[] @params) => _params = @params;

        public int Count => _params.Length;

        public QueryParameter this[int index] =>
            new QueryParameter
            {
                Name = _params[index].Name.ToSpan(),
                Value = _params[index].Value.ToSpan()
            };

        public IEnumerable<(string Name, string Value)> AsEnumerable() =>
            _params.Select(x => (x.Name.ToStringValue(), x.Value.ToStringValue()));
    }

    //URI = scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public ref struct UriParseResult
    {
        public ReadOnlySpan<char> Scheme { get; set; }

        ///////////
        //authority part
        public ReadOnlySpan<char> Username { get; set; }

        public ReadOnlySpan<char> Password { get; set; }

        public ReadOnlySpan<char> Host { get; set; }

        public ReadOnlySpan<char> Port { get; set; }
        ///////////
        
        public PathSegmentCollection Path { get; set; }

        public QueryParameterCollection Query { get; set; }

        public ReadOnlySpan<char> Fragment { get; set; }
    }
}
