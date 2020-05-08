using System;
using System.Linq;
using Xunit;

namespace Simple.Url.Tests
{
    public class Basics
    {
        [Fact]
        public void Span_conversion_to_StringSegment_should_work()
        {
            var origin = "Test 123456".AsSpan();
            var originCopy = "Test 123456".AsSpan();

            var stringSegment = origin.AsStringSegment();
            var spanFromStringSegment = stringSegment.AsSpan();

            Assert.Equal(originCopy.ToString(), spanFromStringSegment.ToString());
        }

        [Fact]
        public void Can_create_url_only_with_base()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "https://foobar.com");

            Assert.Equal("https://foobar.com", url.AsString());
            Assert.Equal(Schema.Https, url.Schema);
        }

        [Fact]
        public void Can_concat_one_path_segment()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "http://foobar.com");

            url.AddPathSegment("Baz");

            Assert.Equal("http://foobar.com/Baz", url.AsString());
            Assert.Equal(Schema.Http, url.Schema);
        }


        [Fact]
        public void Can_concat_multiple_path_segments()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "http://foobar.com");

            url.AddPathSegment("Foo");
            url.AddPathSegment("Bar");
            url.AddPathSegment("Baz");

            Assert.Equal("http://foobar.com/Foo/Bar/Baz", url.AsString());
            Assert.Equal(Schema.Http, url.Schema);
        }

        [Fact]
        public void Should_throw_on_null_arena() => Assert.Throws<ArgumentNullException>(() => new Url(null, ""));

        [Fact]
        public void Should_throw_on_empty_base_url()
        {
            using var arena = new Arena.Arena();
            Assert.Throws<ArgumentNullException>(() => new Url(arena, ""));
        }

        [Fact]
        public void Can_parse_path()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "http://foobar.com/foobar/bar/baz/test");

            Assert.Equal(new [] { "foobar", "bar", "baz", "test" }, url.Path.Select(x => x.ToString()));
        }

        [Fact]
        public void Can_add_to_parsed_path()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "http://foobar.com/foobar/bar/baz/test");
            url.AddPathSegment("xyz");
            url.AddPathSegment("test123");

            Assert.Equal(new [] { "foobar", "bar", "baz", "test", "xyz", "test123" }, url.Path.Select(x => x.ToString()));
        }


        [Fact]
        public void Should_throw_on_null_base_url()
        {
            using var arena = new Arena.Arena();
            Assert.Throws<ArgumentNullException>(() => new Url(arena, null));
        }
    }
}
