using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkillEffect", menuName = "Scriptable Objects/New Skill Effect/Attack")]
public class AttackSkillEffect : BaseSkillEffect
{
    public override void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        Map map = level.Map;
        Bug targetBug = map.GetCell(target).Bug;
        bug.Skills[SkillType.ATTACK].Count--;
        bug.Skills[SkillType.MOVE].Range = 0;
        targetBug.DecreaseHealth(1);
        if (targetBug.IsDead)
        {
            level.Kill(targetBug);
        }
    }

    public override List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        if (bug == null || bug.Skills[SkillType.ATTACK].Count == 0 || level.CurrentState != LevelState.TURN)
        {
            return new List<Vector2Int>();
        }
        
        Map map = level.Map;
        List<Vector2Int> targets = new List<Vector2Int>();
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                int range = Mathf.Abs(bug.Position.x - x) + Mathf.Abs(bug.Position.y - y);
                Bug target = map.GetBug(x, y);
                if (range <= bug.Skills[SkillType.ATTACK].Range && target != null && target.Side != bug.Side)
                {
                    targets.Add(new Vector2Int(x, y));
                }
            }
        }
        return targets;
    }

    public override SkillType Type()
    {
        return SkillType.ATTACK;
    }
}
