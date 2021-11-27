using System.Collections.Generic;
using UnityEngine;

public class BugSkill
{
    private int initialRange;
    private int initialCount;
    private BaseSkillEffect effect;

    public int Range { get; set; }
    public int Count { get; set; }
    public Sprite Icon => effect.Icon;

    public BugSkill(SkillData data)
    {
        this.initialRange = data.Range;
        this.initialCount = data.Count;
        this.Range = initialRange;
        this.Count = initialCount;
        this.effect = data.SkillEffect;
    }

    public void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        effect.Apply(bug, target, level);
    }

    public List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        return effect.GetTargets(bug, level);
    }

    public SkillType Type()
    {
        return effect.Type();
    }

    public void Reset()
    {
        this.Range = initialRange;
        this.Count = initialCount;
    }
}