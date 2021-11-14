using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int size = 8;
    [SerializeField] private CellData[] cells;

    private Cell[,] map;

    public int Size => size;

    public void Init()
    {
        this.map = new Cell[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Cell cell = new Cell(new Vector2Int(x, y), cells[Random.Range(0, cells.Length)]);
                map[x, y] = cell;
            }
        }
    }

    public Path FindPath(Vector2Int soruce, Vector2Int target, int maxCost)
    {
        return PathFinder.Find(map, soruce, target, maxCost);
    }

    public Cell GetCell(Vector2Int position)
    {
        return map[position.x, position.y];
    }

    public Bug GetBug(Vector2Int position)
    {
        return GetBug(position.x, position.y);
    }

    public Bug GetBug(int x, int y)
    {
        return map[x, y].Bug;
    }

    public void SetBug(int x, int y, Bug bug)
    {
        map[x, y].Bug = bug;
    }

    public void SetBug(Vector2Int position, Bug bug)
    {
        SetBug(position.x, position.y, bug);
    }
}
