using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    public override LevelType Type()
    {
        return LevelType.DO_NOT_DIE;
    }

    protected override void CheckForGameOver()
    {
        if (roundCount < RoundNumber || GetBugs(BugSide.RED).Count == 0)
        {
            OnGameOver.Invoke(BugSide.GREEN);
        }
        else if (GetBugs(BugSide.GREEN).Count == 0)
        {
            OnGameOver.Invoke(BugSide.RED);
        }
    }
}