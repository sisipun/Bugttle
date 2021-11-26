using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private TileBase tile;
    private Vector2Int position;
    private int cost;
    private Bug bug;
    private CellBehaviour cellBehaviour;

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

    public Cell(Vector2Int position, CellData data)
    {
        this.tile = data.Tile;
        this.position = position;
        this.cost = data.Cost;
        this.cellBehaviour = data.CellBehaviour;
        this.bug = null;
    }

    public Cell(Vector2Int position, CellData data, Bug bug)
    {
        this.tile = data.Tile;
        this.position = position;
        this.cost = data.Cost;
        this.cellBehaviour = data.CellBehaviour;
        this.bug = bug;
    }

    public void OnEndRound()
    {
        cellBehaviour.OnEndRound(this);
    }
}