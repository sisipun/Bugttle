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
        if (roundCount < RoundNumber || bugs[BugSide.TOP].Count == 0)
        {
            OnGameOver(BugSide.BOTTOM);
        }
        else if (bugs[BugSide.BOTTOM].Count == 0)
        {
            OnGameOver(BugSide.TOP);
        }
    }
}