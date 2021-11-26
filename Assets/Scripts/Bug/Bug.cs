using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private const string ENABLE_OUTLINE_MATERIAL_KEY = "_OutlineEnabled";

    [SerializeField] private HealthBar health;

    protected SpriteRenderer spriteRenderer;

    protected BugSide side;
    protected int moveRange;
    protected int attackRange;
    protected int attacksCount;

    protected Vector2Int position;
    protected int stepsLeft;
    protected int attacksLeft;

    public BugSide Side => side;
    public bool IsDead => health.IsDead;
    public Vector2Int Position => position;
    public int StepsLeft => stepsLeft;
    public int AttacksLeft => attacksLeft;
    public Sprite Sprite => spriteRenderer.sprite;
    public int Health => health.Count;
    public int MoveRange => moveRange;
    public int AttackRange => attackRange;

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2Int position, BugSide side, BugData data)
    {
        this.position = position;
        this.side = side;
        this.moveRange = data.MoveRange;
        this.attackRange = data.AttackRange;
        this.stepsLeft = moveRange;
        this.attacksCount = data.AttacksCount;
        this.attacksLeft = attacksCount;
        this.spriteRenderer.sprite = side == BugSide.GREEN ? data.GreenBody : data.RedBody;
        this.health.Init(data.Health, side == BugSide.GREEN ? data.GreenColor : data.RedColor);
    }

    public void Damage()
    {
        health.Damage(1);
    }

    public virtual void Attack()
    {
        attacksLeft--;
        stepsLeft = 0;
    }

    public void StartTurn()
    {
        stepsLeft = moveRange;
        attacksLeft = attacksCount;
    }

    public void Move(Vector2Int newPosition, Path path)
    {
        stepsLeft -= path.Cost;
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
