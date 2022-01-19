using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : BaseController
{
    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
    }

    public override IEnumerator TurnAction()
    {
        foreach (Bug bug in level.Bugs[Side])
        {
            yield return StartCoroutine(MakeBugTurn(bug));
        }

        level.EndTurn();
    }

    private IEnumerator MakeBugTurn(Bug bug)
    {
        List<SkillType> shuffeledSkills = new List<SkillType>(bug.Skills.Keys);
        shuffeledSkills.Sort((a, b) => 1 - 2 * Random.Range(0, 2));
        foreach (SkillType skill in shuffeledSkills)
        {
            yield return StartCoroutine(UseSkill(bug, skill));
        }
    }

    private IEnumerator UseSkill(Bug bug, SkillType skillType)
    {
        BugSkill skill = bug.Skills[skillType];
        List<Vector2Int> targets = bug.Skills[skillType].GetTargets(bug, level);
        if (targets.Count > 0 && skill.Count > 0)
        {
            Vector2Int target = skillType == SkillType.MOVE
                ? GetMovePosition(bug, targets)
                : targets[Random.Range(0, targets.Count)];
            skill.Apply(bug, target, level);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private Vector2Int GetMovePosition(Bug bug, List<Vector2Int> targets)
    {
        List<Bug> enemyBugs = level.Bugs[Side == BugSide.TOP ? BugSide.BOTTOM : BugSide.TOP];
        List<Vector2Int> enemyBugPositions = new List<Vector2Int>();
        foreach (Bug enemyBug in enemyBugs)
        {
            enemyBugPositions.Add(enemyBug.Position);
        }

        return MinRange(MinRange(bug.Position, enemyBugPositions), targets);
    }

    private Vector2Int MinRange(Vector2Int source, List<Vector2Int> targets)
    {
        Vector2Int minRangeTarget = targets[0];
        int minRange = Mathf.Abs(source.x - minRangeTarget.x) + Mathf.Abs(source.y - minRangeTarget.y);
        for (int i = 1; i < targets.Count; i++)
        {
            Vector2Int target = targets[i];
            int range = Mathf.Abs(source.x - target.x) + Mathf.Abs(source.y - target.y);
            if (range < minRange)
            {
                minRange = range;
                minRangeTarget = target;
            }
        }
        return minRangeTarget;
    }
}
