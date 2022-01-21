using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveSkillEffect", menuName = "Scriptable Objects/New Skill Effect/Move")]
public class MoveSkillEffect : BaseSkillEffect
{
    public override void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        Map map = level.Map;
        if (level.CurrentState == LevelState.TURN)
        {
            Path path = map.FindPath(bug.Position, target);
            bug.Skills[SkillType.MOVE].DecreaseRange(path.Cost);
        }

        map.SetBug(bug.Position, null);
        map.SetBug(target, bug);
        bug.SetPosition(target);
    }

    public override List<Vector2Int> GetZone(Bug bug, BaseLevel level)
    {
        if (bug == null || bug.Skills[SkillType.MOVE].Range == 0)
        {
            return new List<Vector2Int>();
        }

        if (level.CurrentState == LevelState.SET_POSITIONS)
        {
            return GetInitialPositions(bug, level.Map, level.CurrentSide, 2);
        }

        Map map = level.Map;
        List<Vector2Int> targets = new List<Vector2Int>();
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Vector2Int target = new Vector2Int(x, y);
                Path path = map.FindPath(bug.Position, target, bug.Skills[SkillType.MOVE].Range);
                if (path.IsExists)
                {
                    targets.Add(target);
                }
            }
        }
        return targets;
    }

    public override List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        return GetZone(bug, level);
    }

    private List<Vector2Int> GetInitialPositions(Bug bug, Map map, BugSide currentSide, int zoneSize)
    {
        List<Vector2Int> initialPositions = new List<Vector2Int>();
        int from = currentSide == BugSide.BOTTOM ? 0 : map.Width - zoneSize;
        int to = currentSide == BugSide.BOTTOM ? zoneSize : map.Width;
        for (int x = from; x < to; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map.GetBug(x, y) == null && map.GetCell(x, y).GetCost(bug.Group) >= 0)
                {
                    initialPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return initialPositions;
    }

    public override SkillType Type()
    {
        return SkillType.MOVE;
    }
}
