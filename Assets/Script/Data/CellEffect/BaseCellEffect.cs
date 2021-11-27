using UnityEngine;

public abstract class BaseCellEffect : ScriptableObject
{
    public abstract void OnEndRound(Cell cell);
}
