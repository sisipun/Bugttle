using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Background background;
    [SerializeField] private BugPool bugPool;

    private Cell[,] map;

    public int Width => width;
    public int Height => height;

    void Awake()
    {
        this.map = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.map[x, y] = new Cell(new Vector2Int(x, y));
            }
        }
    }

    public Path FindPath(Vector2Int soruce, Vector2Int target, int maxCost)
    {
        return PathFinder.Find(map, soruce, target, maxCost);
    }

    public Path FindPath(Vector2Int soruce, Vector2Int target)
    {
        return PathFinder.Find(map, soruce, target, width * height);
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
        Cell cell = GetCell(position);
        cell.Init(position, data, bug);
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
            bugPool.Release(bug);
        }
    }

    public Vector2Int WorldToCell(Vector3 position)
    {
        return background.WorldToCell(position);
    }

    public void OnEndRound()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y].OnEndRound();
            }
        }
    }

    public void Clear()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                RemoveBug(x, y);
            }
        }
        this.background.Clear();
    }
}
