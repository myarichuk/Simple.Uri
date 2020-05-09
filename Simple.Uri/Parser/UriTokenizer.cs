using System;
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
        public static TextParser<TextSpan> EncodedCharacter { get; } = 
            Span.MatchedBy(Span.EqualTo('%').IgnoreThen(Character.HexDigit));

        //([a-zA-Z~0-9] | HEX) ([a-zA-Z0-9.+-] | HEX)*
        public static TextParser<TextSpan> String { get; } =
            Span.MatchedBy(Character.LetterOrDigit.Select(c => new TextSpan())
                .IgnoreThen(Character.LetterOrDigit.Select(x => new TextSpan())
                    .Or(Character.In('.','+','-').Select(x => new TextSpan())
                            .Or(EncodedCharacter.Select(x => new TextSpan())))).IgnoreMany());

        public static Tokenizer<UriToken> Instance { get; } = 
            new TokenizerBuilder<UriToken>()
                .Match(Character.EqualTo('='), UriToken.Equal)
                .Match(Character.EqualTo('#'), UriToken.Hash)
                .Match(Character.EqualTo('@'), UriToken.At)
                .Match(Character.EqualTo('?'), UriToken.Question)
                .Match(Character.EqualTo('['), UriToken.LBracket)
                .Match(Character.EqualTo(']'), UriToken.RBracket)
                .Match(Character.EqualTo(';'), UriToken.Semicolon)
                .Match(Character.EqualTo('&'), UriToken.Ampersand)
                .Match(Character.EqualTo('.'), UriToken.Dot)
                .Match(Span.EqualTo("//"), UriToken.DoubleSlash)
                .Match(Character.EqualTo('/'), UriToken.Slash)
                .Match(Span.EqualTo("::"), UriToken.DoubleColon)
                .Match(Character.EqualTo(':'), UriToken.Colon)
                .Match(Numerics.Integer, UriToken.Digits)
                .Match(String, UriToken.String, true)
            .Build();
    }
}
