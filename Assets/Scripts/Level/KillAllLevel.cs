public class KillAllLevel : BaseLevel
{
    public override BugSide? GetWinner()
    {
        if (GetBugs(BugSide.RED).Count == 0)
        {
            return BugSide.GREEN;
        } else if (GetBugs(BugSide.GREEN).Count == 0)
        {
            return BugSide.RED;
        } else {
            return BugSide.GREEN;
        }
    }

    public override LevelType Type()
    {
        return LevelType.KILL_ALL;
    }
}