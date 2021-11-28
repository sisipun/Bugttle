using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseSkillEffect : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private TileBase targetTile;
    
    public Sprite Icon => icon;
    public TileBase TargetTile => targetTile;

    public abstract void Apply(Bug bug, Vector2Int target, BaseLevel level);

    public abstract List<Vector2Int> GetTargets(Bug bug, BaseLevel level);

    public abstract SkillType Type();
}
