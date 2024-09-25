using Pidgin;
using static Pidgin.Parser;

namespace Parser;

public class FilterParser
{
    static Parser<char, T> Tok<T>(Parser<char, T> p)
        => Try(p).Before(SkipWhitespaces);
    static Parser<char, char> Tok(char value)
        => Tok(Char(value));
    static Parser<char, string> Tok(string value)
        => Tok(String(value));
    static readonly Parser<char, char> _colon = Tok(':');

    static readonly Parser<char, Filter> _filter
        = Map(

        );
    public Result<char, string> ParseFilterText(string filterText)
    {
        Parser<char, string> keyParser = Letter.AtLeastOnceString();
        Parser<char, char> separator = Char(':');

        var combinedParser = Map((key, sep, val) => key + sep + val, keyParser, separator, keyParser);

        var result = combinedParser.Parse(filterText);

        return result;
    }
}
