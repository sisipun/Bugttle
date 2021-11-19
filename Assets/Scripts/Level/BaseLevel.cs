using System.Collections.Generic;
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

    public Dictionary<Vector2Int, Path> GetPossibleMoves(Bug bug)
    {
        Dictionary<Vector2Int, Path> moves = new Dictionary<Vector2Int, Path>();
        if (bug.StepsLeft == 0)
        {
            return moves;
        }

        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Vector2Int move = new Vector2Int(x, y);
                Path path = map.FindPath(bug.Position, move, bug.StepsLeft);
                if (path.IsExists)
                {
                    moves.Add(move, path);
                }
            }
        }
        return moves;
    }

    public List<Vector2Int> GetPossibleAttacks(Bug bug)
    {
        List<Vector2Int> attacks = new List<Vector2Int>();
        if (bug.AttacksLeft == 0)
        {
            return attacks;
        }

        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                int range = Mathf.Abs(bug.Position.x - x) + Mathf.Abs(bug.Position.y - y);
                Bug attacked = map.GetBug(x, y);
                if (range <= bug.AttackRange && attacked != null && attacked.Side != bug.Side)
                {
                    attacks.Add(new Vector2Int(x, y));
                }
            }
        }
        return attacks;
    }

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
