using System.Collections;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected BaseLevel level;
    private BugSide side;

    public BugSide Side => side;

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

    public virtual void Clear()
    {
    }
}
