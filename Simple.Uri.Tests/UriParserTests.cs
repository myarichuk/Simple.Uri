using Xunit;

namespace Simple.Uri.Tests
{
    public class UriParserTests
    {
        private readonly UriParser _parser = new UriParser();

        [Fact]
        public void Can_parse_scheme()
        {
            var (success, _) = _parser.TryParseScheme("http://foobar", out var scheme);
            Assert.True(success);
            Assert.Equal("http", scheme.ToString());
        }

        [Fact]
        public void Should_fail_if_schema_is_empty()
        {
            var (success, _) = _parser.TryParseScheme("://foobar", out _);
            Assert.False(success);
        }

        [Fact]
        public void Should_fail_if_schema_is_delimiter_missing()
        {
            var (success, _) = _parser.TryParseScheme("http//foobar", out _);
            Assert.False(success);
        }

        //happy path...
        [Fact]
        public void Should_parse_authority_correctly_when_uri_has_path()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://john:1234@foobar.com:321/path/foo/bar?x=z", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("john", uriInfo.Username.ToString());
            Assert.Equal("1234", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }

        [Fact]
        public void Should_parse_authority_without_username_password_correctly_when_uri_has_path()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://foobar.com:321/path/foo/bar?x=z", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("", uriInfo.Username.ToString());
            Assert.Equal("", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }

        [Fact]
        public void Should_parse_authority_without_username_password_correctly_when_uri_has_no_path()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://foobar.com:321", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("", uriInfo.Username.ToString());
            Assert.Equal("", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }


        [Fact]
        public void Should_parse_authority_correctly_when_uri_has_no_path()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://john:1234@foobar.com:321", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("john", uriInfo.Username.ToString());
            Assert.Equal("1234", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }

        [Fact]
        public void Should_parse_authority_correctly_when_uri_has_ending_slash()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://john:1234@foobar.com:321/", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("john", uriInfo.Username.ToString());
            Assert.Equal("1234", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }


        [Fact]
        public void Should_parse_authority_correctly_when_only_username_is_in_authority_segment()
        {
            UriInfo uriInfo = default;
            var (success, _) = _parser.TryParseAuthority("http://john@foobar.com:321?foobar=xyz", ref uriInfo);

            Assert.True(success); //sanity check

            Assert.Equal("john", uriInfo.Username.ToString());
            Assert.Equal("", uriInfo.Password.ToString());
            Assert.Equal("foobar.com", uriInfo.Host.ToString());
            Assert.Equal("321", uriInfo.Port.ToString());
        }
    }
}
