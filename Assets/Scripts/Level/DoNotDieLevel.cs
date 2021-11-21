using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    public override void Init(Map map)
    {
        base.Init(map);
    }

    public override BugSide? GetWinner()
    {
        if (roundCount < RoundNumber || GetBugs(BugSide.RED).Count == 0)
        {
            return BugSide.GREEN;
        } else if (GetBugs(BugSide.GREEN).Count == 0)
        {
            return BugSide.RED;
        } else 
        {
            return null;
        }
    }
    
    public override LevelType Type()
    {
        return LevelType.DO_NOT_DIE;
    }
}