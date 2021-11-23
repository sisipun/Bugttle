public class KillAllLevel : BaseLevel
{
    public override LevelType Type()
    {
        return LevelType.KILL_ALL;
    }

    protected override void CheckForGameOver()
    {
        if (GetBugs(BugSide.RED).Count == 0)
        {
            OnGameOver(BugSide.GREEN);
        }
        else if (GetBugs(BugSide.GREEN).Count == 0)
        {
            OnGameOver(BugSide.RED);
        }
    }
}