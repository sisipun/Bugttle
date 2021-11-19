using System.Collections;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected BaseLevel level;
    protected BugSide side;
    protected bool turnEnded;

    public bool IsTurnEnded => turnEnded;

    public virtual void Init(BaseLevel level, BugSide side)
    {
        this.level = level;
        this.side = side;
        this.turnEnded = false;
    }

    public virtual void StartTurn()
    {
        this.turnEnded = false;
    }
    
    public abstract IEnumerator TurnAction();

    public virtual void EndTurn()
    {
    }

    public virtual void Reset()
    {
    }
}
