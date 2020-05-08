using System;
using System.Collections.Generic;

namespace Simple.Url
{
    public static class UrlHelper
    {
        private readonly static int HttpPrefixLength = "http://".Length;
        private readonly static int HttpsPrefixLength = "https://".Length;

        public static bool TryParseSchema(ReadOnlySpan<char> baseUrl, out Schema schema)
        {
            schema = Schema.Unsupported;

            if (baseUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                schema = Schema.Http;
            else if (baseUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                schema = Schema.Https;
            else
                return false;

            return true;
        }

        public static bool TryParsePath(ReadOnlySpan<char> fullUrl, List<StringSegment> results)
        {
            int skip;
            if (fullUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                skip = HttpPrefixLength;
            else if (fullUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                skip = HttpsPrefixLength;
            else 
                return false;

            for(int i = skip; i < fullUrl.Length; i++)
            {
                skip++;
                if(fullUrl[i] == '/')
                    break;
            }       

            var begin = skip;
            for(int i = skip; i < fullUrl.Length; i++)
            {
                if(fullUrl[i] == '/')
                {
                    var end = i;
                    var segment = fullUrl.Slice(begin, end - begin);
                    AddToResult(results, segment);

                    begin = i + 1;
                }

                if (fullUrl[i] == '?' || i == fullUrl.Length - 1) //we have reached query parameters...
                {
                    var segment = 
                        fullUrl[i] == '?' ? 
                            fullUrl.Slice(begin, i - begin) : 
                            fullUrl.Slice(begin);

                    AddToResult(results, segment);

                    break;
                }
            }

            return true;

            static void AddToResult(List<StringSegment> results, ReadOnlySpan<char> segment)
            {
                if(!segment.IsEmpty)
                    results.Add(segment.AsStringSegment());
            }
        }
    }
}
