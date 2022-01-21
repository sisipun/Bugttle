using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private Tile backTile;
    private Tile frontTile;

    private Vector2Int position;
    private Dictionary<BugGroup, int> costs;
    private Bug bug;
    private BaseCellEffect cellEffect;

    public Tile BackTile => backTile;
    public Tile FrontTile => frontTile;
    public Vector2Int Position => position;
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
        this.costs = new Dictionary<BugGroup, int>();
    }

    public void Init(Vector2Int position, CellData data, Bug bug)
    {
        this.backTile = data.BackTile;
        this.frontTile = data.FrontTile;
        this.position = position;
        this.cellEffect = data.CellEffect;
        this.bug = bug;
        this.costs.Clear();
        foreach (CellData.GroupCost cost in data.GroupsCost)
        {
            this.costs[cost.Group] = cost.Cost;
        }
    }

    public int GetCost(BugGroup group)
    {
        return costs[group];
    }

    public void OnEndRound()
    {
        cellEffect.OnEndRound(this);
    }
}