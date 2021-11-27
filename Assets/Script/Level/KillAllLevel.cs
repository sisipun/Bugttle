public class KillAllLevel : BaseLevel
{
    public override LevelType Type()
    {
        return LevelType.KILL_ALL;
    }

    protected override void CheckGameOver()
    {
        if (bugs[BugSide.RED].Count == 0)
        {
            OnGameOver(BugSide.GREEN);
        }
        else if (bugs[BugSide.GREEN].Count == 0)
        {
            OnGameOver(BugSide.RED);
        }
    }
}