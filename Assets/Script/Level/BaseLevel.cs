using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLevel : MonoBehaviour
{
    protected LevelState currentState;
    protected BugSide currentSide;
    protected BugSide initialSide;

    protected Map map;
    protected int roundNumber;
    protected Dictionary<BugSide, List<Bug>> bugs;

    public LevelState CurrentState => currentState;
    public BugSide CurrentSide => currentSide;
    public Map Map => map;
    public Dictionary<BugSide, List<Bug>> Bugs => bugs;
    public int RoundNumber => roundNumber;

    public UnityAction<BugSide> OnEndTurn;
    public UnityAction<BugSide> OnGameOver;

    public virtual void Init(Map map)
    {
        this.currentState = LevelState.SET_POSITIONS;
        this.initialSide = BugSide.GREEN;
        this.currentSide = initialSide;
        this.roundNumber = 1;
        this.map = map;

        this.bugs = new Dictionary<BugSide, List<Bug>>();
        bugs[BugSide.GREEN] = new List<Bug>();
        bugs[BugSide.RED] = new List<Bug>();
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null)
                {
                    bugs[bug.Side].Add(bug);
                }
            }
        }
    }

    public abstract LevelType Type();

    protected abstract void CheckGameOver();

    public void Kill(Bug bug)
    {
        map.RemoveBug(bug.Position);
        bugs[bug.Side].Remove(bug);
        CheckGameOver();
    }

    public void EndTurn()
    {
        if (currentState == LevelState.TURN && currentSide != initialSide)
        {
            OnEndRound();
        }

        SwitchSide();
        CheckBugsState();
        CheckStateChange();
        OnEndTurn(currentSide);
        CheckGameOver();
    }

    protected virtual void OnEndRound()
    {
        roundNumber++;
        map.OnEndRound();
    }

    private void SwitchSide()
    {
        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;
    }

    private void CheckBugsState()
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
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
                    bug.ResetSkills();
                }
            }
        }
    }

    private void CheckStateChange()
    {
        if (currentState == LevelState.SET_POSITIONS && currentSide == initialSide)
        {
            currentState = LevelState.TURN;
        }
    }
}
