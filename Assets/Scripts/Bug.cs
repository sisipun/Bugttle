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

    public List<Vector2Int> PossibleTurns(Bug[,] map)
    {
        List<Vector2Int> turns = new List<Vector2Int>();
        for (int i = position.x - turnRange; i <= position.x + turnRange; i++)
        {
            for (int j = position.y - turnRange; j <= position.y + turnRange; j++)
            {
                if (i >= 0 && j >= 0 && i < map.GetLength(0) && j < map.GetLength(1) && map[i, j] == null)
                {
                    turns.Add(new Vector2Int(i, j));
                }
            }
        }
        return turns;
    }
}
