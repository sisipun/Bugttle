using UnityEngine;

public class DoNotDieLevel : BaseLevel
{
    [SerializeField] private int roundCount;

    private int turnsLeft;

    public override void Init(Map map)
    {
        base.Init(map);
        this.turnsLeft = roundCount * 2;
    }

    public override bool IsGameOver()
    {
        return turnsLeft <= 0;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        this.turnsLeft--;
    }
}