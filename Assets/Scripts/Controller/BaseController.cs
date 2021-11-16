using System.Collections;
using UnityEngine;

public abstract class BaseController : ScriptableObject
{
    protected BaseLevel level;
    protected UserInterface ui;
    protected BugSide side;

    public virtual void Init(BaseLevel level, UserInterface ui, BugSide side)
    {
        this.level = level;
        this.ui = ui;
        this.side = side;
    }

    public virtual void StartTurn()
    {
    }
    public abstract IEnumerator HandleInput();

    public virtual void EndTurn()
    {
    }
}
