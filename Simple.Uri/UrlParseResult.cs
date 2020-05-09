using System;

namespace Simple.Uri
{
    //URI = scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public ref struct UriParseResult
    {
        public ReadOnlySpan<char> Scheme { get; set; }

        ///////////
        //authority part
        public ReadOnlySpan<char> UserInfo { get; set; }

        public ReadOnlySpan<char> Host { get; set; }

        public ReadOnlySpan<char> Port { get; set; }
        ///////////
        
        public ReadOnlySpan<char> Path { get; set; }

        public ReadOnlySpan<char> Query { get; set; }

        public ReadOnlySpan<char> Fragment { get; set; }
    }
}
