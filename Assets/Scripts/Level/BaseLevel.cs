using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    protected LevelState initialState;
    protected LevelState currentState;
    protected BugSide initialSide;
    protected BugSide currentSide;
    protected int initialZoneSize;

    protected Map map;
    protected int turnNumber;
    protected Dictionary<BugSide, List<Bug>> bugs;

    public BugSide CurrentSide => currentSide;
    public Map LevelMap => map;
    public int RoundNumber => (turnNumber + 1) / 2;

    public virtual void Init(Map map)
    {
        this.initialState = LevelState.SET_POSITIONS;
        this.initialSide = BugSide.GREEN;
        this.initialZoneSize = map.Size / 4;
        this.map = map;
        this.bugs = new Dictionary<BugSide, List<Bug>>();
        bugs[BugSide.GREEN] = new List<Bug>();
        bugs[BugSide.RED] = new List<Bug>();
        for(int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null)
                {
                    bugs[bug.Side].Add(bug);
                }
            }
        }
        Reset();
    }

    public abstract BugSide? GetWinner();

    public abstract LevelType Type();

    public List<Bug> GetBugs(BugSide side)
    {
        return bugs[side];
    }

    public Dictionary<Vector2Int, Path> GetPossibleMoves(Bug bug)
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

    public List<Vector2Int> GetPossibleAttacks(Bug bug)
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

    public void Move(Bug bug, Vector2Int destination, Path path)
    {
        map.SetBug(bug.Position, null);
        map.SetBug(destination, bug);
        bug.Move(destination, path);
    }

    public void Attack(Bug source, Bug target)
    {
        source.Attack();
        target.Damage();
        if (target.IsDead)
        {
            map.RemoveBug(target.Position);
            bugs[target.Side].Remove(target);
        }
    }

    public void EndTurn()
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

    private void CheckForStateChange()
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

    private Dictionary<Vector2Int, Path> GetInitialPositions()
    {
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
