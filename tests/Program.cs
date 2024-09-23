using Parser;
using Pidgin;
using TUnit;

public class PidginTests
{
    [Test]
    [Arguments("""status:open""")]
    public async Task CanParserSimpleFilter(string filter)
    {
        var sut = new FilterParser();

        var result = sut.ParseFilterText(filter);
        await Assert.That(result.Success).IsEqualTo(true);
    }

    [Test]
    [Arguments("asd    ", "asd")]
    [Arguments("asd", "asd")]
    [Arguments("aajjhksjdhf                ", "aajjhksjdhf")]
    public async Task CanParseWhiteSpaceThenString(string input, string expected)
    {
        var parser = PrologParser.Tok(expected);

        var result = parser.Parse(input);

        await Assert.That(result.Success).IsEqualTo(true);
    }
}

public static class Consts
{
    public static readonly string[] filters = [
"""status:open""",
"""priority:high AND due:today""",
"""assigned:@johndoe OR assigned:@janedoe""",
"""project:"Website Redesign" AND tag:urgent AND status:!closed""",
"""created:>2023-01-01""",
"""updated:<1w AND priority:medium""",
"""milestone:"Q2 Release" OR milestone:"Q3 Release" """,
"""status:closed AND priority:low AND updated:>1m""",
"""!assigned:@team_lead""",
"""due:>tomorrow AND status:open AND project:"Mobile App" """,
"""tag:bug""",
"""priority:high OR priority:critical OR tag:urgent""",
"""created:2023-04-01..2023-04-30 AND assigned:@me AND !status:closed""",
"""status:in_progress AND !tag:blocked AND due:<2w""",
"""project:"Marketing Campaign" OR project:"Sales Initiative" OR project:"Customer Outreach" """,
"""assigned:@alice AND priority:high AND due:this_week AND !tag:postponed""",
"""updated:yesterday..today""",
"""milestone:"Product Launch" AND status:!closed AND assigned:@product_team""",
"""tag:frontend AND !tag:backend AND priority:medium""",
"""due:<2w AND priority:!low AND assigned:@active_members""",
"""created:last_month""",
"""project:"Customer Support" AND tag:urgent AND status:open AND priority:high""",
"""assigned:@department_heads OR assigned:@team_leads""",
"""status:review""",
"""milestone:"Version 2.0" AND tag:testing AND !assigned:@qa_team""",
"""due:overdue""",
"""project:"Data Migration" AND !tag:completed AND status:in_progress AND updated:>1w""",
"""priority:low AND created:<6m AND !assigned:@intern""",
"""assigned:@qa_team AND status:open AND project:"Bug Fixes" """,
"""tag:documentation OR tag:training OR tag:onboarding""",
"""updated:this_year AND status:closed AND project:"Legacy System" """,
"""project:"Annual Report" AND due:<1m AND assigned:@finance_team AND priority:high""",
"""status:backlog""",
"""milestone:"Q4 Goals" AND assigned:@executive_team AND tag:strategic""",
"""created:today AND priority:high AND !status:closed""",
"""project:"Legacy System" AND tag:deprecated AND status:review""",
"""due:next_week""",
"""status:on_hold AND updated:<1m AND assigned:@project_manager""",
"""tag:security AND priority:critical AND due:today""",
"""assigned:@dev_team AND !status:closed AND project:"New Feature" """,
"""project:"Customer Feedback" AND created:>1w AND status:open""",
"""milestone:"Beta Release" """,
"""due:2023-12-31 AND status:!completed AND priority:high AND assigned:@me""",
"""priority:!low AND assigned:@active_members AND updated:>3d""",
"""tag:ui_ux AND project:"Mobile App" AND status:in_progress""",
"""created:<1m AND !tag:obsolete AND status:open""",
"""assigned:@marketing_team OR assigned:@sales_team""",
"""status:blocked AND priority:high AND due:<1w AND project:"Critical Fix" """,
"""tag:performance AND !status:closed AND updated:this_week""",
"""milestone:"Year-End Review" AND assigned:@manager AND priority:medium AND due:2023-12-15""",
    ];
}
