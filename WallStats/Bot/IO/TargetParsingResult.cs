namespace WallStats.Bot.IO
{
    public enum TargetParsingResult
    {
        Ok,
        NotExists,
        FewMatches,
        WallClosed,
        WallReadOnly,
        ApplicationLink
    }
}