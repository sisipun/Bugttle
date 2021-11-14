using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    [SerializeField]
    private HealthBar health;

    private SpriteRenderer spriteRenderer;

    private Side side;
    private int moveRange;
    private int attackRange;
    
    private Vector2Int position;
    private int stepsLeft;
    private int attacksLeft;

    public bool IsUserSide => side == Side.USER;
    public bool IsDead => health.IsDead;
    public Vector2Int Position => position;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2Int position, Side side, BugData data)
    {
        this.position = position;
        this.side = side;
        this.moveRange = data.MoveRange;
        this.attackRange = data.AttackRange;
        this.stepsLeft = moveRange;
        this.attacksLeft = 1;
        this.spriteRenderer.sprite = IsUserSide ? data.UserBody : data.AiBody;
        this.health.Init(data.Health, IsUserSide ? data.UserColor : data.AiColor);
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

    public void EndTurn()
    {
        stepsLeft = moveRange;
        attacksLeft = 1;
    }

    public void Move(Vector2Int newPosition, Path path) 
    {
        stepsLeft -= path.Cost;
        position = newPosition;
    }

    public Dictionary<Vector2Int, Path> PossibleMoves(Cell[,] map)
    {
        Dictionary<Vector2Int, Path> moves = new Dictionary<Vector2Int, Path>();
        if (stepsLeft == 0)
        {
            return moves;
        }

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Vector2Int move = new Vector2Int(x, y);
                Path path = PathFinder.Find(map, position, move, stepsLeft);
                if (path.IsExists)
                {
                    moves.Add(move, path);
                }
            }
        }
        return moves;
    }

    public List<Vector2Int> PossibleAttacks(Cell[,] map)
    {
        List<Vector2Int> attacks = new List<Vector2Int>();
        if (attacksLeft == 0)
        {
            return attacks;
        }

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int range = Mathf.Abs(position.x - x) + Mathf.Abs(position.y - y);
                Bug attacked = map[x, y].Bug;
                if (range <= attackRange && attacked != null && attacked.side != side)
                {
                    attacks.Add(new Vector2Int(x, y));
                }
            }
        }
        return attacks;
    }

    public enum Side
    {
        USER,
        AI
    }
}
