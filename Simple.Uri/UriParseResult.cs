using System;

namespace Simple.Uri
{
    //URI = scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]

    public ref struct UriInfo
    {
        public ReadOnlySpan<char> Scheme { get; set; }

        ///////////
        //authority part
        public ReadOnlySpan<char> Username { get; set; }

        public ReadOnlySpan<char> Password { get; set; }

        public ReadOnlySpan<char> Host { get; set; }

        public ReadOnlySpan<char> Port { get; set; }
        ///////////
        
        public ReadOnlySpan<Memory<char>> Path { get; set; }

        public ReadOnlySpan<(Memory<char> Name, Memory<char> Value)> Query { get; set; }

        public ReadOnlySpan<char> Fragment { get; set; }
    }
}
