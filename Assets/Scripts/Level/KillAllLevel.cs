public class KillAllLevel : BaseLevel
{
    public override bool IsGameOver()
    {
        bool hasGreen = false;
        bool hasRed = false;
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null)
                {
                    if (bug.Side == BugSide.GREEN)
                    {
                        hasGreen = true;
                    }
                    if (bug.Side == BugSide.RED)
                    {
                        hasRed = true;
                    }
                }
            }
        }

        return !hasGreen || !hasRed;
    }
}