using UnityEngine;

public class BattleRoyal : BaseLevel
{
    [SerializeField] private CellData fireCell;
    [SerializeField] private int firePeriodInRounds;

    public override LevelType Type()
    {
        return LevelType.BATTLE_ROYAL;
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
        if (RoundNumber % firePeriodInRounds == 0)
        {
            int circleIndex = (RoundNumber / firePeriodInRounds) - 1;
            for (int x = circleIndex; x < map.Size - circleIndex; x++)
            {
                CellToFire(x, circleIndex);
                CellToFire(x, map.Size - circleIndex - 1);
            }
            for (int y = circleIndex; y < map.Size - circleIndex; y++)
            {
                CellToFire(circleIndex, y);
                CellToFire(map.Size - circleIndex - 1, y);
            }
        }
        base.OnEndRound();
    }

    private void CellToFire(int x, int y)
    {
        map.SetCell(x, y, fireCell);
    }
}
