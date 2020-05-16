using System;

namespace Simple.Uri
{
    public partial class UriParser
    {
        public bool TryFindFirstOccurenceOf(ReadOnlySpan<char> uriSegment, ReadOnlySpan<char> token, StringComparison comparisonType, out int index)
        {
            index = uriSegment.IndexOf(token, comparisonType);            
            return index >= 0;
        }

        public bool TryFindFirstOccurenceOf(ReadOnlySpan<char> uriSegment, ReadOnlySpan<char> token, out int index)
        {
            index = uriSegment.IndexOf(token);            
            return index >= 0;
        }
    }
}
