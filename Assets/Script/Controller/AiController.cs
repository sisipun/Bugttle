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
        foreach (SkillType type in bug.Skills.Keys)
        {
            yield return StartCoroutine(UseSkill(bug, type));
        }
    }

    private IEnumerator UseSkill(Bug bug, SkillType skillType)
    {
        BugSkill attackSkill = bug.Skills[skillType];
        List<Vector2Int> targets = bug.Skills[skillType].GetTargets(bug, level);
        if(targets.Count > 0 && attackSkill.Count > 0)
        {
            attackSkill.Apply(bug, targets[Random.Range(0, targets.Count)], level);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
