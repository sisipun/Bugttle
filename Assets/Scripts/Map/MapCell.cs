using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCell
{
    private TileBase tile;
    private Vector2Int position;
    private int cost;
    private Bug bug;

    public TileBase Tile => tile;
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

    public MapCell(Vector2Int position, CellData data)
    {
        this.tile = data.Tile;
        this.position = position;
        this.cost = data.Cost;
        this.bug = null;
    }
}