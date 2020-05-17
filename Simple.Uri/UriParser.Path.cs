using System;

namespace Simple.Uri
{
    public partial class UriParser
    {
        private const string PathSeparatorToken = "/";
        private const string FragmentDelimiter = "#";
        private const string QueryDelimiter = "?";

        //assuming restOfUri is a slice of the URI after authority
        public (bool success, ReadOnlyMemory<char> error) TryParsePath(ReadOnlySpan<char> uri, int authorityEndIndex, out int pathEndIndex, ref UriInfo uriInfo)
        {
            var restOfUri = uri.Slice(authorityEndIndex);
            ReadOnlySpan<char> pathSlice;
            if(TryFindFirstOccurenceOf(restOfUri, QueryDelimiter, out pathEndIndex) ||
                TryFindFirstOccurenceOf(restOfUri, FragmentDelimiter, out pathEndIndex))
            {
                pathSlice = restOfUri.Slice(PathSeparatorToken.Length, pathEndIndex - 1); //both query delimiter and fragment delimiter have size = 1
            }
            else
                pathSlice = restOfUri.Slice(PathSeparatorToken.Length);

            if(!pathSlice.IsEmpty && pathSlice[^1] == PathSeparatorToken[0]) //ensure there is no trailing slashes
                pathSlice = pathSlice[0 .. ^1];

            uriInfo.Path = pathSlice;

            return (true, default);
        }
    }
}
