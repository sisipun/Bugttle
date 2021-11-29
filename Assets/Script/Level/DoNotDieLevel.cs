using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    public override LevelType Type()
    {
        return LevelType.DO_NOT_DIE;
    }

    public override string CurrentStateText()
    {
        if (CurrentState == LevelState.SET_POSITIONS)
        {
            return string.Format("Set bugs position");
        }
        else if (CurrentState == LevelState.TURN)
        {
            return string.Format(
                "Round: {0}.\n{1} rounds to win",
                RoundNumber,
                roundCount - RoundNumber + 1
            );
        }
        else
        {
            return "";
        }
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