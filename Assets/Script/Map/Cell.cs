using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private Tile backTile;
    private Tile frontTile;

    private Vector2Int position;
    private int cost;
    private Bug bug;
    private BaseCellEffect cellEffect;

    public Tile BackTile => backTile;
    public Tile FrontTile => frontTile;
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

    public Cell(Vector2Int position)
    {
        this.position = position;
    }

    public void Init(Vector2Int position, CellData data, Bug bug)
    {
        this.backTile = data.BackTile;
        this.frontTile = data.FrontTile;
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