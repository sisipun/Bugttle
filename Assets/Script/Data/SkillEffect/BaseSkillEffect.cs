using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillEffect : ScriptableObject
{
    [SerializeField] private Sprite icon;
    
    public Sprite Icon => icon;

    public abstract void Apply(Bug bug, Vector2Int target, BaseLevel level);

    public abstract List<Vector2Int> GetTargets(Bug bug, BaseLevel level);

    public abstract SkillType Type();
}
