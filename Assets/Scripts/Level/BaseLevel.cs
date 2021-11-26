using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLevel : MonoBehaviour
{
    protected LevelState initialState;
    protected LevelState currentState;
    protected BugSide initialSide;
    protected BugSide currentSide;
    protected int initialZoneSize;

    protected Map map;
    protected int roundNumber;
    protected Dictionary<BugSide, List<Bug>> bugs;

    public BugSide CurrentSide => currentSide;
    public Map LevelMap => map;
    public int RoundNumber => roundNumber;

    public UnityAction<BugSide> OnEndTurn;
    public UnityAction<BugSide> OnGameOver;

    public virtual void Init(Map map)
    {
        this.initialState = LevelState.SET_POSITIONS;
        this.initialSide = BugSide.GREEN;
        this.initialZoneSize = map.Size / 4;
        this.map = map;
        this.bugs = new Dictionary<BugSide, List<Bug>>();
        bugs[BugSide.GREEN] = new List<Bug>();
        bugs[BugSide.RED] = new List<Bug>();
        for (int x = 0; x < map.Size; x++)
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

    public abstract LevelType Type();

    protected abstract void CheckForGameOver();

    public List<Bug> GetBugs(BugSide side)
    {
        return bugs[side];
    }

    public Dictionary<Vector2Int, Path> GetPossibleMoves(Bug bug)
    {
        if (currentState == LevelState.SET_POSITIONS)
        {
            return GetInitialPositions();
        }
        else
        {
            Dictionary<Vector2Int, Path> moves = new Dictionary<Vector2Int, Path>();
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
    }

    public List<Vector2Int> GetPossibleAttacks(Bug bug)
    {
        if (bug.AttacksLeft == 0 || currentState != LevelState.TURN)
        {
            return new List<Vector2Int>();
        }
        else
        {
            List<Vector2Int> attacks = new List<Vector2Int>();
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
            Kill(target);
        }
    }

    public void Kill(Bug bug)
    {
        map.RemoveBug(bug.Position);
        bugs[bug.Side].Remove(bug);
        CheckForGameOver();
    }

    public void EndTurn()
    {
        if (currentState == LevelState.TURN && currentSide != initialSide)
        {
            EndRound();
        }

        SwitchSide();
        CheckForBugState();
        CheckForStateChange();
        OnEndTurn(currentSide);
        CheckForGameOver();
    }

    public void Reset()
    {
        this.currentState = initialState;
        this.currentSide = initialSide;
        this.roundNumber = 1;
    }

    protected virtual void EndRound()
    {
        roundNumber++;
        map.OnEndRound();
    }

    protected void SwitchSide()
    {
        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;
    }

    protected void CheckForBugState()
    {
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug == null)
                {
                    continue;
                }

                if (bug.IsDead)
                {
                    Kill(bug);
                }
                else if (bug.Side == currentSide)
                {
                    bug.StartTurn();
                }
            }
        }
    }

    protected void CheckForStateChange()
    {
        if (currentState == LevelState.SET_POSITIONS && currentSide == initialSide)
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
