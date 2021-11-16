using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<Vector2Int> value;
    private bool exists;
    private int cost;

    public List<Vector2Int> Value => value;
    public bool IsExists => exists;
    public int Cost => cost;

    public Path(List<Vector2Int> value, int cost)
    {
        this.value = value;
        this.cost = cost;
        this.exists = true;
    }

    public Path()
    {
        this.value = new List<Vector2Int>();
        this.cost = 0;
        this.exists = false;
    }
}