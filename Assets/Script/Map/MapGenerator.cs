using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private CellData[] cells;

    private List<CellData> cellsFrequency;

    void Awake()
    {
        this.cellsFrequency = new List<CellData>();
        foreach (CellData cellData in cells)
        {
            for (int i = 0; i < cellData.Frequency; i++)
            {
                cellsFrequency.Add(cellData);
            }
        }
    }

    public Cell[,] Generate(int size)
    {
        Cell[,] generatedCells = new Cell[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                CellData cellData = cellsFrequency[Random.Range(0, cellsFrequency.Count)];
                while ((x == 0 || x == size - 1) && cellData.Cost < 0)
                {
                    cellData = cellsFrequency[Random.Range(0, cellsFrequency.Count)];
                }
                Cell cell = new Cell(new Vector2Int(x, y), cellData);
                generatedCells[x, y] = cell;
            }
        }

        return generatedCells;
    }
}