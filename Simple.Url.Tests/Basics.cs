using System;
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
            var builder = new UrlBuilder(arena, "https://foobar.com");

            Assert.Equal("https://foobar.com", builder.GenerateUrl());
            Assert.Equal(Schema.Https, builder.Schema);
        }

        [Fact]
        public void Can_concat_one_path_segment()
        {
            using var arena = new Arena.Arena();
            var builder = new UrlBuilder(arena, "http://foobar.com");

            builder.AddPathSegment("Baz");

            Assert.Equal("http://foobar.com/Baz", builder.GenerateUrl());
            Assert.Equal(Schema.Http, builder.Schema);
        }


        [Fact]
        public void Can_concat_multiple_path_segments()
        {
            using var arena = new Arena.Arena();
            var builder = new UrlBuilder(arena, "http://foobar.com");

            builder.AddPathSegment("Foo");
            builder.AddPathSegment("Bar");
            builder.AddPathSegment("Baz");

            Assert.Equal("http://foobar.com/Foo/Bar/Baz", builder.GenerateUrl());
            Assert.Equal(Schema.Http, builder.Schema);
        }

        [Fact]
        public void Should_throw_on_null_arena() => Assert.Throws<ArgumentNullException>(() => new UrlBuilder(null, ""));

        [Fact]
        public void Should_throw_on_empty_base_url()
        {
            using var arena = new Arena.Arena();
            Assert.Throws<ArgumentNullException>(() => new UrlBuilder(arena, ""));
        }

        [Fact]
        public void Should_throw_on_null_base_url()
        {
            using var arena = new Arena.Arena();
            Assert.Throws<ArgumentNullException>(() => new UrlBuilder(arena, null));
        }
    }
}
