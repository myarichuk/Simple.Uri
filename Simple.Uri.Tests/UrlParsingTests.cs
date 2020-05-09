using Simple.Uri.Parser;
using Superpower;
using Xunit;

namespace Simple.Uri.Tests
{
    public class UrlParsingTests
    {
        [Fact]
        public void Can_match_encoded_character()
        {
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("ABC").HasValue);
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("%").HasValue);
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("3ABC").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%2f").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%ABC").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%244f").HasValue);
        }

        //[Fact]
        //public void Can_parse_scheme()
        //{
        //    Assert.True(ParserHelper.TryParseSchema("http://foobar.com:1234", out var parseResult, out var errorMessage));
        //    Assert.Equal("http", parseResult.ToString());

        //    Assert.True(ParserHelper.TryParseSchema("xyzxyz://foobar.com:1234", out parseResult, out errorMessage));
        //    Assert.Equal("xyzxyz", parseResult.ToString());
        //}
    }
}
