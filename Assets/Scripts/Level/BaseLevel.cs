using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    protected BugSide currentSide;
    protected Map map;
    protected int turnNumber;

    public BugSide CurrentSide => currentSide;
    public Map LevelMap => map;
    public int RoundNumber => (turnNumber + 1) / 2;

    public virtual void Init(Map map)
    {
        Reset();
        this.map = map;
    }

    public abstract bool IsGameOver();

    public abstract LevelType Type();

    public virtual void Move(Vector2Int from, Vector2Int to, Path path)
    {
        Bug bug = map.GetBug(from);
        map.SetBug(from, null);
        map.SetBug(to, bug);
        bug.Move(to, path);
    }

    public virtual void Attack(Bug source, Bug target)
    {
        source.Attack();
        target.Damage();
        if (target.IsDead)
        {
            map.RemoveBug(target.Position);
        }
    }

    public virtual void EndTurn()
    {
        turnNumber++;
        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null && bug.Side == currentSide)
                {
                    bug.StartTurn();
                }
            }
        }
    }

    public void Reset()
    {
        this.currentSide = BugSide.GREEN;
        this.turnNumber = 1;
    }
}
