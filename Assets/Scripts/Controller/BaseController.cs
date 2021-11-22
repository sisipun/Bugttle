using System.Collections;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected BaseLevel level;
    protected BugSide side;

    public virtual void Init(BaseLevel level, BugSide side)
    {
        this.level = level;
        this.side = side;
    }

    public virtual void OnStartTurn()
    {
    }
    
    public abstract IEnumerator TurnAction();

    public virtual void OnEndTurn()
    {
    }

    public virtual void Reset()
    {
    }
}
