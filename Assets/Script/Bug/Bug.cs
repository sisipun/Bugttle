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
    public Vector2Int Position => position;
    public bool IsDead => health.IsDead;
    public bool IsFullHealth => health.IsFull;
    public Sprite Sprite => spriteRenderer.sprite;
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

        if (skills == null)
        {
            skills = new Dictionary<SkillType, BugSkill>();
        }
        else
        {
            skills.Clear();
        }
        
        foreach (SkillData skillData in data.Skills)
        {
            skills.Add(skillData.SkillEffect.Type(), new BugSkill(skillData));
        }
    }

    public void ResetSkills()
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

    public void SetPosition(Vector2Int newPosition)
    {
        position = newPosition;
    }

    public void SetOutlined(bool outlined)
    {
        spriteRenderer.material.SetFloat(ENABLE_OUTLINE_MATERIAL_KEY, outlined ? 1 : 0);
    }
}
