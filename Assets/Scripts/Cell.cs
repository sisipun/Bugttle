using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private Vector2Int position;
    private int cost;
    private Bug bug;

    public Vector2Int Position => position;
    public int Cost => cost;
    public bool HasBug => bug != null;
    public Bug Bug
    {
        get
        {
            return bug;
        }

        set
        {
            bug = value;
        }
    }

    public Cell(Tilemap map, Vector2Int position, CellData data)
    {
        this.position = position;
        this.cost = data.Cost;
        map.SetTile((Vector3Int)this.position, data.Tile);
    }
}