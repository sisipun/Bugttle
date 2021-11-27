using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private TileBase tile;
    private Vector2Int position;
    private int cost;
    private Bug bug;
    private BaseCellEffect cellEffect;

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
        this.cellEffect = data.CellEffect;
        this.bug = null;
    }

    public Cell(Vector2Int position, CellData data, Bug bug)
    {
        this.tile = data.Tile;
        this.position = position;
        this.cost = data.Cost;
        this.cellEffect = data.CellEffect;
        this.bug = bug;
    }

    public void OnEndRound()
    {
        cellEffect.OnEndRound(this);
    }
}