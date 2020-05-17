using System;
namespace Simple.Uri
{

    //scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public ref struct Uri
    {
        private UriInfo UriInfo { get; set; }
        private readonly static UriParser Parser = new UriParser();

        /// <exception cref="T:System.InvalidOperationException">Failed to tokenize the uri.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="uri"/> is <see langword="null"/></exception>
        /// <exception cref="T:Superpower.ParseException">The tokenizer could not correctly perform tokenization.</exception>
        public static UriInfo Parse(ReadOnlySpan<char> uri)
        {
            if (uri.IsEmpty || uri.IsWhiteSpace()) 
                throw new ArgumentNullException(nameof(uri));

            UriInfo uriInfo = default;

            var result = Parser.TryParseScheme(uri, out var scheme);
            if(!result.success && !result.error.IsEmpty)
                throw new InvalidOperationException(result.error.ToString());

            uriInfo.Scheme = scheme;
            
            result = Parser.TryParseAuthority(uri, out var authorityEndIndex, ref uriInfo);
            if(!result.success && !result.error.IsEmpty)
                throw new InvalidOperationException(result.error.ToString());


            result = Parser.TryParsePath(uri, authorityEndIndex, out var pathEndIndex, ref uriInfo);
            if(!result.success && !result.error.IsEmpty)
                throw new InvalidOperationException(result.error.ToString());

            //TODO: add verification that there are no invalid characters at paths

            return uriInfo;
        }

    }
}
