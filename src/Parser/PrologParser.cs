using System.Collections.Immutable;
using Pidgin;
using static Pidgin.Parser<char>;
using static Pidgin.Parser;

namespace Parser;

public class PrologParser
{
    static Parser<char, T> Tok<T>(Parser<char, T> p)
        => Try(p).Before(SkipWhitespaces);
    static Parser<char, char> Tok(char value)
        => Tok(Char(value));
    static Parser<char, string> Tok(string value)
        => Tok(String(value));
    static readonly Parser<char, char> _comma = Tok(',');
    static readonly Parser<char, char> _openParen = Tok('(');
    static readonly Parser<char, char> _closeParen = Tok(')');
    static readonly Parser<char, char> _dot = Tok('.');
    static readonly Parser<char, string> _colonDash = Tok(":-");
    static Parser<char, string> Name(Parser<char, char> firstLetter)
        => Tok(
            from first in firstLetter
            from rest in OneOf(Letter, Digit, Char('_')).ManyString()
            select first + rest
        );

    static readonly Parser<char, Term> _atom =
    Name(Lowercase)
        .Select(name => (Term)new Atom(name))
        .Labelled("atom");

    static readonly Parser<char, Term> _variable
        = Name(Uppercase.Or(Char('_')))
            .Select(name => (Term)new Variable(name))
            .Labelled("variable");

    static readonly Parser<char, Predicate> _predicate
        = (
            from name in Try(Name(Lowercase).Before(_openParen))
            from args in CommaSeparated(_term).Before(_closeParen)
            select new Predicate(name, args)
        ).Labelled("predicate");

    static Parser<char, ImmutableArray<T>> CommaSeparated<T>(Parser<char, T> p) where T : notnull
        => p.Separated(_comma).Select(x => x.ToImmutableArray());

    static readonly Parser<char, Term> _term = Rec(()
        => OneOf(
            _variable,
            _predicate.Cast<Term>(),
            _atom
        )).Labelled("term");

    static readonly Parser<char, Rule> _rule
        = Map(
            (head, body) => new Rule(head, body),
            _predicate,
            _colonDash
                .Then(CommaSeparatedAtLeastOnce(_predicate))
                .Or(Return(ImmutableArray<Predicate>.Empty))
        )
        .Before(_dot)
        .Labelled("rule");

    static Parser<char, ImmutableArray<T>> CommaSeparatedAtLeastOnce<T>(Parser<char, T> p)
        => p.SeparatedAtLeastOnce(_comma).Select(x => x.ToImmutableArray());

    static readonly Parser<char, ImmutableArray<Rule>> _program =
        from _ in SkipWhitespaces
        from rules in _rule.Many()
        select rules.ToImmutableArray();

    static readonly Parser<char, Predicate> _query = SkipWhitespaces.Then(_predicate);

    public static ImmutableArray<Rule> ParseProgram(string input) => _program.ParseOrThrow(input);
    public static Predicate ParseQuery(string input) => _query.ParseOrThrow(input);
}

public abstract class Term { }

public class Rule : Term
{
    public Rule(Predicate head, ImmutableArray<Predicate> body)
    {
        Head = head;
        Body = body;
    }

    public Predicate Head { get; init; }
    public ImmutableArray<Predicate> Body { get; init; } = [];
    public override string? ToString()
    {
        return Body.Length > 0
        ? $"{Head} :- {string.Join(",", Body)}"
        : Head.ToString();
    }
}

public class Predicate : Term
{
    public Predicate(string name, ImmutableArray<Term> args)
    {
        Name = name;
        Args = args;
    }

    public string Name { get; init; }
    public ImmutableArray<Term> Args { get; init; }

    public override string ToString()
    {
        return $"{Name}({string.Join(",", Args)})";
    }
}
public class Variable : Term
{
    public Variable(string name)
    {
        Name = name;
    }

    public string Name { get; init; }

    public override string ToString()
    {
        return Name;
    }
}
public class Atom : Term
{
    public Atom(string name)
    {
        Value = name;
    }

    public string Value { get; init; }

    public override string ToString()
    {
        return Value;
    }
}
