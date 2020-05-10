using System;
using System.Text.RegularExpressions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;
// ReSharper disable EventExceptionNotDocumented

namespace Simple.Uri.Parser
{
    public static class UriTokenizer
    {
        //('%' [a-fA-F0-9] [a-fA-F0-9]) +
        public static TextParser<Unit> EncodedCharacter { get; } =
            Character.EqualTo('%').Value(Unit.Value)
                .IgnoreThen(Character.HexDigit.AtLeastOnce()).Value(Unit.Value);

        //([a-zA-Z~0-9] | HEX) ([a-zA-Z0-9.+-] | HEX)*
        public static TextParser<Unit> String { get; } =
            from startChar in Character.Letter.Value(Unit.Value).Or(EncodedCharacter)
            from @char in Character.LetterOrDigit.Value(Unit.Value)
                                    .Or(Character.In('.', '+', '-').Value(Unit.Value))
                                    .Or(EncodedCharacter)
                                    .IgnoreMany()
            select Unit.Value;

        public static Tokenizer<UriToken> Instance { get; } = 
            new TokenizerBuilder<UriToken>()
                .Match(Span.WhiteSpace, UriToken.Whitespace)
                .Match(Span.EqualTo('='), UriToken.Equal)
                .Match(Span.EqualTo('#'), UriToken.Hash)
                .Match(Span.EqualTo('@'), UriToken.At)
                .Match(Span.EqualTo('?'), UriToken.Question)
                .Match(Span.EqualTo('['), UriToken.LBracket)
                .Match(Span.EqualTo(']'), UriToken.RBracket)
                .Match(Span.EqualTo(';'), UriToken.Semicolon)
                .Match(Span.EqualTo('&'), UriToken.Ampersand)
                .Match(Span.EqualTo('.'), UriToken.Dot)
                .Match(Span.EqualTo("//"), UriToken.DoubleSlash)
                .Match(Span.EqualTo('/'), UriToken.Slash)
                .Match(Span.EqualTo("::"), UriToken.DoubleColon)
                .Match(Span.EqualTo(':'), UriToken.Colon)
                .Match(Numerics.Natural, UriToken.Digits)
                .Match(String, UriToken.String)
            .Build();
    }
}
