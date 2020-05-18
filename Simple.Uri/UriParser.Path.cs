using System;
using System.Collections.Generic;

namespace Simple.Uri
{
    public partial class UriParser
    {
        private const string PathSeparatorToken = "/";
        private const string FragmentDelimiter = "#";
        private const string QueryDelimiter = "?";

        //assuming restOfUri is a slice of the URI after authority
        public (bool success, ReadOnlyMemory<char> error, ReadOnlyMemory<char> pathAsMemory) TryParsePath(ReadOnlyMemory<char> uri, int authorityEndIndex, out int pathEndIndex, ref UriInfo uriInfo)
        {
            
            var restOfUri = uri.Slice(authorityEndIndex);
            ReadOnlyMemory<char> pathSlice;
            if(TryFindFirstOccurenceOf(restOfUri, QueryDelimiter, out pathEndIndex) ||
                TryFindFirstOccurenceOf(restOfUri, FragmentDelimiter, out pathEndIndex))
            {
                pathSlice = restOfUri.Slice(PathSeparatorToken.Length, pathEndIndex - 1); //both query delimiter and fragment delimiter have size = 1
            }
            else
                pathSlice = restOfUri.Slice(PathSeparatorToken.Length);

            if(!pathSlice.IsEmpty && pathSlice.Span[^1] == PathSeparatorToken[0]) //ensure there is no trailing slashes
                pathSlice = pathSlice[0 .. ^1];

            uriInfo.Path = pathSlice.Span;

            return (true, default, pathSlice);
        }

        public IEnumerable<ReadOnlyMemory<char>> ParsePathIntoCollection(ReadOnlyMemory<char> pathSlice)
        {
            var current = pathSlice;
            while(TryFindFirstOccurenceOf(current, PathSeparatorToken, out var separatorIndex))
            {
                var pathSegment = current.Slice(0, separatorIndex);
                current = current.Slice(separatorIndex + PathSeparatorToken.Length);
                
                yield return pathSegment;
            }

            yield return current;
        }
    }
}
