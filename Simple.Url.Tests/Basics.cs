using System;
using Xunit;

namespace Simple.Url.Tests
{
    public class Basics
    {
        [Fact]
        public void Can_concat_path_segments()
        {
            using var arena = new Arena.Arena();
            var url = new Url(arena, "http://foobar.com");

            url.AddPathSegment("Foo");
            url.AddPathSegment("Bar");
            url.AddPathSegment("Baz");

            Assert.Equal("http://foobar.com/Foo/Bar/Baz", url.GenerateUrl());
        }
    }
}
