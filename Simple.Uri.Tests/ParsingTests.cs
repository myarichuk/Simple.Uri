using System.Linq;
using Simple.Uri.Parser;
using Superpower;
using Xunit;
// ReSharper disable ExceptionNotDocumented
// ReSharper disable EventExceptionNotDocumented

namespace Simple.Uri.Tests
{
    public class ParsingTests
    {
        [Fact]
        public void Can_parse_scheme()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar:");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Schema.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);
        }

        [Fact]
        public void Fail_to_parse_scheme_without_delimiter()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Schema.TryParse(tokenizationResult.Value);
            Assert.False(parseResult.HasValue);
        }

        [Fact]
        public void Can_parse_url_with_high_level_api()
        {
            var parseResult = Uri.Parse("foobar://usr:pwd@hostname.com:1234/this/is/a/test?foo=bar&x=y#fragment");
            Assert.Equal("foobar", parseResult.Scheme.ToString());
            Assert.Equal("usr", parseResult.Username.ToString());
            Assert.Equal("pwd", parseResult.Password.ToString());
            Assert.Equal("hostname.com", parseResult.Host.ToString());
            Assert.Equal("1234", parseResult.Port.ToString());

            Assert.Equal(new []{"this", "is", "a", "test"}, parseResult.Path.AsEnumerable());

            Assert.Equal("foo", parseResult.Query[0].Name.ToString());
            Assert.Equal("bar", parseResult.Query[0].Value.ToString());

            Assert.Equal("x", parseResult.Query[1].Name.ToString());
            Assert.Equal("y", parseResult.Query[1].Value.ToString());

            Assert.Equal("fragment", parseResult.Fragment.ToString());
        }

        [Fact]
        public void Can_parse_url()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://usr:pwd@hostname.com:1234/this/is/a/test?foo=bar&x=y#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("usr", parseResult.Value.Authority.Username.ToStringValue());
            Assert.Equal("pwd", parseResult.Value.Authority.Password.ToStringValue());
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.Equal("1234", parseResult.Value.Authority.Port.ToStringValue());

            Assert.Equal(new []{"this", "is", "a", "test"}, parseResult.Value.Path.Select(x => x.ToStringValue()));
            Assert.Equal("foo", parseResult.Value.Query[0].Name.ToStringValue());
            Assert.Equal("bar", parseResult.Value.Query[0].Value.ToStringValue());

            Assert.Equal("x", parseResult.Value.Query[1].Name.ToStringValue());
            Assert.Equal("y", parseResult.Value.Query[1].Value.ToStringValue());

            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }

        [Fact]
        public void Can_parse_url_without_path()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://hostname.com:1234?foo=bar&x=y#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.Equal("1234", parseResult.Value.Authority.Port.ToStringValue());
            Assert.Equal("foo", parseResult.Value.Query[0].Name.ToStringValue());
            Assert.Equal("bar", parseResult.Value.Query[0].Value.ToStringValue());

            Assert.Equal("x", parseResult.Value.Query[1].Name.ToStringValue());
            Assert.Equal("y", parseResult.Value.Query[1].Value.ToStringValue());

            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }

        [Fact]
        public void Can_parse_url_without_path_and_port()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://hostname.com?foo=bar&x=y#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.False(parseResult.Value.Authority.Port.HasValue);
            Assert.Equal("foo", parseResult.Value.Query[0].Name.ToStringValue());
            Assert.Equal("bar", parseResult.Value.Query[0].Value.ToStringValue());

            Assert.Equal("x", parseResult.Value.Query[1].Name.ToStringValue());
            Assert.Equal("y", parseResult.Value.Query[1].Value.ToStringValue());

            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }


        [Fact]
        public void Can_parse_url_without_path_and_port_and_query()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://hostname.com#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }

        [Fact]
        public void Can_parse_url_with_only_hostname()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://hostname.com");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.Null(parseResult.Value.Path);
            Assert.Null(parseResult.Value.Query);
            Assert.False(parseResult.Value.Fragment.HasValue);
        }

        [Fact]
        public void Can_parse_url_without_port()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://hostname.com/this/is/a/test?foo=bar&x=y#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.False(parseResult.Value.Authority.Username.HasValue);
            Assert.False(parseResult.Value.Authority.Password.HasValue);
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.False(parseResult.Value.Authority.Port.HasValue);

            Assert.Equal(new []{"this", "is", "a", "test"}, parseResult.Value.Path.Select(x => x.ToStringValue()));
            Assert.Equal("foo", parseResult.Value.Query[0].Name.ToStringValue());
            Assert.Equal("bar", parseResult.Value.Query[0].Value.ToStringValue());

            Assert.Equal("x", parseResult.Value.Query[1].Name.ToStringValue());
            Assert.Equal("y", parseResult.Value.Query[1].Value.ToStringValue());

            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }

        [Fact]
        public void Can_parse_url_without_pwd()
        {
            var tokenizationResult= UriTokenizer.Instance.TryTokenize("foobar://usr@hostname.com:1234/this/is/a/test?foo=bar&x=y#fragment");
            Assert.True(tokenizationResult.HasValue);

            var parseResult = UriParser.Instance.TryParse(tokenizationResult.Value);
            Assert.True(parseResult.HasValue);

            Assert.Equal("foobar", parseResult.Value.Schema.ToStringValue());
            Assert.Equal("usr", parseResult.Value.Authority.Username.ToStringValue());
            Assert.False(parseResult.Value.Authority.Password.HasValue);
            Assert.Equal("hostname.com", parseResult.Value.Authority.Host.ToStringValue());
            Assert.Equal("1234", parseResult.Value.Authority.Port.ToStringValue());

            Assert.Equal(new []{"this", "is", "a", "test"}, parseResult.Value.Path.Select(x => x.ToStringValue()));
            Assert.Equal("foo", parseResult.Value.Query[0].Name.ToStringValue());
            Assert.Equal("bar", parseResult.Value.Query[0].Value.ToStringValue());

            Assert.Equal("x", parseResult.Value.Query[1].Name.ToStringValue());
            Assert.Equal("y", parseResult.Value.Query[1].Value.ToStringValue());

            Assert.Equal("fragment", parseResult.Value.Fragment.ToStringValue());
        }
    }
}
