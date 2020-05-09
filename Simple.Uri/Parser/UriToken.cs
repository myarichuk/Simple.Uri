using Superpower.Display;

namespace Simple.Uri.Parser
{
    public enum UriToken
    {
        None,

        [Token(Category="delimiter", Example = "/")]
        Slash,

        [Token(Category="delimiter", Example = "//")]
        DoubleSlash,

        [Token(Category="delimiter", Example = ":")]
        Colon,

        [Token(Category="delimiter", Example = "::")]
        DoubleColon,

        [Token(Category="delimiter", Example = "#")]
        Hash,

        [Token(Category="delimiter", Example = ".")]
        Dot,

        [Token(Category="delimiter", Example = "@")]
        At,

        [Token(Category="delimiter", Example = "?")]
        Question,

        [Token(Category="delimiter", Example = "[")]
        LBracket,

        [Token(Category="delimiter", Example = "]")]
        RBracket,

        [Token(Category="param delimiter", Example = ";")]
        Semicolon,

        [Token(Category="param delimiter", Example = "&")]
        Ampersand,

        [Token(Category="param delimiter", Example = "=")]
        Equal,

        [Token(Category = "uri segment")]
        String,

        [Token(Category = "uri segment")]
        Digits,
    }
}
