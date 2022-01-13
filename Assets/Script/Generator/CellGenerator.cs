using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
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

    public void Generate(Map map)
    {
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                CellData cellData = cellsFrequency[Random.Range(0, cellsFrequency.Count)];
                while ((x == 0 || x == map.Size - 1) && cellData.Cost < 0)
                {
                    cellData = cellsFrequency[Random.Range(0, cellsFrequency.Count)];
                }
                map.SetCell(x, y, cellData);
            }
        }
    }
}