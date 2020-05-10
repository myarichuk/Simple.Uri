using Superpower.Model;

namespace Simple.Uri.Parser
{
    public struct RawUriParseResult
    {
        public Token<UriToken> Schema { get; set; }

        public AuthorityParseResult Authority { get; set; }

        public Token<UriToken>[] Path { get; set; }

        public (Token<UriToken> Name, Token<UriToken> Value)[] Query { get; set; }

        public Token<UriToken> Fragment { get; set; }
    }
}