using UnityEngine;

[CreateAssetMenu(fileName = "FlyAttackSkillEffect", menuName = "Scriptable Objects/New Skill Effect/Fly Attack")]
public class FlyAttackSkillEffect : AttackSkillEffect
{
    public override void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        Map map = level.Map;
        Bug targetBug = map.GetCell(target).Bug;
        bug.Skills[SkillType.ATTACK].Count--;
        targetBug.DecreaseHealth(1);
        if (targetBug.IsDead)
        {
            level.Kill(targetBug);
        }
    }
}
