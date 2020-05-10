using Superpower;
using Superpower.Model;
using Superpower.Parsers;
// ReSharper disable EventExceptionNotDocumented

namespace Simple.Uri.Parser
{
    /*
     * scheme:[//[user:password@]host[:port]][/]path[?query][#fragment]
     */
    public static class UriParser
    {
        // -> [schema]:
        public static TokenListParser<UriToken, Token<UriToken>> Schema { get; } =
            from scheme in Token.EqualTo(UriToken.String)
            from ending in Token.EqualTo(UriToken.Colon)
            select scheme;

        #region Authority

        public static TokenListParser<UriToken, (Token<UriToken> user, Token<UriToken> password)> UserInfo { get; } =
            from user in Token.EqualTo(UriToken.String)
            from _ in Token.EqualTo(UriToken.Colon)
            from password in Token.EqualTo(UriToken.String).OptionalOrDefault()
            from __ in Token.EqualTo(UriToken.At)
            select (user, password);

        public static TokenListParser<UriToken, (Token<UriToken> user, Token<UriToken> password)> UserInfoWithoutPassword { get; } =
            from user in Token.EqualTo(UriToken.String)
            from _ in Token.EqualTo(UriToken.At)
            select (user, default(Token<UriToken>));

        public static TokenListParser<UriToken, AuthorityParseResult> Authority { get; } =
            from begin in Token.EqualTo(UriToken.DoubleSlash)
            from userInfo in UserInfo.Try()
                        .Or(UserInfoWithoutPassword.Try()).OptionalOrDefault()
            from host in Token.EqualTo(UriToken.String)
            from port in Token.EqualTo(UriToken.Colon)
                                           .IgnoreThen(Token.EqualTo(UriToken.Digits))
                                           .OptionalOrDefault().Try()
            select new AuthorityParseResult
            {
                User = userInfo.user,
                Password = userInfo.password,
                Host = host,
                Port = port
            };

        #endregion

        public static TokenListParser<UriToken, Token<UriToken>[]> Path { get; } =
            from _ in Token.EqualTo(UriToken.Slash).Optional()
            from path in Token.EqualTo(UriToken.String).AtLeastOnceDelimitedBy(Token.EqualTo(UriToken.Slash))
            select path;

        #region Query

        public static TokenListParser<UriToken, (Token<UriToken> Name, Token<UriToken> Value)> QueryParameter { get; } =
            from name in Token.EqualTo(UriToken.String)
            from _ in Token.EqualTo(UriToken.Equal)
            from value in Token.EqualTo(UriToken.String)
            select (name, value);

        public static TokenListParser<UriToken, (Token<UriToken> Name, Token<UriToken> Value)[]> Query { get; } =
            from _ in Token.EqualTo(UriToken.Question)
            from @params in QueryParameter.AtLeastOnceDelimitedBy(Token.EqualTo(UriToken.Ampersand))
                                        .Try()
                                        .Or(QueryParameter.AtLeastOnceDelimitedBy(Token.EqualTo(UriToken.Semicolon)))
            select @params;

        #endregion

        public static TokenListParser<UriToken, Token<UriToken>> Fragment { get; } =
            from _ in Token.EqualTo(UriToken.Hash)
            from fragment in Token.EqualTo(UriToken.String)
            select fragment;

        public static TokenListParser<UriToken, RawUriParseResult> Instance { get; } =
            from schema in Schema
            from authority in Authority.OptionalOrDefault()
            from path in Path.OptionalOrDefault()
            from query in Query.OptionalOrDefault()
            from fragment in Fragment.OptionalOrDefault()
            select new RawUriParseResult
            {
                Schema = schema,
                Authority = authority,
                Path = path,
                Query = query,
                Fragment = fragment
            };
    }
}
