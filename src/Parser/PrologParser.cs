using System.Collections.Immutable;
using Pidgin;
using static Pidgin.Parser;

namespace Parser;

public class PrologParser
{
    public static Parser<char, string> Tok(string value)
    {
        return Try(String(value)).Before(SkipWhitespaces);
    }
}

public abstract class Term { }

public class Rule : Term
{
    public required Predicate Head { get; init; }
    public ImmutableArray<Predicate> Body { get; } = [];
    public override string? ToString()
    {
        return Body.Length > 0
        ? $"{Head} :- {Body}"
        : Head.ToString();
    }
}

public class Predicate : Term
{
    public string Name { get; }
    public ImmutableArray<Term> Args { get; }

}
public class Atom : Term
{
    public string Value { get; set; }
}
