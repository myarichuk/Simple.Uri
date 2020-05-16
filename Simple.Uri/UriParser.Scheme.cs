using System;

namespace Simple.Uri
{
    //URI = scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public partial class UriParser
    {
        private const string SchemaEmptyError = "Invalid URI: schema should not be empty";
        private const string SchemaDelimiterNotFoundError = "Invalid URI: schema delimiter not found (delimiter is ':')";

        public (bool success, ReadOnlyMemory<char> error) TryParseScheme(ReadOnlySpan<char> uri, out ReadOnlySpan<char> scheme)
        {
            scheme = default;

            int colonIndex = -1;
            for(int i = 0; i < uri.Length; i++)
            {
                if(uri[i] == ':')
                {
                    colonIndex = i;
                    break;
                }
            }

            if(colonIndex == -1)
                return (false, SchemaDelimiterNotFoundError.AsMemory());

            if (colonIndex == 0)
                return (false, SchemaEmptyError.AsMemory());

            scheme = uri.Slice(0, colonIndex);

            return (true, default);
        }
    }
}
