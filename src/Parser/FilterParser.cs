using Pidgin;
using static Pidgin.Parser;

namespace Parser;

public class FilterParser
{
    static Parser<char, T> Tok<T>
    public Result<char, string> ParseFilterText(string filterText)
    {
        Parser<char, string> keyParser = Letter.AtLeastOnceString();
        Parser<char, char> separator = Char(':');

        var combinedParser = Map((key, sep, val) => key + sep + val, keyParser, separator, keyParser);

        var result = combinedParser.Parse(filterText);

        return result;
    }
}
