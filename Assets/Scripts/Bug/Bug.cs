using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    [SerializeField]
    private HealthBar health;

    private SpriteRenderer spriteRenderer;

    private BugSide side;
    private int moveRange;
    private int attackRange;

    private Vector2Int position;
    private int stepsLeft;
    private int attacksLeft;

    public BugSide Side => side;
    public bool IsDead => health.IsDead;
    public Vector2Int Position => position;

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
        this.attacksLeft = 1;
        this.spriteRenderer.sprite = side == BugSide.GREEN ? data.GreenBody : data.RedBody;
        this.health.Init(data.Health, side == BugSide.GREEN ? data.GreenColor : data.RedColor);
    }

    public void Damage()
    {
        health.Damage(1);
    }

    public void Attack()
    {
        attacksLeft--;
        stepsLeft = 0;
    }

    public void StartTurn()
    {
        stepsLeft = moveRange;
        attacksLeft = 1;
    }

    public void Move(Vector2Int newPosition, Path path)
    {
        stepsLeft -= path.Cost;
        position = newPosition;
    }

    public Dictionary<Vector2Int, Path> PossibleMoves(Map map)
    {
        Dictionary<Vector2Int, Path> moves = new Dictionary<Vector2Int, Path>();
        if (stepsLeft == 0)
        {
            return moves;
        }

        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Vector2Int move = new Vector2Int(x, y);
                Path path = map.FindPath(position, move, stepsLeft);
                if (path.IsExists)
                {
                    moves.Add(move, path);
                }
            }
        }
        return moves;
    }

    public List<Vector2Int> PossibleAttacks(Map map)
    {
        List<Vector2Int> attacks = new List<Vector2Int>();
        if (attacksLeft == 0)
        {
            return attacks;
        }

        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                int range = Mathf.Abs(position.x - x) + Mathf.Abs(position.y - y);
                Bug attacked = map.GetBug(x, y);
                if (range <= attackRange && attacked != null && attacked.side != side)
                {
                    attacks.Add(new Vector2Int(x, y));
                }
            }
        }
        return attacks;
    }
}
