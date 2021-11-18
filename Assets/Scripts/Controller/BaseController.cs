using System.Collections;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected BaseLevel level;
    protected LevelUi ui;
    protected BugSide side;

    public virtual void Init(BaseLevel level, LevelUi ui, BugSide side)
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
