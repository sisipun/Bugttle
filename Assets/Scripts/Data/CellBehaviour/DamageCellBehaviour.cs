using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageCellBehaviour", menuName = "Scriptable Objects/New Cell Behaviour/Damage")]
public class DamageCellBehaviour : CellBehaviour
{
    public override void OnEndRound(Cell cell)
    {
        if (cell.Bug != null)
        {
            cell.Bug.Damage();
        }
    }
}
