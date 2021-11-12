using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int turnRange;
    private int attackRange;
    private int health;

    private Vector2Int position;

    public bool IsDead => health == 0;
    public Vector2Int Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2Int position, BugData data)
    {
        this.spriteRenderer.sprite = data.Body;
        this.position = position;
        this.turnRange = data.TurnRange;
        this.attackRange = data.AttackRange;
        this.health = data.Health;
    }

    public void Hit()
    {
        health -= 1;
    }

    public List<Vector2Int> PossibleTurns(Bug[,] map)
    {
        List<Vector2Int> turns = new List<Vector2Int>();
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int range = Mathf.Abs(position.x - x) + Mathf.Abs(position.y - y);
                Vector2Int turn = new Vector2Int(x, y);
                if (range <= turnRange && map[x, y] == null && turn != position)
                {
                    turns.Add(turn);
                }
            }
        }
        return turns;
    }

    public List<Vector2Int> PossibleAttacks(Bug[,] map)
    {
        List<Vector2Int> attacks = new List<Vector2Int>();
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int range = Mathf.Abs(position.x - x) + Mathf.Abs(position.y - y);
                Vector2Int attack = new Vector2Int(x, y);
                if (range <= attackRange && map[x, y] != null && attack != position)
                {
                    attacks.Add(attack);
                }
            }
        }
        return attacks;
    }
}
