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
            skill.Apply(bug, targets[Random.Range(0, targets.Count)], level);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
