using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BugSkill
{
    private int initialRange;
    private int initialCount;
    private int range;
    private int count;
    private BaseSkillEffect effect;

    public int Range => range;
    public int Count => count;
    public Sprite Icon => effect.Icon;
    public TileBase TargetTile => effect.TargetTile;
    public TileBase ZoneTile => effect.ZoneTile;

    public BugSkill(SkillData data)
    {
        this.initialRange = data.Range;
        this.initialCount = data.Count;
        this.range = initialRange;
        this.count = initialCount;
        this.effect = data.SkillEffect;
    }

    public void Apply(Bug bug, Vector2Int target, BaseLevel level)
    {
        effect.Apply(bug, target, level);
    }

    public List<Vector2Int> GetZone(Bug bug, BaseLevel level)
    {
        return effect.GetZone(bug, level);
    }

    public List<Vector2Int> GetTargets(Bug bug, BaseLevel level)
    {
        return effect.GetTargets(bug, level);
    }

    public SkillType Type()
    {
        return effect.Type();
    }

    public void DecreaseRange(int value)
    {
        range -= value;
        if (range < 0)
        {
            range = 0;
        }
    }

    public void NullifyRange()
    {
        range = 0;
    }

    public void DecreaseCount(int value)
    {
        count -= value;
        if (count < 0)
        {
            count = 0;
        }
    }

    public void NullifyCount()
    {
        count = 0;
    }

    public void Reset()
    {
        this.range = initialRange;
        this.count = initialCount;
    }
}