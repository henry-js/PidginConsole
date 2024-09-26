using System.Collections.Immutable;
using Pidgin;
using Pidgin.Expression;
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
    static readonly Parser<char, AstNode> _attribute
        = Map(
            (key, _, value) => new AttributeNode(key, value),
            Letter.AtLeastOnceString(),
            _colon,
            Letter.AtLeastOnceString()
        ).Cast<AstNode>();

    public AstNode ParseAttributeFilter(string filterText)
    {
        return _attribute.ParseOrThrow(filterText);
    }
}

public abstract class AstNode { }

public class AttributeNode : AstNode
{
    public AttributeNode(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }

    public override string ToString()
        => $"{Key}:{Value}";
}

public class Value : AstNode
{

}

public class Tag : AstNode
{
    public string Value { get; set; }
}

public enum LogicalOperatorType { And, Or }
public class LogicalExpression : AstNode
{
    public LogicalOperatorType Type { get; init; }
    public AstNode Left { get; init; }
    public AstNode Right { get; init; }
}

public enum TagOperatorType { Pos, Neg }
public class TagOperator : AstNode
{
    public TagOperatorType Type { get; set; }
    public Tag Attribute { get; set; }
}

public record Attribute
{
    public static readonly ImmutableArray<Attribute> Columns = [
        new ("description"){
            Modifiable = true,
            SupportedFormats = [Format.combined, Format.desc, Format.oneline, Format.truncated, Format.count ],
            Format = Format.combined,
            Type = ColumnType.text
        },
        new ("due") {
            Modifiable = true,
            SupportedFormats = [Format.formatted, Format.julian, Format.epoch, Format.iso, Format.age, Format.relative,Format.remaining, Format.countdown],
            Format = Format.formatted,
            Type = ColumnType.date
        },

    ];

    private Attribute(string name) => Name = name;
    public Attribute(ColumnType type, string name) : this(name) => Type = type;
    public bool Modifiable { get; set; }
    public string Name { get; }
    public required ColumnType Type { get; set; }
    public Format[] SupportedFormats { get; private set; } = [];
    public Format Format { get; private set; }
}

public enum ColumnType
{
    text,
    date,
    number
}

public enum Format
{
    list,
    count,
    indicator,
    combined,
    desc,
    oneline,
    truncated,
    formatted,
    julian,
    epoch,
    iso,
    age,
    relative,
    remaining,
    countdown
}
