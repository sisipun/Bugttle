using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    public override void Init(Map map)
    {
        base.Init(map);
    }

    public override bool IsGameOver()
    {
        return roundCount < RoundNumber;
    }

    
    public override LevelType Type()
    {
        return LevelType.DO_NOT_DIE;
    }

    public override void EndTurn()
    {
        base.EndTurn();
    }
}