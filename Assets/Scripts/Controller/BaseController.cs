using System.Collections;
using UnityEngine;

public abstract class BaseController : ScriptableObject
{
    protected GameManager game;
    protected BugSide side;

    public virtual void Init(GameManager game, BugSide side)
    {
        this.game = game;
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
