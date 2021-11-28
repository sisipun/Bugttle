using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSkillEffect", menuName = "Scriptable Objects/New Skill Effect/Heal")]
public class HealSkillEffect : BaseSkillEffect
{
    public override void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        Map map = level.Map;
        Bug targetBug = map.GetCell(target).Bug;
        bug.Skills[SkillType.HEAL].DecreaseCount(1);
        bug.Skills[SkillType.MOVE].NullifyRange();
        targetBug.IncreaseHealth(1);
    }

    public override List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        if (bug == null || bug.Skills[SkillType.HEAL].Count == 0 || level.CurrentState != LevelState.TURN)
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
                if (range <= bug.Skills[SkillType.HEAL].Range && target != null && target.Side == bug.Side && !target.IsFullHealth)
                {
                    targets.Add(target.Position);
                }
            }
        }

        return targets;
    }

    public override SkillType Type()
    {
        return SkillType.HEAL;
    }
}
