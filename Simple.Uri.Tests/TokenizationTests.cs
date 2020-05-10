using Simple.Uri.Parser;
using Superpower;
using Xunit;

namespace Simple.Uri.Tests
{
    public class TokenizationTests
    {
        [Fact]
        public void Can_tokenize_encoded_character()
        {
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("ABC").HasValue);
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("%").HasValue);
            Assert.False(UriTokenizer.EncodedCharacter.TryParse("3ABC").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%2f").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%ABC").HasValue);
            Assert.True(UriTokenizer.EncodedCharacter.TryParse("%244f").HasValue);
        }

        [Fact]
        public void Can_tokenize_string()
        {
            var res = UriTokenizer.String.TryParse("123ABC");
            Assert.False(UriTokenizer.String.TryParse("123ABC").HasValue);
            Assert.False(UriTokenizer.String.TryParse("123ABC:").HasValue);
            Assert.False(UriTokenizer.String.TryParse("123:ABC").HasValue);
            Assert.True(UriTokenizer.String.TryParse("A12BC12").HasValue);
            Assert.True(UriTokenizer.String.TryParse("A%12BC12").HasValue);
        }
    }
}
