using Superpower.Display;

namespace Simple.Uri.Parser
{
    public enum UriToken
    {
        None,

        [Token(Category="path delimiter", Example = "/")]
        Slash,

        [Token(Category="delimiter", Example = "//")]
        DoubleSlash,

        [Token(Category="delimiter", Example = ":")]
        Colon,

        [Token(Category="delimiter", Example = "::")]
        DoubleColon,

        [Token(Category="fragment delimiter", Example = "#")]
        Hash,

        [Token(Category="delimiter", Example = ".")]
        Dot,

        [Token(Category="delimiter", Example = "@")]
        At,

        [Token(Category="query delimiter", Example = "?")]
        Question,

        [Token(Category="ipv6 delimiter", Example = "[")]
        LBracket,

        [Token(Category="ipv6 delimiter", Example = "]")]
        RBracket,

        [Token(Category="parameter delimiter", Example = ";")]
        Semicolon,

        [Token(Category="parameter delimiter", Example = "&")]
        Ampersand,

        [Token(Category="parameter delimiter", Example = "=")]
        Equal,

        [Token(Category = "string")]
        String,

        [Token(Category = "digit")]
        Digits,

        [Token(Category = "whitespace")]
        Whitespace,
    }
}
