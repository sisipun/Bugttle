using UnityEngine;

[CreateAssetMenu(fileName = "NewCellBehaviour", menuName = "Scriptable Objects/New Cell Behaviour/Default")]
public class CellBehaviour : ScriptableObject
{
    public virtual void OnEndRound(Cell cell)
    {
    }
}
