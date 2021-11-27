using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private const string ENABLE_OUTLINE_MATERIAL_KEY = "_OutlineEnabled";

    [SerializeField] private HealthBar health;

    private SpriteRenderer spriteRenderer;

    private BugSide side;
    private Vector2Int position;
    private Dictionary<SkillType, BugSkill> skills;

    public BugSide Side => side;
    public bool IsDead => health.IsDead;
    public bool IsFullHealth => health.IsFull;
    public Vector2Int Position => position;
    public Sprite Sprite => spriteRenderer.sprite;
    public int Health => health.Count;
    public Dictionary<SkillType, BugSkill> Skills => skills;

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2Int position, BugSide side, BugData data)
    {
        this.position = position;
        this.side = side;
        this.spriteRenderer.sprite = side == BugSide.GREEN ? data.GreenBody : data.RedBody;
        this.health.Init(data.Health, side == BugSide.GREEN ? data.GreenColor : data.RedColor);

        skills = new Dictionary<SkillType, BugSkill>();
        foreach (SkillData skillData in data.Skills)
        {
            skills.Add(skillData.SkillEffect.Type(), new BugSkill(skillData));
        }
    }

    public void Reset()
    {
        foreach (BugSkill skill in skills.Values)
        {
            skill.Reset();
        }
    }

    public void IncreaseHealth(int count)
    {
        health.IncreaseHealth(count);
    }

    public void DecreaseHealth(int count)
    {
        health.DecreaseHealth(count);
    }

    public void ChangePosition(Vector2Int newPosition)
    {
        position = newPosition;
    }

    public void SetOutlined()
    {
        spriteRenderer.material.SetFloat(ENABLE_OUTLINE_MATERIAL_KEY, 1);
    }

    public void ResetOutlined()
    {
        spriteRenderer.material.SetFloat(ENABLE_OUTLINE_MATERIAL_KEY, 0);
    }
}
