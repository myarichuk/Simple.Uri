using System;
using System.Linq;
using Simple.Uri.Parser;
using Superpower;
using UriParser = Simple.Uri.Parser.UriParser;

namespace Simple.Uri
{

    //scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
    public static class Uri
    {
        /// <exception cref="T:System.InvalidOperationException">Failed to tokenize the uri.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="uri"/> is <see langword="null"/></exception>
        /// <exception cref="T:Superpower.ParseException">The tokenizer could not correctly perform tokenization.</exception>
        public static UriParseResult Parse(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri)) 
                throw new ArgumentNullException(nameof(uri));

            var tokenizeResult = UriTokenizer.Instance.TryTokenize(uri);
            if (!tokenizeResult.HasValue)
                throw new InvalidOperationException(tokenizeResult.ErrorMessage);


            var parseResult = UriParser.Instance.TryParse(tokenizeResult.Value);
            if (!parseResult.HasValue)
                throw new InvalidOperationException(parseResult.ErrorMessage);

            return new UriParseResult
            {
                Scheme = parseResult.Value.Schema.ToSpan(),
                Host = parseResult.Value.Authority.Host.ToSpan(),
                Port = parseResult.Value.Authority.Port.ToSpan(),
                Username = parseResult.Value.Authority.Username.ToSpan(),
                Password = parseResult.Value.Authority.Password.ToSpan(),
                Path = new PathSegmentCollection(parseResult.Value.Path),
                Query = new QueryParameterCollection(parseResult.Value.Query),
                Fragment = parseResult.Value.Fragment.ToSpan()
            };
        }

    }
}
