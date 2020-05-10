using Superpower.Model;

namespace Simple.Uri.Parser
{
    public struct AuthorityParseResult
    {
        public Token<UriToken> User { get; set; }

        public Token<UriToken> Password { get; set; }

        public Token<UriToken> Host { get;set; }

        public Token<UriToken> Port { get; set; }

        public bool HasResult() => Host.HasValue || User.HasValue || Password.HasValue;
    }
}
