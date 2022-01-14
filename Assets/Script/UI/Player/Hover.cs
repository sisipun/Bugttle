using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hover : MonoBehaviour
{
    private Tilemap tilemap;
    private int width;
    private int height;

    void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
    }

    public void Init(Map map)
    {
        this.width = map.Width;
        this.height = map.Height;
    }

    public void Set(ICollection<Vector2Int> positions, TileBase tile)
    {
        foreach (Vector2Int position in positions)
        {
            tilemap.SetTile(((Vector3Int)position), tile);
        }
    }

    public void Clear()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
            }
        }
    }
}
