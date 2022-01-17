using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    public override LevelType Type()
    {
        return LevelType.DO_NOT_DIE;
    }

    protected override void CheckGameOver()
    {
        if (roundCount < RoundNumber || bugs[BugSide.RED].Count == 0)
        {
            OnGameOver(BugSide.GREEN);
        }
        else if (bugs[BugSide.GREEN].Count == 0)
        {
            OnGameOver(BugSide.RED);
        }
    }
}