using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private CellData[] cells;
    [SerializeField] private Background background;

    private Cell[,] map;

    public int Size => size;

    public void Init()
    {
        this.map = new Cell[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                CellData cellData = cells[Random.Range(0, cells.Length)];
                while((x == 0 || x == size - 1) && cellData.Cost < 0)
                {
                    cellData = cells[Random.Range(0, cells.Length)];
                }
                Cell cell = new Cell(new Vector2Int(x, y), cellData);
                map[x, y] = cell;
            }
        }
        this.background.Init(this);
    }

    public Path FindPath(Vector2Int soruce, Vector2Int target, int maxCost)
    {
        return PathFinder.Find(map, soruce, target, maxCost);
    }

    public Path FindPath(Vector2Int soruce, Vector2Int target)
    {
        return PathFinder.Find(map, soruce, target, Size * Size);
    }

    public Cell GetCell(Vector2Int position)
    {
        return GetCell(position.x, position.y);
    }

    public Cell GetCell(int x, int y)
    {
        return map[x, y];
    }

    public void SetCell(int x, int y, CellData data)
    {
        SetCell(new Vector2Int(x, y), data);
    }

    public void SetCell(Vector2Int position, CellData data)
    {
        Bug bug = GetBug(position);
        Cell cell = new Cell(position, data, bug);
        map[position.x, position.y] = cell;
        background.SetCell(cell);
    }

    public Bug GetBug(Vector2Int position)
    {
        return GetBug(position.x, position.y);
    }

    public Bug GetBug(int x, int y)
    {
        return map[x, y].Bug;
    }

    public void SetBug(Vector2Int position, Bug bug)
    {
        SetBug(position.x, position.y, bug);
    }

    public void SetBug(int x, int y, Bug bug)
    {
        map[x, y].Bug = bug;
        if (bug != null)
        {
            bug.transform.position = background.CellToWorld(new Vector2Int(x, y));
        }
    }

    public void RemoveBug(Vector2Int position)
    {
        RemoveBug(position.x, position.y);
    }

    public void RemoveBug(int x, int y)
    {
        Bug bug = GetBug(x, y);
        if (bug != null)
        {
            SetBug(x, y, null);
            Destroy(bug.gameObject);
        }
    }

    public Vector2Int WorldToCell(Vector3 position)
    {
        return background.WorldToCell(position);
    }

    public void OnEndRound()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                map[x, y].OnEndRound();
            }
        }
    }

    public void Clear()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                RemoveBug(x, y);
            }
        }
        this.background.Clear();
    }
}
