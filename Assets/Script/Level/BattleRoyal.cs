using UnityEngine;

public class BattleRoyal : BaseLevel
{
    [SerializeField] private CellData waterCell;
    [SerializeField] private int waterPeriodInRounds;

    public override LevelType Type()
    {
        return LevelType.BATTLE_ROYAL;
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
                "Round: {0}.\n{1} rounds till water.", 
                RoundNumber, 
                (RoundNumber % waterPeriodInRounds) + 1
            );
        }
        else
        {
            return "";
        }
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

    protected override void OnEndRound()
    {
        if (RoundNumber % waterPeriodInRounds == 0)
        {
            int circleIndex = (RoundNumber / waterPeriodInRounds) - 1;
            for (int x = circleIndex; x < map.Width - circleIndex; x++)
            {
                CellToWater(x, circleIndex);
                CellToWater(x, map.Height - circleIndex - 1);
            }
            for (int y = circleIndex; y < map.Height - circleIndex; y++)
            {
                CellToWater(circleIndex, y);
                CellToWater(map.Width - circleIndex - 1, y);
            }
        }
        base.OnEndRound();
    }

    private void CellToWater(int x, int y)
    {
        map.SetCell(x, y, waterCell);
    }
}
