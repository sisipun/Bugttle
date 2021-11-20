using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    protected LevelState initialState;
    protected LevelState currentState;
    protected BugSide initialSide;
    protected BugSide currentSide;
    protected Map map;
    protected int turnNumber;

    public BugSide CurrentSide => currentSide;
    public Map LevelMap => map;
    public int RoundNumber => (turnNumber + 1) / 2;

    public virtual void Init(Map map)
    {
        this.initialState = LevelState.SET_POSITIONS;
        this.initialSide = BugSide.GREEN;
        this.map = map;
        Reset();
    }

    public abstract bool IsGameOver();

    public abstract LevelType Type();

    public virtual Dictionary<Vector2Int, Path> GetPossibleMoves(Bug bug)
    {
        Dictionary<Vector2Int, Path> moves = new Dictionary<Vector2Int, Path>();
        if (bug.StepsLeft == 0 || currentState == LevelState.PICK_BUGS)
        {
            return moves;
        }
        else if (currentState == LevelState.SET_POSITIONS)
        {
            return GetInitialPositions();
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

    public virtual List<Vector2Int> GetPossibleAttacks(Bug bug)
    {
        List<Vector2Int> attacks = new List<Vector2Int>();
        if (bug.AttacksLeft == 0 || currentState != LevelState.TURN)
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
        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;
        if (currentState == LevelState.TURN)
        {
            turnNumber++;
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

        CheckForStateChange();
    }

    public void Reset()
    {
        this.currentState = initialState;
        this.currentSide = initialSide;
        this.turnNumber = 1;
    }


    protected virtual void CheckForStateChange()
    {
        if (currentState == LevelState.TURN || currentSide != initialSide)
        {
            return;
        }

        if (currentState == LevelState.PICK_BUGS)
        {
            currentState = LevelState.SET_POSITIONS;
        }
        else if (currentState == LevelState.SET_POSITIONS)
        {
            currentState = LevelState.TURN;
        }
    }

    protected virtual Dictionary<Vector2Int, Path> GetInitialPositions()
    {
        int initialZoneSize = 2;
        int initialLine = currentSide == BugSide.GREEN ? 0 : map.Size - initialZoneSize;
        int endLine = currentSide == BugSide.GREEN ? initialZoneSize : map.Size;
        Dictionary<Vector2Int, Path> initialPositions = new Dictionary<Vector2Int, Path>();
        for (int x = initialLine; x < endLine; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                if (map.GetBug(x, y) == null)
                {
                    initialPositions.Add(new Vector2Int(x, y), new Path());
                }
            }
        }

        return initialPositions;
    }
}
