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
            bug.Skills[SkillType.MOVE].Range -= path.Cost;
        }
        
        map.SetBug(bug.Position, null);
        map.SetBug(target, bug);
        bug.ChangePosition(target);
    }

    public override List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        if (bug == null || bug.Skills[SkillType.MOVE].Range == 0)
        {
            return new List<Vector2Int>();
        }

        if (level.CurrentState == LevelState.SET_POSITIONS)
        {
            return GetInitialPositions(level.Map, level.CurrentSide, 2);
        }

        Map map = level.Map;
        List<Vector2Int> targets = new List<Vector2Int>();
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
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

    private List<Vector2Int> GetInitialPositions(Map map, BugSide currentSide, int zoneSize)
    {
        List<Vector2Int> initialPositions = new List<Vector2Int>();
        int from = currentSide == BugSide.GREEN ? 0 : map.Size - zoneSize;
        int to = currentSide == BugSide.GREEN ? zoneSize : map.Size;
        for (int x = from; x < to; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                if (map.GetBug(x, y) == null)
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
