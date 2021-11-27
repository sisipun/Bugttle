using UnityEngine;

[CreateAssetMenu(fileName = "DamageCellEffect", menuName = "Scriptable Objects/New Cell Effect/Damage")]
public class DamageCellEffect : BaseCellEffect
{
    public override void OnEndRound(Cell cell)
    {
        if (cell.Bug != null)
        {
            cell.Bug.DecreaseHealth(1);
        }
    }
}
