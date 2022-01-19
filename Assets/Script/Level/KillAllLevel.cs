public class KillAllLevel : BaseLevel
{
    public override LevelType Type()
    {
        return LevelType.KILL_ALL;
    }

    protected override void CheckGameOver()
    {
        if (bugs[BugSide.TOP].Count == 0)
        {
            OnGameOver(BugSide.BOTTOM);
        }
        else if (bugs[BugSide.BOTTOM].Count == 0)
        {
            OnGameOver(BugSide.TOP);
        }
    }
}