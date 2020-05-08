using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Url.Tests
{
    public class UrlHelperTests
    {
        [Fact]
        public void Can_parse_path_segments()
        {
            var url = "http://foobar/this/is/a/test";
            var pathSegments = new List<StringSegment>();
            Assert.True(UrlHelper.TryParsePath(url, pathSegments));

            Assert.Equal(4, pathSegments.Count);

            Assert.Equal(new [] {"this", "is", "a", "test"}, pathSegments.Select(x => x.AsSpan().ToString()));
        }

        [Fact]
        public void Can_parse_path_segments_with_separator_in_middle()
        {
            var url = "http://foobar/this//is//a/test/";
            var pathSegments = new List<StringSegment>();
            Assert.True(UrlHelper.TryParsePath(url, pathSegments));

            Assert.Equal(4, pathSegments.Count);

            Assert.Equal(new [] {"this", "is", "a", "test"}, pathSegments.Select(x => x.AsSpan().ToString()));
        }


        [Fact]
        public void Can_parse_path_segments_with_trailing_separator()
        {
            var url = "http://foobar/this/is/a/test/";
            var pathSegments = new List<StringSegment>();
            Assert.True(UrlHelper.TryParsePath(url, pathSegments));

            Assert.Equal(4, pathSegments.Count);

            Assert.Equal(new [] {"this", "is", "a", "test"}, pathSegments.Select(x => x.AsSpan().ToString()));
        }

        [Fact]
        public void Can_parse_path_segments_with_multiple_trailing_separators()
        {
            var url = "http://foobar/this/is/a/test///";
            var pathSegments = new List<StringSegment>();
            Assert.True(UrlHelper.TryParsePath(url, pathSegments));

            Assert.Equal(4, pathSegments.Count);

            Assert.Equal(new [] {"this", "is", "a", "test"}, pathSegments.Select(x => x.AsSpan().ToString()));
        }

        [Fact]
        public void Can_parse_path_segments_with_query_params()
        {
            var url = "http://foobar/this/is/a/test?foo=bar&bar=foo";
            var pathSegments = new List<StringSegment>();
            Assert.True(UrlHelper.TryParsePath(url, pathSegments));

            Assert.Equal(4, pathSegments.Count);

            Assert.Equal(new [] {"this", "is", "a", "test"}, pathSegments.Select(x => x.AsSpan().ToString()));
        }
    }
}
