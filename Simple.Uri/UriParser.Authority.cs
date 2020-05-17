using System;

namespace Simple.Uri
{
    //URI = scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public partial class UriParser
    {
        private const string AuthorityDelimiterToken = "//";
        private const string AuthorityEndTokenA = "/";
        private const string AuthorityEndTokenB = "?";

        private const string SegmentDelimiterToken = ":";
        private const string UsernamePasswordEndToken = "@";

        public (bool success, ReadOnlyMemory<char> error) TryParseAuthority(ReadOnlySpan<char> uri, out int authorityEndIndex, ref UriInfo uriInfo)
        {
            authorityEndIndex = -1;
            if (!TryFindFirstOccurenceOf(uri, AuthorityDelimiterToken, out var authorityStartIndex))
                return (false, default);

            ReadOnlySpan<char> authoritySlice;
            var uriWithoutScheme = uri.Slice(authorityStartIndex + AuthorityDelimiterToken.Length);
            if (TryFindFirstOccurenceOf(uriWithoutScheme, AuthorityEndTokenA, out authorityEndIndex) ||
                TryFindFirstOccurenceOf(uriWithoutScheme, AuthorityEndTokenB, out authorityEndIndex))
                authoritySlice = uriWithoutScheme.Slice(0, authorityEndIndex);
            else
            {
                authoritySlice = uriWithoutScheme;
                authorityEndIndex = uriWithoutScheme.Length;
            }

            uriInfo.Authority = authoritySlice;

            ActuallyParseAuthority(authoritySlice, ref uriInfo);
            authorityEndIndex += authorityStartIndex + AuthorityDelimiterToken.Length; //make it absolute
            return (true, default);
        }

        private void ActuallyParseAuthority(ReadOnlySpan<char> authoritySlice, ref UriInfo uriInfo)
        {
            ParseAuthenticationSegment(authoritySlice, out var authenticationEndTokenIndex, ref uriInfo);
            ParseHostAndPortsSegment(authoritySlice, authenticationEndTokenIndex, ref uriInfo);
        }

        private void ParseHostAndPortsSegment(ReadOnlySpan<char> authoritySlice, int authenticationEndTokenIndex, ref UriInfo uriInfo)
        {
            ReadOnlySpan<char> hostAndPortSlice;
            if (authenticationEndTokenIndex > 0)
            {
                hostAndPortSlice = authoritySlice.Slice(authenticationEndTokenIndex);
            }
            else
            {
                hostAndPortSlice = authoritySlice;
            }

            if (TryFindFirstOccurenceOf(hostAndPortSlice, SegmentDelimiterToken, out var hostAndPortDelimiterIndex))
            {
                uriInfo.Host = hostAndPortSlice.Slice(0, hostAndPortDelimiterIndex);
                uriInfo.Port = hostAndPortSlice.Slice(hostAndPortDelimiterIndex + SegmentDelimiterToken.Length);
            }
            else
            {
                uriInfo.Host = hostAndPortSlice;
            }
        }

        private void ParseAuthenticationSegment(ReadOnlySpan<char> authoritySlice,out int authenticationEndToken, ref UriInfo authority)
        {
            if (TryFindFirstOccurenceOf(authoritySlice, UsernamePasswordEndToken, out authenticationEndToken))
            {
                //parse username password....
                var authenticationSlice = authoritySlice.Slice(0, authenticationEndToken);
                if (TryFindFirstOccurenceOf(authenticationSlice, SegmentDelimiterToken, out var delimiterTokenIndex))
                {
                    authority.Username = authenticationSlice.Slice(0, delimiterTokenIndex);
                    authority.Password = authenticationSlice.Slice(delimiterTokenIndex + SegmentDelimiterToken.Length);
                }
                else //we have only username...
                {
                    authority.Username = authenticationSlice;
                }
                authenticationEndToken += UsernamePasswordEndToken.Length;
            }
        }

        public ref struct Authority
        {
            public ReadOnlySpan<char> Username { get; set; }

            public ReadOnlySpan<char> Password { get; set; }

            public ReadOnlySpan<char> Host { get; set; }

            public ReadOnlySpan<char> Port { get; set; }
        }
    }
}
